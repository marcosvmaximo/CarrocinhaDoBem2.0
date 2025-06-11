using System;
using System.Text;
using System.Threading.Tasks;
using webApi.Context;
using webApi.Models;

namespace webApi.Services
{
    public class PixService : IPixService
    {
        private readonly DataContext _context;

        // --- INFORMAÇÕES DA ONG (Configure aqui) ---
        private const string PixKey = "a3325f4e-e2fe-42f3-9d63-c56ed2c0c1f2"; // Chave PIX da ONG (E-mail, CPF, CNPJ ou Chave Aleatória)
        private const string MerchantName = "ONG CARROCINHA DO BEM"; // Nome do beneficiário (até 25 caracteres)
        private const string MerchantCity = "CURITIBA"; // Cidade do beneficiário (até 15 caracteres)
        private const string TxtId = "***"; // Para PIX estático, é comum usar '***'

        public PixService(DataContext context)
        {
            _context = context;
        }

        public async Task<CreatePixChargeResponse> CreatePixChargeAsync(CreatePixChargeRequest request)
        {
            try
            {
                // Constrói a string do payload do PIX seguindo o padrão EMV-QRCPS-MPM
                var payload = new StringBuilder();
                payload.Append(FormatField("00", "01")); // Payload Format Indicator
                payload.Append(BuildMerchantAccountInfo()); // Merchant Account Information
                payload.Append(FormatField("52", "0000")); // Merchant Category Code (0000 para não especificado)
                payload.Append(FormatField("53", "986")); // Transaction Currency (986 para BRL)
                payload.Append(FormatField("54", request.Amount.ToString("F2"))); // Transaction Amount
                payload.Append(FormatField("58", "BR")); // Country Code
                payload.Append(FormatField("59", MerchantName)); // Merchant Name
                payload.Append(FormatField("60", MerchantCity)); // Merchant City
                payload.Append(BuildAdditionalData(request.DonationId)); // Additional Data Field
                
                // Calcula o CRC16
                string crc16 = CalculateCrc16(payload.ToString());
                payload.Append(FormatField("63", crc16));

                var pixPayload = payload.ToString();

                // Cria o registro da transação PIX no banco de dados
                var newPixTransaction = new PixTransaction
                {
                    DonationId = request.DonationId,
                    Amount = request.Amount,
                    TransactionId = TxtId, // Em um PIX Estático, o TxId não é único por transação
                    QrCode = pixPayload,
                    CopiaECola = pixPayload,
                    CreationDate = DateTime.UtcNow,
                    ExpirationDate = DateTime.UtcNow.AddHours(24), // Define uma expiração de 24h
                    Status = "AwaitingPayment"
                };

                _context.PixTransactions.Add(newPixTransaction);
                await _context.SaveChangesAsync();

                // Retorna a resposta com sucesso
                return new CreatePixChargeResponse
                {
                    TransactionId = newPixTransaction.TransactionId,
                    QrCode = newPixTransaction.QrCode,
                    CopiaECola = newPixTransaction.CopiaECola,
                    ExpirationDate = newPixTransaction.ExpirationDate.Value,
                    Status = newPixTransaction.Status
                };
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna a mensagem de erro
                return new CreatePixChargeResponse
                {
                    ErrorMessage = $"Erro interno ao criar payload PIX: {ex.Message}"
                };
            }
        }
        
        // Simulação de consulta de status (para uma implementação real, consultaria a API do banco)
        public Task<PixTransaction> GetPixTransactionStatusAsync(string transactionId)
        {
            throw new NotImplementedException("Consulta de status não aplicável para PIX estático simulado.");
        }

        // Simulação de processamento de webhook (para uma implementação real, receberia notificações do banco)
        public Task ProcessPixWebhookAsync(object webhookPayload)
        {
             // Aqui, você decodificaria o 'webhookPayload', encontraria a 'PixTransaction' e a 'Donation'
             // correspondente e atualizaria o status de ambas para 'Paid'.
            throw new NotImplementedException("Processamento de webhook não aplicável para PIX estático simulado.");
        }

        #region Métodos Auxiliares para Geração do Payload
        private string BuildMerchantAccountInfo()
        {
            var gui = "br.gov.bcb.pix";
            var key = PixKey;
            
            var part1 = FormatField("00", gui);
            var part2 = FormatField("01", key);

            return FormatField("26", $"{part1}{part2}");
        }

        private string BuildAdditionalData(int donationId)
        {
            // O TxId em um pix estático é geralmente '***', mas podemos usar o ID da doação para referência
            // se o aplicativo de pagamento do usuário o exibir.
            var txId = $"DOACAO{donationId:D8}"; 
            var value = FormatField("05", txId);
            return FormatField("62", value);
        }

        private string FormatField(string id, string value)
        {
            var len = value.Length.ToString("D2");
            return $"{id}{len}{value}";
        }
        
        private string CalculateCrc16(string data)
        {
            ushort crc = 0xFFFF;
            ushort polynomial = 0x1021;

            foreach (byte b in Encoding.UTF8.GetBytes(data))
            {
                crc ^= (ushort)(b << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                    {
                        crc = (ushort)((crc << 1) ^ polynomial);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            return crc.ToString("X4");
        }
        #endregion
    }
}