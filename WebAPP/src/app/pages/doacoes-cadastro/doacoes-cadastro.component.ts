import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DoacoesService, DonationRequest } from './doacoes.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-doacoes-cadastro',
  templateUrl: './doacoes-cadastro.component.html',
  providers: [MessageService]
})
export class DoacoesCadastroComponent {

  donationForm: FormGroup;
  isLoading = false;

  constructor(
      private fb: FormBuilder,
      private doacoesService: DoacoesService,
      private messageService: MessageService
  ) {
    this.donationForm = this.fb.group({
      donationValue: [null, [Validators.required, Validators.min(1.00)]] // Valor mínimo de R$1,00
    });
  }

  submitDonation(): void {
    if (this.donationForm.invalid) {
      this.messageService.add({ severity: 'warn', summary: 'Atenção', detail: 'Por favor, insira um valor válido para a doação.' });
      return;
    }

    this.isLoading = true;

    const donationData: DonationRequest = {
      donationValue: this.donationForm.value.donationValue,
      description: 'Doação para cuidados gerais',
      institutionId: 1, // Fixo para a instituição principal
    };

    // ETAPA 1: Criar o registro da doação
    this.doacoesService.createDonation(donationData).subscribe({
      next: (createdDonation) => {
        // ETAPA 2: Iniciar a sessão de checkout
        this.doacoesService.createCheckoutSession(createdDonation.id).subscribe({
          next: (checkoutResponse) => {
            // Sucesso! Redireciona o usuário para a página de pagamento do PSP
            window.location.href = checkoutResponse.checkoutUrl;
          },
          error: (err) => {
            console.error('Erro ao criar sessão de checkout:', err);
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível iniciar o pagamento. Tente novamente mais tarde.' });
            this.isLoading = false;
          }
        });
      },
      error: (err) => {
        console.error('Erro ao criar doação:', err);
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Ocorreu um problema ao registrar sua doação.' });
        this.isLoading = false;
      }
    });
  }
}
