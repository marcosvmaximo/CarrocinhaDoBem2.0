import { Routes } from '@angular/router';
import { AppLayoutComponent } from "./layout/app.layout.component";
import { LadingPageComponent } from "./pages/lading-page/lading-page.component";
import { LadingPageHomeComponent } from "./pages/lading-page-home/lading-page-home.component";
import { CatalogoAdocaoComponent } from "./pages/catalogo-adocao/catalogo-adocao.component";
import { SobreComponent } from "./pages/sobre/sobre.component";
import { ContatoComponent } from "./pages/contato/contato.component";
import { NossaMissaoComponent } from "./pages/nossa-missao/nossa-missao.component";
import { ConfiguracaoComponent } from "./pages/configuracao/configuracao.component";
import { LoginComponent } from "./pages/auth/login/login.component";
import { CadastroComponent } from "./pages/auth/cadastro/cadastro.component";
import { SemAcessoComponent } from "./pages/sem-acesso/sem-acesso.component";
import { authGuard } from "./commons/guards/auth.guard";
import { authInverterGuard } from "./commons/guards/auth-inverter.guard";
import { adminGuard } from "./commons/guards/admin.guard";
import { DoacoesCadastroComponent } from "./pages/doacoes-cadastro/doacoes-cadastro.component";
import { DonationSuccessComponent } from './pages/donation-success/donation-success.component';
import { DonationCancelComponent } from './pages/donation-cancel/donation-cancel.component';
import { HomeComponent } from './pages/home/home.component';
import { PetCadastroComponent } from './pages/pet-cadastro/pet-cadastro.component';
import { AdocaoComponent } from './pages/adocao/adocao.component';
import { DoacoesComponent } from './pages/doacoes/doacoes.component';
import { DoacoesAdminComponent } from './pages/doacoes-admin/doacoes-admin.component';
import { ApadrinhadosComponent } from './pages/apadrinhados/apadrinhados.component';
import { ApadrinhadosCadastroComponent } from './pages/apadrinhados-cadastro/apadrinhados-cadastro.component';
import { ApadrinhadosAdminComponent } from './pages/apadrinhados-admin/apadrinhados-admin.component';
import { PerfilComponent } from './pages/perfil/perfil.component';

export const routes: Routes = [
  // Rota de entrada da aplicação, redireciona para a landing page pública
  { path: '', redirectTo: 'inicio', pathMatch: 'full' },

  // Rotas públicas (Landing Page) - Não exigem login
  {
    path: 'inicio', component: LadingPageComponent, children: [
      { path: '', component: LadingPageHomeComponent },
      { path: 'pets', component: CatalogoAdocaoComponent },
      { path: 'sobre', component: SobreComponent },
      { path: 'contato', component: ContatoComponent },
      { path: 'nossa-missao', component: NossaMissaoComponent }
    ]
  },

  // Rotas do dashboard, protegidas pelo authGuard (exigem login)
  {
    path: 'dashboard', component: AppLayoutComponent, canActivate: [authGuard], children: [
      { path: '', component: HomeComponent },
      { path: 'pets', component: CatalogoAdocaoComponent },
      { path: 'pets-cadastro', component: PetCadastroComponent, canActivate: [adminGuard] },
      { path: 'pets-adocao/:id', component: AdocaoComponent },
      { path: 'doacoes', component: DoacoesComponent },
      { path: 'doacoes-cadastro', component: DoacoesCadastroComponent }, // Rota de doação para utilizadores logados
      { path: 'doacoes-admin', component: DoacoesAdminComponent, canActivate: [adminGuard] },
      { path: 'apadrinhados', component: ApadrinhadosComponent },
      { path: 'apadrinhados-cadastro', component: ApadrinhadosCadastroComponent },
      { path: 'apadrinhados-admin', component: ApadrinhadosAdminComponent, canActivate: [adminGuard] },
      { path: 'perfil', component: PerfilComponent },
      { path: 'configuracao', component: ConfiguracaoComponent }
    ]
  },

  // Rotas de autenticação, protegidas pelo authInverterGuard (só para não logados)
  { path: 'entrar', component: LoginComponent, canActivate: [authInverterGuard] },
  { path: 'registrar', component: CadastroComponent, canActivate: [authInverterGuard] },

  // Rotas de retorno do pagamento (públicas)
  { path: 'donation/success', component: DonationSuccessComponent },
  { path: 'donation/cancel', component: DonationCancelComponent },

  // Rota de fallback para páginas não encontradas
  { path: '**', component: SemAcessoComponent },
];
