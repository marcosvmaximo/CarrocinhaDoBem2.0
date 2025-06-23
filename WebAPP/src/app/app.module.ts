// WebAPP/src/app/app.module.ts

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// --- Módulos do PrimeNG ---
import { ToastModule } from 'primeng/toast';
import { InputNumberModule } from 'primeng/inputnumber';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api'; // 1. IMPORTE O SERVIÇO AQUI

// --- Módulos e Componentes do seu Projeto ---
import { routes } from './app.routes';
import { AppComponent } from './app.component';
import { AppLayoutModule } from './layout/app.layout.module';
import { AuthInterceptor } from './commons/interceptors/auth.interceptor';

// --- Componentes Standalone ---
import { DoacoesCadastroComponent } from './pages/doacoes-cadastro/doacoes-cadastro.component';
import { DonationSuccessComponent } from './pages/donation-success/donation-success.component';
import { DonationCancelComponent } from './pages/donation-cancel/donation-cancel.component';
import { NotfoundComponent } from './demo/components/notfound/notfound.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { CadastroComponent } from './pages/auth/cadastro/cadastro.component';

@NgModule({
  declarations: [
    AppComponent,
    DoacoesCadastroComponent,
    NotfoundComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppLayoutModule,
    RouterModule.forRoot(routes),
    ToastModule,
    InputNumberModule,
    ButtonModule,
    DonationSuccessComponent,
    DonationCancelComponent,
    LoginComponent,
    CadastroComponent
  ],
  providers: [
    // 2. ADICIONE O MessageService AQUI
    MessageService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }