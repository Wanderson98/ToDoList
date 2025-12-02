# 🚀 ToDo List API - Clean Architecture & DevOps

![Build Status](https://img.shields.io/github/actions/workflow/status/Wanderson98/ToDoList/ci-pipeline.yml?label=CI%20Build&logo=github)
![.NET](https://img.shields.io/badge/.NET-10%20-512bd4?logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ed?logo=docker)
![License](https://img.shields.io/badge/License-MIT-green)

Este projeto é uma Web API robusta para gerenciamento de tarefas, desenvolvida com foco em **Clean Architecture**, **Boas Práticas de Engenharia de Software** e **Cultura DevOps**.

O objetivo principal não é apenas criar uma lista de tarefas, mas demonstrar a implementação de um ciclo completo de desenvolvimento de software moderno, desde a concepção do domínio até o deploy automatizado.

---

## 🏗️ Arquitetura e Design

O projeto segue os princípios da **Clean Architecture** (Arquitetura Limpa), visando desacoplamento e testabilidade. A solução está dividida em:

* **🧩 ToDo.Domain:** O coração do sistema. Contém as Entidades (`Tarefa`, `Usuario`) e Interfaces de Repositório. Não depende de ninguém.
* **⚙️ ToDo.Services:** Regras de negócio, Validações (`FluentValidation`), DTOs e lógica de Autenticação.
* **💻  ToDo.Infrastructure:** Implementação técnica. Acesso a dados (`EF Core`), Mapeamento com Banco de Dados.
* **🌐 ToDo.WebApi:** A porta de entrada. Controllers, Middlewares de Erro, Configuração de DI e Swagger.

---

## 🛠️ Tech Stack (Tecnologias)

### Core
* **C# / .NET 10** (Compatível com .NET 8/9)
* **Entity Framework Core** (ORM)
* **PostgreSQL** (Banco de Dados Relacional)

### Qualidade & Segurança
* **xUnit & Moq:** Testes Unitários automatizados para camada de Serviço.
* **FluentValidation:** Validação de entrada de dados e regras de negócio.
* **JWT (JSON Web Token):** Autenticação Stateless.
* **BCrypt:** Hashing seguro de senhas.
* **User Secrets:** Proteção de credenciais em ambiente de desenvolvimento.

### DevOps & Observabilidade
* **Docker & Docker Compose:** Containerização da API, Banco e Ferramentas.
* **GitHub Actions:** Pipeline de CI/CD (Build, Test e Push para Docker Hub).
* **Serilog:** Logging estruturado.
* **Seq:** Servidor de centralização e visualização de logs em tempo real.

### Documentação
* **Swagger (Swashbuckle):** Documentação interativa da API com suporte a JWT.

---

## 🚀 Como Rodar o Projeto

### Pré-requisitos
* [Docker](https://www.docker.com/) instalado.
* [.NET SDK](https://dotnet.microsoft.com/) (para rodar o comando de migração).

### Passo a Passo (Via Docker)

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/Wanderson98/ToDoList.git](https://github.com/Wanderson98/ToDoList.git)
    cd ToDoList
    ```

2.  **Configure as variáveis de ambiente:**
    Crie um arquivo `.env` na raiz (baseado nas configurações do `docker-compose.yml`) ou ajuste o compose para seus testes locais.

3.  **Suba a infraestrutura:**
    ```bash
    docker compose up -d --build
    ```
    Isso iniciará: `API` (Porta 8080), `PostgreSQL` (Porta 5432) e `Seq` (Porta 8081).

4.  **⚡ Aplique as Migrations (Criar Banco de Dados):**
    Como o container do PostgreSQL inicia vazio, execute este comando na raiz do projeto para criar as tabelas:
    ```bash
    dotnet ef database update --connection "Host=localhost;Port=5432;Database=appdb;Username=dev;Password=dev123" --project ToDo.Infrastructure --startup-project ToDo.WebApi
    ```
    > **Nota:** A string de conexão acima aponta para `localhost` (sua máquina) conectando na porta `5432` exposta pelo Docker.

5.  **Acesse a Documentação (Swagger):**
    Abra [http://localhost:8080/swagger](http://localhost:8080/swagger).

6.  **Acesse os Logs (Seq):**
    Abra [http://localhost:8081](http://localhost:8081).

---

## 🧪 Rodando os Testes

Para executar a suíte de testes unitários:

```bash
dotnet test
```

🔌 Endpoints Principais

A API é protegida por JWT. É necessário criar um usuário e realizar login para acessar os recursos de tarefas.

Auth

    POST /api/auth/cadastrar: Cria um novo usuário.

    POST /api/auth/login: Retorna o Token JWT.

Tarefas (Requer Token Bearer)

    GET /api/todo: Lista tarefas.

    POST /api/todo: Cria tarefa.

    GET /api/todo/{id}: Detalhes.

    PATCH /api/todo/{id}/concluir: Marca como concluída.

    DELETE /api/todo/{id}: Remove tarefa.

📈 Melhorias Futuras (Roadmap)

    [ ] Implementar Cache Distribuído com Redis.

    [ ] Adicionar Testes de Integração (Testcontainers).

    [ ] Criar Front-end em Angular.

    [ ] Refatorar para CQRS com MediatR.