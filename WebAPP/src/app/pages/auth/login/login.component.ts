import {Component} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule, Validators
} from '@angular/forms';
import {Router, RouterLink} from "@angular/router";
import {ButtonDirective} from "primeng/button";
import {CheckboxModule} from "primeng/checkbox";
import {InputTextModule} from "primeng/inputtext";
import {PasswordModule} from "primeng/password";
import {Ripple} from "primeng/ripple";
import {MessageModule} from "primeng/message";
import {NgClass, NgIf} from "@angular/common";
import {AuthService} from "../services/auth.service";
import {MessageService} from "primeng/api";
import {ToastModule} from "primeng/toast";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    ButtonDirective,
    CheckboxModule,
    InputTextModule,
    PasswordModule,
    FormsModule,
    Ripple,
    MessageModule,
    NgClass,
    ToastModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  // <<< CORREÇÃO AQUI >>>
  // Adiciona o MessageService à lista de providers do componente.
  providers: [MessageService]
})
export class LoginComponent{
  senha!: string;
  form: FormGroup;

  private fieldNames: any = {
    email: "E-mail",
    senha: "Senha"
  };

  // O seu construtor e o resto da classe permanecem os mesmos
  constructor(private fb: FormBuilder, private service: AuthService, private msgService: MessageService, private router: Router) {
    this.form = fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required]]
    });
  }

  get f() { return this.form.controls; }

  entrar() {
    if (this.form.invalid) {
      this.showErrors();
      return;
    }

    const email = this.f['email'].value;
    const senha = this.f['senha'].value;

    // Este método foi corrigido anteriormente para salvar o token e o usuário
    this.service.login({email: email, password: senha})
        .subscribe({
          next: (response) => {
            // <<< CORREÇÃO PRINCIPAL AQUI >>>
            // Verifica apenas a existência do token na resposta.
            if (response && response.token) {
              this.msgService.add({ key: 'tst', severity: 'success', summary: 'Sucesso', detail: 'Login realizado com sucesso!' });

              // 1. Salva o token JWT no localStorage
              localStorage.setItem("token", response.token);

              // 2. Descodifica o token para extrair as informações do usuário
              try {
                const payload = JSON.parse(atob(response.token.split('.')[1]));
                const user = {
                  id: payload.nameid,
                  userName: payload.unique_name,
                  email: payload.email,
                  userType: payload.role
                };

                // 3. Salva os dados do usuário e o status de login/admin
                localStorage.setItem("user", JSON.stringify(user));
                localStorage.setItem("logado", "true");
                localStorage.setItem("admin", user.userType === 'Admin' ? 'true' : 'false');

              } catch(e) {
                console.error("Erro ao descodificar o token JWT:", e);
                this.msgService.add({ key: 'tst', severity: 'error', summary: 'Erro', detail: 'Token de autenticação inválido.' });
                return;
              }

              // 4. Redireciona para a página inicial
              setTimeout(() => {
                this.router.navigate(['/inicio']);
              }, 1000);

            } else {
              this.msgService.add({ key: 'tst', severity: 'error', summary: 'Erro', detail: 'Resposta de login inválida do servidor.' });
            }
          },
          error: (err) => {
            console.error('Erro no login:', err);
            this.msgService.add({ key: 'tst', severity: 'error', summary: 'Erro', detail: 'Email ou senha inválidos.' });
          }
        });
  }

  // Seus métodos de validação permanecem os mesmos
  showErrors(){
    Object.keys(this.form.controls).forEach(key => {
      const controlErrors = this.form.get(key)?.errors;
      if (controlErrors != null) {
        Object.keys(controlErrors).forEach(keyError => {
          switch (keyError) {
            case 'required':
              this.showErrorViaToast(`O campo ${this.fieldNames[key]} é obrigatório.`);
              break;
            case 'email':
              this.showErrorViaToast(`O campo ${this.fieldNames[key]} não é um e-mail válido.`);
              break;
            default:
              this.showErrorViaToast(`Erro no campo ${this.fieldNames[key]}.`);
              break;
          }
        });
      }
    });
  }

  showErrorViaToast(message: string) {
    this.msgService.add({ key: 'tst', severity: 'error', summary: 'Erro de Validação', detail: message });
  }
}
