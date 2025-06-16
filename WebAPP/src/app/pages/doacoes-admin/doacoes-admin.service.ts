import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { apiUrl } from '../../commons/api';
import { IDoacao } from "../doacoes/model/IDoacao";

// Interface para os dados do sumário que vêm da API
export interface DonationSummary {
    totalDonations: number;
    totalValuePaid: number;
    donationsByStatus: { status: string, count: number }[];
}

@Injectable({
    providedIn: 'root'
})
export class DoacoesAdminService {

    private readonly apiUrl = `${apiUrl}/Donation`;

    constructor(private http: HttpClient) { }

    /**
     * Busca todas as doações para a tabela.
     * Requer que o utilizador esteja autenticado com um token de Admin.
     * @returns Um Observable com um array de doações.
     */
    getAllDonations(): Observable<IDoacao[]> {
        return this.http.get<IDoacao[]>(`${this.apiUrl}/admin/all`);
    }

    /**
     * Busca os dados agregados para os gráficos e cartões do dashboard.
     * Requer que o utilizador esteja autenticado com um token de Admin.
     * @returns Um Observable com o sumário das doações.
     */
    getDonationSummary(): Observable<DonationSummary> {
        return this.http.get<DonationSummary>(`${this.apiUrl}/admin/summary`);
    }
}
