# CarrocinhaDoBem

## Integrantes

Vinicius da Costa Pereira

Vinicius Viana Gomes

Marcos Vinicius maximo


## About

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 18.0.2.

## Development server

Run ⁠ ng serve ⁠ for a dev server. Navigate to ⁠ http://localhost:4200/ ⁠. The application will automatically reload if you change any of the source files.

## Code scaffolding

Run ⁠ ng generate component component-name ⁠ to generate a new component. You can also use ⁠ ng generate directive|pipe|service|class|guard|interface|enum|module ⁠.

## Build

Run ⁠ ng build ⁠ to build the project. The build artifacts will be stored in the ⁠ dist/ ⁠ directory.

## Running unit tests

Run ⁠ ng test ⁠ to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run ⁠ ng e2e ⁠ to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

## Further help

To get more help on the Angular CLI use ⁠ ng help ⁠ or go check out the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.



# Como instalar

## Instalar NODE.JS e verificar versão
node --version

## Instalar .NET e verificar versão
dotnet --version

## Instalar Angular
npm install -g @angular/cli

## Instalar Node Modules
npm install

## Verficar versão Angular
ng v

## Limpar terminal
clear

## Criar projeto Angular
ng new nome_projeto --standalone false

## Bibliotecas Angular
npm install bootstrap@5.3.3

## Atualiza dependencias do projeto
ng update

## Cria um novo componente/serviço
ng generate component nome-do-componente
ng generate service nome-do-servico

## Cria um novo arquivo TS
touch exemplo.interface.ts

## Iniciar a aplicação angular no browser
ng serve -o

## Criar a API
dotnet new webapi -n nome_API
dotnet new webapi --framework net6.0 -n nome_API

## Bibliotecas API
dotnet dev-certs https --trust
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNet
dotnet add package Microsoft.EntityFrameworkCore.Relational
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.Extensions.Logging.Console
dotnet add package AutoMapper
dotnet add package Serilog
dotnet add package Serilog.AspNetCore
dotnet add package Microsoft.Extensions.Hosting
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
dotnet ef --version

## Criar pastas API
mkdir nome_pasta

## Verificar API
dotnet build

## Criar BD na API
dotnet ef migrations add CreateDatabase
dotnet ef database update

## Iniciar API
dotnet run

## ERRO PC PUC

Criar o arquivo dotnet-tools.json
{
"version": 1,
"isRoot": true,
"tools": {
"dotnet-ef": {
"version": "6.0.0",  ## Substitua pela versão correta
"commands": ["dotnet-ef"]
}
}
}
dotnet tool restore
dotnet ef database update
