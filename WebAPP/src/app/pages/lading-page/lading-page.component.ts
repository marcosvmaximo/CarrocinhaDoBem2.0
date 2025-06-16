import {Component, OnInit} from '@angular/core';
import {Router, RouterLink, RouterOutlet} from "@angular/router";
import {AuthService} from "../auth/services/auth.service";
import {NgClass, NgIf} from "@angular/common";
import {MenubarModule} from "primeng/menubar";
import {MenuItem} from "primeng/api";
import {IUser} from "../auth/services/IUser";
import {ButtonDirective} from "primeng/button";
import {DividerModule} from "primeng/divider";
import {StyleClassModule} from "primeng/styleclass";
import {Ripple} from "primeng/ripple";

@Component({
  selector: 'app-lading-page',
  standalone: true,
  imports: [
    ButtonDirective,
    DividerModule,
    StyleClassModule,
    Ripple,
    RouterOutlet,
    NgIf,
    NgClass,
    RouterLink,
    MenubarModule
  ],
  templateUrl: './lading-page.component.html',
  styleUrl: './lading-page.component.scss'
})
export class LadingPageComponent implements OnInit {
  tieredItems: MenuItem[] = [];
  user: IUser | null = null;

  // CORREÇÃO: O router precisa ser 'public' para ser acessado pelo template HTML.
  constructor(public router: Router, private authService: AuthService) {}

  ngOnInit(): void {
    // 1. Primeiro, executa a ação para carregar os dados do usuário na propriedade 'this.user'
    this.obterUser();

    // 2. Agora, com 'this.user' populado, construa o menu
    // A lógica aqui já está correta, usando a propriedade 'this.user' após ela ser populada.
    this.tieredItems = [
      {
        label: this.user ? `Olá, ${this.user.userName}` : 'Menu Usuário',
        icon: 'pi pi-user',
        items: [
          {
            label: 'Perfil',
            icon: 'pi pi-user-edit',
            command: () => this.router.navigate(['/dashboard/perfil'])
          },
          {
            label: 'Configurações',
            icon: 'pi pi-cog',
            command: () => this.router.navigate(['/dashboard/configuracao'])
          },
          {
            separator: true
          },
          {
            label: 'Sair',
            icon: 'pi pi-sign-out',
            command: () => this.deslogar()
          }
        ]
      },
    ];
  }

  logado(): boolean {
    return this.authService.estaLogado();
  }

  deslogar(): void {
    // CORREÇÃO: Como AuthService não tem o método 'logout',
    // executamos a lógica de limpeza diretamente aqui.
    localStorage.removeItem("user");
    localStorage.removeItem("logado");
    localStorage.removeItem("admin");

    this.user = null; // Limpa o usuário localmente
    this.router.navigate(['/auth/login']).then(() => {
      window.location.reload(); // Recarrega a página para garantir que o estado seja limpo
    });
  }

  // Este método tem a única responsabilidade de popular a propriedade 'this.user'
  private obterUser(): void {
    const userJson = localStorage.getItem("user");
    if (userJson) {
      try {
        this.user = JSON.parse(userJson);
      } catch (e) {
        console.error("Erro ao fazer parse dos dados do usuário do localStorage:", e);
        this.user = null;
      }
    } else {
      this.user = null;
    }
  }
}
