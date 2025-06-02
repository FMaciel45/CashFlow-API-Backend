# Sobre o projeto
CashFlow é uma API RESTful desenvolvida em .NET 8 com o objetivo de gerenciar
despesas pessoais. A aplicação permite ao usuário se registrar e, ao realizar 
login com e-mail e senha, salvar informações detalhadas sobre suas despesas, como: título, data, 
descrição, valor e tipo depagamento. Os dados são persistidos em um banco de dados MySQL.
Também é possível, para o usuário já logado, atualizar seu e-mail e/ou senha, além de 
listar informações sobre sua conta e deletá-la.

A arquitetura segue os princípios do Domain-Driven Design (DDD), buscando uma
estrutura modular e de fácil manutenção. A API utiliza métodos HTTP padrão e
oferece documentação interativa via Swagger para facilitar a exploração e
teste dos endpoints.

A aplicação segue o padrão de autenticação com Token JWT (JSON Web Token) para maior 
segurança e controle de acesso, garantindo que apenas usuários devidamente autenticados
possam acessar recursos protegidos. O token é gerado no momento do login e permite a 
validação da identidade do usuário de forma eficiente e segura.

## Principais Tecnologias e Conceitos
* **.NET 8:** Plataforma de desenvolvimento.
* **ASP.NET Core:** Framework para construção da API web.
* **Entity Framework Core:** ORM para interação com o banco de dados MySQL.
* **MySQL:** Sistema de gerenciamento de banco de dados relacional.
* **Domain-Driven Design (DDD):** Abordagem arquitetural para organização do
código.
* **Swagger:** Geração de documentação da API e interface de teste.
* **AutoMapper:** Mapeamento entre objetos (ex: Entidades de Domínio e
DTOs).
* **FluentValidation:** Implementação de regras de validação.
* **FluentAssertions:** Biblioteca para escrita de testes mais legíveis.
* **xUnit:** Framework para testes unitários e de integração.
### Estrutura da Aplicação
**`CashFlow.Api`**: Camada de Apresentação/Interface. Responsável por expor os
endpoints da API REST, receber requisições HTTP, encaminhá-las para a camada
de aplicação, tratar respostas e configurar a infraestrutura da API (Swagger,
autenticação, tratamento de erros, etc.). Contém os Controllers.

**`CashFlow.Application`**: Camada de Aplicação. Orquestra os casos de uso da
aplicação. Contém as regras de negócio, coordena as interações entre a camada
de domínio e a camada de infraestrutura. Utiliza DTOs (definidos em
Communication) para receber e retornar dados da API. Implementa os casos de
uso (Use Cases) e pode usar o AutoMapper para mapeamentos.

**`CashFlow.Communication`**: Projeto auxiliar contendo os Data Transfer
Objects (DTOs) - classes usadas para transferir dados entre as camadas,
especialmente entre a API e a Aplicação (Requests e Responses). Também contém
Enums e validadores associados a esses DTOs.

**`CashFlow.Domain`**: Camada de Domínio. Contém as
entidades de negócio (Expense, User, etc.), regras de negócio
intrínsecas ao domínio, interfaces de repositório ("contratos" para acesso a
dados) e Domain Services. É independente das outras camadas.

**`CashFlow.Exception`**: Projeto auxiliar para definir exceções customizadas
da aplicação, permitindo um tratamento de erros mais específico e organizado.

**`CashFlow.Infrastructure`**: Camada de Infraestrutura. Implementa os
detalhes técnicos e preocupações externas. Contém a implementação concreta das
interfaces definidas no Domínio (ex.: Repositórios usando Entity Framework
Core), acesso ao banco de dados (DbContext), serviços externos, logging, etc.

### Fluxo da Aplicação
1. Requisição HTTP → 2. Controller → 3. Validação → 4. Caso de Uso → 5.
Repositório → 6. Banco de Dados → 7. Resposta
### Utilização
1. Clone o projeto
```bash
git clone https://github.com/FMaciel45/CashFlow-API-Backend.git
```
2. Altere os parâmetros em ```appsettings.Development.json```
3. Execute a API pressionando F5
