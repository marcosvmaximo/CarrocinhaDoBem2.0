import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { apiUrl } from '../../commons/api';

// Interface para criar a doação (continua a mesma)
export interface DonationRequest {
  donationValue: number;
  description: string;
  institutionId: number;
}

// Interface para a resposta da criação da doação (continua a mesma)
export interface DonationResponse {
  id: number;
  donationValue: number;
  status: string;
}

// Interface para a resposta do endpoint de checkout
export interface CheckoutResponse {
  checkoutUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class DoacoesService {

  private readonly apiUrl = `${apiUrl}/Donation`; // URL do controller

  constructor(private http: HttpClient) { }

  /**
   * ETAPA 1: Cria o registro da doação no banco.
   * @param donationData Os dados da doação (valor, descrição).
   * @returns Um Observable com a doação criada, incluindo seu ID.
   */
  createDonation(donationData: DonationRequest): Observable<DonationResponse> {
    return this.http.post<DonationResponse>(this.apiUrl, donationData);
  }

  /**
   * ETAPA 2: Inicia a sessão de checkout para uma doação existente.
   * @param donationId O ID da doação criada na Etapa 1.
   * @returns Um Observable com a URL de checkout do PSP.
   */
  createCheckoutSession(donationId: number): Observable<CheckoutResponse> {
    // Chama o novo endpoint: /api/Donation/{id}/checkout
    return this.http.post<CheckoutResponse>(`${this.apiUrl}/${donationId}/checkout`, {});
  }
}
