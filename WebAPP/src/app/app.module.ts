import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'; // Importa HTTP_INTERCEPTORS
import { RouterModule } from '@angular/router';

// --- Módulos do PrimeNG ---
import { ToastModule } from 'primeng/toast';
import { InputNumberModule } from 'primeng/inputnumber';
import { ButtonModule } from 'primeng/button';

// --- Módulos e Componentes do seu Projeto ---
import { routes } from './app.routes';
import { AppComponent } from './app.component';
import { AppLayoutModule } from './layout/app.layout.module';
import { AuthInterceptor } from './commons/interceptors/auth.interceptor'; // Importa o novo interceptor

// --- Componentes Standalone ---
import { DoacoesCadastroComponent } from './pages/doacoes-cadastro/doacoes-cadastro.component';
import { DonationSuccessComponent } from './pages/donation-success/donation-success.component';
import { DonationCancelComponent } from './pages/donation-cancel/donation-cancel.component';
import { NotfoundComponent } from './demo/components/notfound/notfound.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { CadastroComponent } from './pages/auth/cadastro/cadastro.component'; // Importe o CadastroComponent

@NgModule({
  declarations: [
    AppComponent,
    DoacoesCadastroComponent,
    NotfoundComponent
    // O array de declarações fica mais vazio, pois a maioria dos componentes é standalone.
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

    // Componentes Standalone são importados aqui, como se fossem módulos
    DonationSuccessComponent,
    DonationCancelComponent,
    LoginComponent,
    CadastroComponent
  ],
  providers: [
    // Registra o AuthInterceptor para ser usado em todas as requisições HTTP
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
