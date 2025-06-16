import { Injectable } from '@angular/core';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor() {}

    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        // Pega o token do localStorage
        const token = localStorage.getItem('token');

        // Se o token existir, clona a requisição e adiciona o cabeçalho de autorização
        if (token) {
            const clonedRequest = request.clone({
                headers: request.headers.set('Authorization', `Bearer ${token}`)
            });
            return next.handle(clonedRequest);
        }

        // Se não houver token, deixa a requisição original continuar
        return next.handle(request);
    }
}
