# 🚀 ToDo List API - Clean Architecture & DevOps

![Build Status](https://img.shields.io/github/actions/workflow/status/Wanderson98/ToDoList/ci-pipeline.yml?label=CI%20Build&logo=github)
![.NET](https://img.shields.io/badge/.NET-10%20-512bd4?logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ed?logo=docker)
![Redis](https://img.shields.io/badge/Redis-Cache-red?logo=redis)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-DB-336791?logo=postgresql)

sta é uma Web API de referência desenvolvida para demonstrar a aplicação de **Clean Architecture**, **DevOps Moderno** e **Alta Performance** em ambientes .NET.

O projeto vai além de um simples CRUD, implementando padrões de mercado para segurança, observabilidade e escalabilidade.

---

## 🌟 Diferenciais e Funcionalidades

### 🏗️ Arquitetura & Design
* **Clean Architecture:** Separação estrita de responsabilidades (Domain, Services, Infra, WebApi).
* **Domain-Driven Design (DDD):** Entidades ricas e validações de domínio.
* **Pattern Cache-Aside:** Uso inteligente de **Redis** para reduzir carga no banco e acelerar leituras.

### 🔒 Segurança
* **Autenticação JWT:** Tokens seguros com Claims.
* **Criptografia:** Senhas armazenadas com hash **BCrypt**.
* **Gestão de Perfil:** Endpoints seguros para o usuário consultar e atualizar seus dados.
* **Proteção de Segredos:** Uso de User Secrets em desenvolvimento e Variáveis de Ambiente em produção.

### 📊 Observabilidade
* **Logging Estruturado:** Implementação do **Serilog**.
* **Dashboard em Tempo Real:** Monitoramento de erros e performance via **Seq**.

### 🐳 DevOps
* **Docker Full Stack:** API, Banco (Postgres), Cache (Redis) e Logs (Seq) orquestrados via Docker Compose.
* **CI/CD:** Pipeline automatizado no GitHub Actions para testes e publicação no Docker Hub.

---

## 🛠️ Tech Stack

* **Core:** .NET 10  / C#
* **Banco de Dados:** PostgreSQL + Entity Framework Core
* **Cache:** Redis (StackExchange.Redis)
* **Validação:** FluentValidation
* **Testes:** xUnit + Moq
* **Documentação:** Swagger (Swashbuckle) com suporte a XML Comments

---

## 🚀 Como Rodar o Projeto

### Pré-requisitos
* [Docker Desktop](https://www.docker.com/) instalado.
* [.NET SDK](https://dotnet.microsoft.com/) (para rodar migrations).

### Passo a Passo

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/Wanderson98/ToDoList.git](https://github.com/Wanderson98/ToDoList.git)
    cd ToDoList-CleanArch
    ```

2.  **Suba a infraestrutura:**
    Este comando sobe a API, Postgres, Redis e Seq.
    ```bash
    docker compose up -d --build
    ```

3.  **⚡ Inicialize o Banco de Dados:**
    Execute a migration para criar as tabelas no container do Postgres:
    ```bash
    dotnet ef database update --connection "Host=localhost;Port=5432;Database=appdb;Username=dev;Password=dev123" --project ToDo.Infrastructure --startup-project ToDo.WebApi
    ```

4.  **Acesse os Serviços:**
    * 📄 **Swagger (Doc):** [http://localhost:8080/swagger](http://localhost:8080/swagger)
    * 📊 **Seq (Logs):** [http://localhost:8081](http://localhost:8081)

---

## 🔌 Endpoints Principais

A API possui política de **CORS** configurada, pronta para integração com Front-ends (Angular/React).

### 🔐 Autenticação (Público)
* `POST /api/auth/cadastrar`: Registra um novo usuário.
* `POST /api/auth/login`: Retorna o Token JWT.

### 👤 Usuário (Requer Token)
* `GET /api/usuarios/perfil`: Retorna os dados do usuário logado.
* `PUT /api/usuarios`: Atualiza nome, email ou senha (exige senha atual).

### ✅ Tarefas (Requer Token - Com Cache Redis)
* `GET /api/todo`: Lista tarefas (Cacheado por 5 min).
* `POST /api/todo`: Cria tarefa (Invalida cache).
* `PATCH /api/todo/{id}/concluir`: Marca como concluída.
* `DELETE /api/todo/{id}`: Remove tarefa.

---

## 🧪 Testes

O projeto possui cobertura de testes unitários na camada de serviço, validando regras de negócio e mocks de repositório.

```bash
dotnet test
```

📈 Melhorias Futuras (Roadmap)

    [x] Implementar Cache Distribuído com Redis.

    [ ] Adicionar Testes de Integração (Testcontainers).

    [ ] Criar Front-end em Angular.

    [ ] Refatorar para CQRS com MediatR.
