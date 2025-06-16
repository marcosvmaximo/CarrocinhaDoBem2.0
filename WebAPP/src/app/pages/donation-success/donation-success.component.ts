import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-donation-success',
  standalone: true,
  imports: [
    RouterLink,
    ButtonModule
  ],
  templateUrl: './donation-success.component.html',
  styleUrls: ['./donation-success.component.scss']
})
export class DonationSuccessComponent {
  // Este componente não precisa de lógica complexa,
  // apenas exibe a mensagem de sucesso.
}
