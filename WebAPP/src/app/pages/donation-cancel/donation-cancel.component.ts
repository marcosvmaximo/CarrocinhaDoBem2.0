import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-donation-cancel',
  standalone: true,
  imports: [
    RouterLink,
    ButtonModule
  ],
  templateUrl: './donation-cancel.component.html',
  styleUrls: ['./donation-cancel.component.scss']
})
export class DonationCancelComponent {
  // Este componente também é apenas para exibição.
}
