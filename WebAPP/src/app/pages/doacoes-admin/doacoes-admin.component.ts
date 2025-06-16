import { Component, OnInit } from '@angular/core';
import { CommonModule } from "@angular/common";
import { forkJoin } from 'rxjs';

// Models e Serviços
import { DoacoesAdminService, DonationSummary } from './doacoes-admin.service';
import { IDoacao } from '../doacoes/model/IDoacao';
import { MessageService } from 'primeng/api';

// Módulos do PrimeNG para o Template
import { TableModule } from "primeng/table";
import { ButtonModule } from "primeng/button";
import { InputTextModule } from "primeng/inputtext";
import { TagModule } from 'primeng/tag';
import { ToastModule } from 'primeng/toast';
import { ChartModule } from 'primeng/chart';

@Component({
  selector: 'app-doacoes-admin',
  standalone: true,
  imports: [
    // Módulos essenciais
    CommonModule,

    // Módulos do PrimeNG
    TableModule,
    ButtonModule,
    InputTextModule,
    TagModule,
    ToastModule,
    ChartModule
  ],
  templateUrl: './doacoes-admin.component.html',
  styleUrls: ['./doacoes-admin.component.scss'],
  providers: [MessageService] // Fornece o serviço de notificações para este componente
})
export class DoacoesAdminComponent implements OnInit {

  donations: IDoacao[] = [];
  summary?: DonationSummary;

  pieChartData: any;
  pieChartOptions: any;

  isLoading = true;

  constructor(
      private doacoesAdminService: DoacoesAdminService,
      private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.loadAllData();
  }

  loadAllData(): void {
    this.isLoading = true;

    // Usando forkJoin para buscar os dados da tabela e dos gráficos em paralelo
    forkJoin({
      donations: this.doacoesAdminService.getAllDonations(),
      summary: this.doacoesAdminService.getDonationSummary()
    }).subscribe({
      next: (data) => {
        this.donations = data.donations;
        this.summary = data.summary;
        this.setupPieChart(); // Configura os dados do gráfico
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Erro ao buscar dados do dashboard:', err);
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível carregar os dados.' });
        this.isLoading = false;
      }
    });
  }

  // Configura os dados e opções para o gráfico de pizza
  setupPieChart(): void {
    const documentStyle = getComputedStyle(document.documentElement);
    const backgroundColors = this.summary?.donationsByStatus.map(d => {
      return this.getStatusColor(d.status, documentStyle);
    });

    this.pieChartData = {
      labels: this.summary?.donationsByStatus.map(d => d.status),
      datasets: [
        {
          data: this.summary?.donationsByStatus.map(d => d.count),
          backgroundColor: backgroundColors,
        }
      ]
    };

    this.pieChartOptions = {
      plugins: {
        legend: {
          labels: {
            usePointStyle: true,
            color: documentStyle.getPropertyValue('--text-color')
          }
        }
      }
    };
  }

  // Mapeia o status a uma cor do tema do PrimeNG
  getStatusColor(status: string, style: CSSStyleDeclaration): string {
    switch (status.toLowerCase()) {
      case 'paid': return style.getPropertyValue('--green-500');
      case 'pending': return style.getPropertyValue('--yellow-500');
      case 'awaitingpayment': return style.getPropertyValue('--orange-500');
      case 'processingpayment': return style.getPropertyValue('--cyan-500');
      default: return style.getPropertyValue('--gray-500');
    }
  }

  // Função para formatar o status para exibição na tabela
  getStatusSeverity(status: string): string {
    switch (status.toLowerCase()) {
      case 'paid': return 'success';
      case 'pending':
      case 'awaitingpayment': return 'warning';
      case 'failed': return 'danger';
      case 'processingpayment': return 'info';
      default: return 'secondary';
    }
  }
}
