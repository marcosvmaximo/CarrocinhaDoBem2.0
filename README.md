![Editor _ Mermaid Chart-2025-06-16-210002](https://github.com/user-attachments/assets/08009052-95f7-4c8a-879f-ebb41a7fc2ea)![Editor _ Mermaid Chart-2025-06-16-205939](https://github.com/user-attachments/assets/49f4c1bd-3920-46ad-8359-c60ea82cb5ed)![Editor _ Mermaid Chart-2025-06-16-205906](https://github.com/user-attachments/assets/1c7e8cd9-aab8-4d22-8b50-6f40fc69607a)# 🐾 Carrocinha do Bem 2.0

![Status](https://img.shields.io/badge/status-em%20desenvolvimento-yellow)
![License](https://img.shields.io/badge/license-MIT-blue)
![Tech-Backend](https://img.shields.io/badge/backend-.NET%206-blueviolet)
![Tech-Frontend](https://img.shields.io/badge/frontend-Angular-red)

Uma plataforma completa para conectar instituições de caridade, doadores, padrinhos e adotantes de animais. O objetivo é facilitar a gestão de animais para adoção, captação de recursos e o processo de apadrinhamento, centralizando tudo em um único ecossistema digital.

---

## 📚 Tabela de Conteúdos

* [Sobre o Projeto](#-sobre-o-projeto)
* [Tecnologias Utilizadas](#-tecnologias-utilizadas)
* [Como Começar](#-como-começar)
  * [Pré-requisitos](#pré-requisitos)
  * [Instalação e Execução](#instalação-e-execução)
* [Estrutura do Projeto](#-estrutura-do-projeto)
* [Documentação UML](#-documentação-uml)
  * [Diagrama de Classes](#diagrama-de-classes)
  * [Diagramas de Sequência](#diagramas-de-sequência)
  * [Diagrama de Atividades](#diagrama-de-atividades)
* [Licença](#-licença)

---

## 🌟 Sobre o Projeto

O **Carrocinha do Bem 2.0** é uma aplicação web Full Stack projetada para resolver um problema real: a dificuldade na gestão e financiamento de abrigos de animais.

**Principais Funcionalidades:**
* **Gestão de Animais:** Cadastro e listagem de animais para adoção e apadrinhamento.
* **Catálogo de Adoção:** Interface pública para que visitantes possam encontrar e solicitar a adoção de um pet.
* **Sistema de Doações:** Permite doações financeiras via PIX (com um simulador de pagamentos para testes).
* **Apadrinhamento:** Usuários podem se tornar padrinhos de um animal, contribuindo com um valor mensal.
* **Autenticação e Perfis:** Sistema de cadastro e login para usuários e instituições, com papéis e permissões distintas.
* **Painel Administrativo:** Interfaces para que instituições possam gerenciar doações recebidas, pedidos de adoção e animais apadrinhados.

---

## 🛠️ Tecnologias Utilizadas

O projeto segue uma arquitetura moderna, desacoplada entre front-end e back-end.

### Back-end
* **.NET 6:** Plataforma de desenvolvimento robusta e de alta performance.
* **ASP.NET Core Web API:** Para a construção dos endpoints RESTful.
* **Entity Framework Core 6:** ORM para interação com o banco de dados.
* **PostgreSQL (ou In-Memory para testes):** Banco de dados relacional.
* **API de Pagamentos Falsa (FakePSP.Api):** Um serviço simulado para processar pagamentos PIX, permitindo testes de ponta a ponta do fluxo de doação.

### Front-end
* **Angular:** Framework para a construção da interface de usuário reativa e moderna.
* **TypeScript:** Superset do JavaScript que adiciona tipagem estática.
* **PrimeNG:** Biblioteca de componentes de UI para Angular, garantindo uma interface rica e consistente.
* **SCSS:** Pré-processador de CSS para um código de estilos mais organizado e reaproveitável.

---

## 🚀 Como Começar

Siga estas instruções para obter uma cópia do projeto e executá-la em sua máquina local para desenvolvimento e testes.

### Pré-requisitos

Você precisará ter as seguintes ferramentas instaladas:
* [Git](https://git-scm.com/)
* [.NET SDK 6.0 ou superior](https://dotnet.microsoft.com/download/dotnet/6.0)
* [Node.js e npm](https://nodejs.org/en/)
* [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)

### Instalação e Execução

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/seu-usuario/carrocinha-do-bem-2.0.git](https://github.com/seu-usuario/carrocinha-do-bem-2.0.git)
    cd carrocinha-do-bem-2.0
    ```

2.  **Execute o Back-end (API Principal):**
    ```bash
    cd Api/webApi
    dotnet restore
    dotnet run
    ```
    A API principal estará rodando em `https://localhost:7001` (ou uma porta similar).

3.  **Execute o Back-end (Simulador de Pagamentos):**
    *Abra um novo terminal.*
    ```bash
    cd Api/FakePSP.Api
    dotnet restore
    dotnet run
    ```
    O simulador de pagamentos estará rodando em `https://localhost:7002` (ou uma porta similar).

4.  **Execute o Front-end:**
    *Abra outro novo terminal.*
    ```bash
    cd WebAPP
    npm install
    ng serve
    ```
    Acesse a aplicação no seu navegador em `http://localhost:4200/`.

---

## 📁 Estrutura do Projeto

O código-fonte está organizado em duas pastas principais:

* `📁 Api/`: Contém todo o código do back-end, incluindo a API principal (`webApi`) e o simulador de pagamentos (`FakePSP.Api`).
* `📁 WebAPP/`: Contém todo o código do front-end desenvolvido em Angular.

---

## 📄 Documentação UML

A seguir, a documentação visual da arquitetura e dos principais fluxos do sistema.

### Diagrama de Classes

**Descrição:** A imagem abaixo representa a estrutura estática do sistema, mostrando as principais entidades, seus atributos e como elas se relacionam.

![Diagrama de Classes](https://github.com/user-attachments/assets/b722c521-09a9-46bd-9160-7561d026625d)

### Diagramas de Sequência

**Descrição:** Os diagramas a seguir detalham a sequência de interações entre os componentes do sistema para realizar o fluxo de **Doação via PIX**.

**1. Criação da Cobrança PIX:**
![Criação da Doação](https://github.com/user-attachments/assets/2e9841f5-ce33-481b-a1ce-455bd391a881)


**2. Confirmação do Pagamento (Webhook):**
![Editor _ Mermaid Chart-2025-06-16-210002](https://github.com/user-attachments/assets/573db714-3082-4b84-a7a1-feae92b537d3)

---

## 📝 Licença

Distribuído sob a licença MIT. Veja `LICENSE` para mais informações.
