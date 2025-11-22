# API de Agendamento de Consultas Médicas - Backend

Uma API RESTful completa desenvolvida em **.NET 8.0** para gerenciamento de agendamentos médicos, construída com **Entity Framework Core** e **PostgreSQL**. Esta API fornece endpoints seguros para autenticação, gerenciamento de usuários, médicos, clientes, consultas, horários, planos de saúde e sistema de lista de espera.

## Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Stack Tecnológico](#stack-tecnológico)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Modelo de Dados](#modelo-de-dados)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Configuração](#instalação-e-configuração)
- [Configuração do Banco de Dados](#configuração-do-banco-de-dados)
- [Autenticação e Autorização](#autenticação-e-autorização)
- [Integração com Frontend](#integração-com-frontend)
- [Endpoints da API](#endpoints-da-api)
- [Padrões de Desenvolvimento](#padrões-de-desenvolvimento)
- [Migrations](#migrations)
- [Desenvolvimento](#desenvolvimento)
- [Deploy](#deploy)
- [Troubleshooting](#troubleshooting)

## Visão Geral

Esta API serve como backend para um sistema completo de agendamento de consultas médicas, oferecendo:

### Status de Implementação

A maioria das funcionalidades está implementada tanto no backend quanto no frontend. No entanto, as seguintes funcionalidades estão disponíveis **apenas no backend** (APIs prontas, mas sem interface no frontend):

- **Notifications** (`/Notifications`): Sistema de notificações
- **Reviews** (`/Reviews`): Sistema de avaliações de consultas
- **Anamnese** (`/Anamnese`): Histórico médico dos pacientes

Essas APIs estão funcionais e podem ser testadas via Swagger ou Postman, mas não possuem interface gráfica no frontend ainda.

- **Autenticação JWT via Supabase**: Integração com Supabase para autenticação e gerenciamento de usuários
- **Gerenciamento Multi-papel**: Suporte para Clientes, Médicos e Administradores
- **CRUD Completo**: Operações completas de criação, leitura, atualização e exclusão para todas as entidades
- **Sistema de Lista de Espera**: Gerenciamento inteligente de filas de espera para consultas
- **Soft Delete**: Exclusão lógica com campos `deleted_at` e `canceled_at`
- **Validação de Dados**: Constraints de banco de dados e validação em múltiplas camadas
- **Conversão de Enums**: Mapeamento automático de enums C# para strings no banco de dados

### Principais Entidades

- **Users**: Contas de usuário do sistema
- **Clients**: Perfis de pacientes/clientes
- **Doctors**: Perfis de médicos com especialidades
- **Clinics**: Clínicas médicas
- **ClinicUsers**: Associações usuário-clínica (médicos, assistentes, admins)
- **Appointments**: Consultas agendadas
- **Schedules**: Horários de disponibilidade dos médicos
- **HealthPlans**: Planos de saúde
- **DoctorHealthPlans**: Associações médico-plano de saúde
- **ClientHealthPlans**: Planos de saúde dos clientes
- **Waitlist**: Lista de espera para consultas
- **Notifications**: Notificações do sistema ⚠️ _(Apenas backend - não implementado no frontend)_
- **Reviews**: Avaliações de consultas ⚠️ _(Apenas backend - não implementado no frontend)_
- **Anamnese**: Histórico médico dos pacientes ⚠️ _(Apenas backend - não implementado no frontend)_

## Arquitetura

### Padrão Arquitetural

A API segue o padrão **Repository Pattern** com separação clara de responsabilidades:

```
┌─────────────────────────────────────────────────────────┐
│                    Controllers                          │
│         (Endpoints HTTP, Validação de Request)          │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                   Repositories                          │
│    (Lógica de Negócio, Acesso a Dados, Validações)      │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                  AppDbContext                           │
│         (Entity Framework Core, PostgreSQL)             │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│               PostgreSQL Database                       │
│             (Supabase Cloud ou Local)                   │
└─────────────────────────────────────────────────────────┘
```

### Camadas da Aplicação

1. **Controllers**: Recebem requisições HTTP, validam entrada, chamam repositórios e retornam respostas
2. **Repositories**: Contêm lógica de negócio, acesso a dados via Entity Framework Core e validações
3. **Models**: Entidades de domínio e DTOs (Data Transfer Objects)
4. **DatabaseContext**: Configuração do Entity Framework Core, mapeamentos e conversões
5. **Services**: Serviços auxiliares (autenticação Supabase, upload de fotos, email)
6. **Middleware**: Middleware customizado para validação JWT do Supabase

### Fluxo de Requisição

```
HTTP Request
    │
    ▼
[Middleware: SupabaseJwtMiddleware] → Valida JWT token
    │
    ▼
[Authentication] → Verifica autenticação
    │
    ▼
[Authorization] → Verifica permissões ([Authorize])
    │
    ▼
[Controller] → Valida ModelState, extrai dados
    │
    ▼
[Repository] → Executa lógica de negócio
    │
    ▼
[AppDbContext] → Executa queries SQL via EF Core
    │
    ▼
[PostgreSQL/Supabase] → Retorna dados
    │
    ▼
HTTP Response
```

## Stack Tecnológico

### Framework e Runtime

- **.NET 8.0**: Framework moderno e performático
- **C# 12**: Linguagem de programação com recursos avançados
- **ASP.NET Core**: Framework web para construção de APIs RESTful

### Banco de Dados

- **PostgreSQL**: Banco de dados relacional (via Supabase)
- **Entity Framework Core 9.0.9**: ORM para acesso a dados
- **Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4**: Provider EF Core para PostgreSQL

### Autenticação e Segurança

- **Supabase 1.1.1**: Plataforma de autenticação e banco de dados
- **JWT (JSON Web Tokens)**: Tokens de autenticação
- **Microsoft.AspNetCore.Authentication.JwtBearer 8.0.14**: Suporte a autenticação JWT

### Documentação e Ferramentas

- **Swashbuckle.AspNetCore 8.0.0**: Geração automática de documentação Swagger/OpenAPI
- **Microsoft.EntityFrameworkCore.Tools 9.0.9**: Ferramentas para migrations

### Outras Dependências

- **Microsoft.IdentityModel.Tokens 8.7.0**: Validação e manipulação de tokens JWT
- **Microsoft.AspNetCore.SignalR 1.2.0**: Suporte a comunicação em tempo real (preparado para futuras funcionalidades)

## Estrutura do Projeto

```
medical-appointment-scheduling-api/
├── medical-appointment-scheduling-api/
│   ├── Controllers/                    # Controladores HTTP
│   │   ├── AnamneseController.cs
│   │   ├── AppointmentsController.cs
│   │   ├── AuthController.cs
│   │   ├── ClientHealthPlansController.cs
│   │   ├── ClientsController.cs
│   │   ├── ClinicsController.cs
│   │   ├── ClinicUsersController.cs
│   │   ├── DoctorHealthPlansController.cs
│   │   ├── DoctorsController.cs
│   │   ├── HealthPlansController.cs
│   │   ├── NotificationsController.cs
│   │   ├── ReviewsController.cs
│   │   ├── SchedulesController.cs
│   │   ├── UsersController.cs
│   │   └── WaitlistController.cs
│   │
│   ├── DatabaseContext/                # Contexto do Entity Framework
│   │   └── AppDbContext.cs            # Configuração de entidades e conversões
│   │
│   ├── Migrations/                     # Migrations do banco de dados
│   │   ├── 20251014034349_InitialCreate.cs
│   │   ├── 20251116201624_UpdateWaitlistToUseDoctorInsteadOfAppointment.cs
│   │   └── ...
│   │
│   ├── Middleware/                     # Middleware customizado
│   │   ├── SupabaseJwtMiddleware.cs    # Validação de JWT do Supabase
│   │   └── SupabaseAuthHandler.cs     # Handler de autenticação
│   │
│   ├── Models/                         # Modelos de dados
│   │   ├── Anamnese.cs
│   │   ├── Appointments.cs
│   │   ├── Clients.cs
│   │   ├── Clinics.cs
│   │   ├── ClinicUsers.cs
│   │   ├── Doctors.cs
│   │   ├── HealthPlans.cs
│   │   ├── Notifications.cs
│   │   ├── Reviews.cs
│   │   ├── Schedules.cs
│   │   ├── SystemEnums.cs             # Enums do sistema
│   │   ├── Users.cs
│   │   ├── Waitlist.cs
│   │   └── DTO/                       # Data Transfer Objects
│   │       ├── CurrentUserDto.cs
│   │       ├── FiltroMedicos.cs
│   │       ├── LoginRequest.cs
│   │       ├── RegisterUserDto.cs
│   │       └── ...
│   │
│   ├── Repository/                     # Repositórios (lógica de negócio)
│   │   ├── AnamneseRepository.cs
│   │   ├── AppointmentsRepository.cs
│   │   ├── ClientsRepository.cs
│   │   ├── ClinicsRepository.cs
│   │   ├── ClinicUsersRepository.cs
│   │   ├── DoctorsRepository.cs
│   │   ├── EmailRepository.cs
│   │   ├── HealthPlansRepository.cs
│   │   ├── NotificationsRepository.cs
│   │   ├── ReviewsRepository.cs
│   │   ├── SchedulesRepository.cs
│   │   ├── UsersRepository.cs
│   │   ├── WaitlistRepository.cs
│   │   └── IRepository/               # Interfaces dos repositórios
│   │       ├── IAnamneseRepository.cs
│   │       ├── IAppointmentsRepository.cs
│   │       └── ...
│   │
│   ├── Services/                       # Serviços auxiliares
│   │   ├── SupabaseTokenService.cs    # Integração com Supabase Auth
│   │   └── ProfilePhotoService.cs     # Gerenciamento de fotos de perfil
│   │
│   ├── Config/                         # Configurações adicionais
│   │
│   ├── Program.cs                      # Ponto de entrada e configuração
│   ├── MigrationRunner.cs              # Executor de migrations
│   ├── appsettings.json               # Configurações da aplicação
│   ├── appsettings.Development.json   # Configurações de desenvolvimento
│   └── medical-appointment-scheduling-api.csproj
│
├── database-example.sql                # Script SQL de exemplo
└── README.md                           # Este arquivo
```

## Modelo de Dados

### Diagrama de Entidades Principais

```
Users (1) ──┬── (1) Clients
            │
            ├── (1) Doctors
            │
            └── (N) ClinicUsers ── (N) Clinics

Doctors (N) ── (N) DoctorHealthPlans ── (N) HealthPlans
Doctors (1) ── (N) Schedules
Doctors (1) ── (N) Appointments
Doctors (1) ── (N) Waitlist

Clients (1) ── (N) Appointments
Clients (1) ── (N) ClientHealthPlans ── (N) HealthPlans
Clients (1) ── (N) Waitlist
Clients (1) ── (1) Anamnese

Appointments (1) ── (1) Reviews
```

### Principais Relacionamentos

- **Users → Clients/Doctors**: Um usuário pode ser um cliente OU um médico (1:1)
- **Users → ClinicUsers**: Um usuário pode estar associado a múltiplas clínicas (1:N)
- **Doctors → Schedules**: Um médico tem múltiplos horários de disponibilidade (1:N)
- **Doctors → Appointments**: Um médico pode ter múltiplas consultas (1:N)
- **Clients → Appointments**: Um cliente pode ter múltiplas consultas (1:N)
- **Doctors ↔ HealthPlans**: Relacionamento muitos-para-muitos via `DoctorHealthPlans`
- **Clients ↔ HealthPlans**: Relacionamento muitos-para-muitos via `ClientHealthPlans`
- **Waitlist**: Relaciona `ClientId` e `DoctorId` (e opcionalmente `ClinicId`)

### Enums do Sistema

Todos os enums estão definidos em `SystemEnums.cs` e são convertidos automaticamente para strings no banco de dados:

- **Weekday**: Dias da semana (monday, tuesday, ..., sunday)
- **AppointmentType**: Tipo de consulta (in_person, online)
- **AppointmentStatus**: Status da consulta (scheduled, completed, canceled, no_show)
- **WaitlistStatus**: Status da lista de espera (pending, confirmed, canceled)
- **Speciality**: Especialidades médicas (Cardiology, Dermatology, etc.)
- **ETipoClinicUser**: Papel na clínica (doctor, assistant, admin)
- **HealthPlans**: Planos de saúde (SUS, Unimed, Bradesco, etc.)
- **NotificationChannel**: Canal de notificação (email, whatsapp)
- **NotificationCategory**: Categoria de notificação (reminder, system, alert)

## Pré-requisitos

Antes de instalar e executar este projeto, certifique-se de ter:

- **.NET 8.0 SDK** ou superior instalado
- **PostgreSQL** (via Supabase Cloud ou instalação local)
- **Visual Studio 2022**, **Visual Studio Code** ou **Rider** (IDE recomendada)
- **Git** para controle de versão
- **Conta Supabase** (para autenticação e banco de dados)

### Verificando Instalação

```bash
# Verificar versão do .NET
dotnet --version
# Deve retornar 8.0.x ou superior

# Verificar se PostgreSQL está acessível (se usando local)
psql --version
```

## Instalação e Configuração

### 1. Clonar o Repositório

```bash
git clone <repository-url>
cd medical-appointment-scheduling-api
```

### 2. Configurar appsettings.json

Edite o arquivo `medical-appointment-scheduling-api/appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "User Id=seu-usuario;Password=sua-senha;Server=seu-servidor;Port=5432;Database=seu-database"
  },
  "Supabase": {
    "Url": "https://seu-projeto.supabase.co",
    "AnonKey": "sua-anon-key",
    "ServiceRoleKey": "sua-service-role-key",
    "JwtSecret": "seu-jwt-secret"
  },
  "AllowedHosts": "*"
}
```

**Importante**: Nunca commite credenciais reais no repositório. Use `appsettings.Development.json` para desenvolvimento local e variáveis de ambiente para produção.

### 3. Restaurar Dependências

```bash
cd medical-appointment-scheduling-api
dotnet restore
```

### 4. Aplicar Migrations

```bash
# Aplicar todas as migrations pendentes
dotnet ef database update

# Ou usar o MigrationRunner
dotnet run --project medical-appointment-scheduling-api
```

### 5. Executar a Aplicação

```bash
dotnet run --project medical-appointment-scheduling-api
```

A API estará disponível em:
- **HTTP**: `http://localhost:5298`
- **HTTPS**: `https://localhost:7213`
- **Swagger UI**: `http://localhost:5298/swagger` ou `https://localhost:7213/swagger`

## Configuração do Banco de Dados

### Usando Supabase (Recomendado)

1. Crie um projeto no [Supabase](https://supabase.com)
2. Obtenha a connection string do PostgreSQL no painel do Supabase
3. Configure as chaves de API (AnonKey, ServiceRoleKey, JwtSecret)
4. Cole a connection string em `appsettings.json`

### Usando PostgreSQL Local

1. Instale PostgreSQL localmente
2. Crie um banco de dados:
```sql
CREATE DATABASE medical_appointment_scheduling;
```
3. Configure a connection string:
```
Host=localhost;Port=5432;Database=medical_appointment_scheduling;Username=postgres;Password=sua-senha
```

### Executando Migrations

```bash
# Listar migrations pendentes
dotnet ef migrations list

# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Reverter última migration
dotnet ef database update NomeDaMigrationAnterior
```

## Autenticação e Autorização

### Fluxo de Autenticação

A API utiliza **Supabase** para autenticação, seguindo este fluxo:

1. **Registro/Login**: Cliente faz requisição para `/Auth/Register` ou `/Auth/DirectLogin`
2. **Supabase Auth**: Supabase valida credenciais e retorna JWT token
3. **Token JWT**: Cliente armazena token e inclui no header `Authorization: Bearer <token>`
4. **Middleware**: `SupabaseJwtMiddleware` valida token em cada requisição
5. **Autorização**: `[Authorize]` verifica se usuário está autenticado

### Middleware de Autenticação

O `SupabaseJwtMiddleware` é executado antes de cada requisição:

```csharp
// Extrai token do header Authorization
// Valida expiração do token
// Cria ClaimsPrincipal com dados do token
// Anexa ao HttpContext.User
```

**Nota**: O middleware confia nos tokens do Supabase sem validar assinatura, pois o Supabase já validou o token ao emiti-lo.

### Protegendo Endpoints

```csharp
[HttpGet("ProtectedEndpoint")]
[Authorize]  // Requer autenticação
public async Task<IActionResult> ProtectedEndpoint()
{
    // Acessa usuário autenticado via User.Claims
    var email = User.FindFirstValue("email");
    // ...
}
```

### Obtendo Dados do Usuário Autenticado

```csharp
// No Controller
var email = User.FindFirstValue("email");
var userId = int.Parse(User.FindFirstValue("sub") ?? "0");

// Buscar usuário completo no banco
var user = await _usersRepository.GetByEmailAsync(email);
```

## Integração com Frontend

Esta API é consumida por uma aplicação frontend desenvolvida em **Angular 17**, fornecendo interfaces para Clientes, Médicos e Administradores.

### Configuração de CORS

Para permitir comunicação com o frontend Angular, configure CORS no `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// No pipeline, antes de app.UseAuthentication()
app.UseCors("AllowAngularApp");
```

### URLs

- **Backend**: `http://localhost:5298` (Swagger: `/swagger`)
- **Frontend**: `http://localhost:4200`

### Configuração do Frontend

No `src/environment.ts` do frontend:

```typescript
export const environment = {
    apiUrl: 'http://localhost:5298',
    production: false
}
```

### Autenticação

O frontend autentica via `POST /Auth/DirectLogin`, recebe JWT token e inclui no header `Authorization: Bearer <token>` em requisições subsequentes.

### Status de Implementação

✅ **Implementado no Frontend**: Autenticação, Usuários, Clientes, Médicos, Consultas, Horários, Lista de Espera, Planos de Saúde, Clínicas

⚠️ **Apenas Backend**: Notifications, Reviews, Anamnese (APIs prontas, sem interface)

## Endpoints da API

### Autenticação (`/Auth`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| POST | `/Auth/DirectLogin` | Login com email e senha | Não |
| POST | `/Auth/Register` | Registro de novo usuário | Não |
| GET | `/Auth/CurrentUser` | Obter usuário autenticado | Sim |

### Usuários (`/Users`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/Users` | Listar todos os usuários | Sim |
| GET | `/Users/{id}` | Obter usuário por ID | Sim |
| POST | `/Users/Register` | Registrar novo usuário | Não |
| PUT | `/Users/{id}` | Atualizar usuário | Sim |
| DELETE | `/Users/{id}` | Excluir usuário (soft delete) | Sim |

### Clientes (`/Clients`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/Clients` | Listar todos os clientes | Sim |
| GET | `/Clients/{id}` | Obter cliente por ID | Sim |
| GET | `/Clients/GetByUserId/{userId}` | Obter cliente por ID de usuário | Sim |
| POST | `/Clients/Create` | Criar novo cliente | Sim |
| PUT | `/Clients/{id}` | Atualizar cliente | Sim |
| DELETE | `/Clients/{id}` | Excluir cliente | Sim |

### Médicos (`/Doctors`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/Doctors` | Listar todos os médicos | Sim |
| GET | `/Doctors/GetById/{id}` | Obter médico por ID | Sim |
| GET | `/Doctors/GetByUserId/{userId}` | Obter médico por ID de usuário | Sim |
| POST | `/Doctors/GetDoctorsByFilter` | Filtrar médicos por critérios | Sim |
| POST | `/Doctors/Create` | Criar novo médico | Sim |
| PUT | `/Doctors/{id}` | Atualizar médico | Sim |
| DELETE | `/Doctors/{id}` | Excluir médico | Sim |

### Consultas (`/Appointments`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/Appointments/{id}` | Obter consulta por ID | Sim |
| GET | `/Appointments/GetByClientId/{clientId}` | Obter consultas do cliente | Sim |
| GET | `/Appointments/GetByDoctorId/{doctorId}` | Obter consultas do médico | Sim |
| POST | `/Appointments/Create` | Criar nova consulta | Sim |
| PUT | `/Appointments/{id}` | Atualizar consulta | Sim |
| DELETE | `/Appointments/{id}` | Cancelar consulta (soft delete) | Sim |

### Horários (`/Schedules`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/Schedules/GetById/{id}` | Obter horário por ID | Sim |
| GET | `/Schedules/GetByDoctorId/{doctorId}` | Obter horários do médico | Sim |
| POST | `/Schedules/Create` | Criar novo horário | Sim |
| PUT | `/Schedules/{id}` | Atualizar horário | Sim |
| DELETE | `/Schedules/{id}` | Excluir horário | Sim |

### Lista de Espera (`/Waitlist`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/Waitlist/GetByDoctorId/{doctorId}` | Obter lista de espera do médico | Sim |
| GET | `/Waitlist/GetByClientId/{clientId}` | Obter lista de espera do cliente | Sim |
| POST | `/Waitlist/JoinWaitlist` | Entrar na lista de espera | Sim |
| PUT | `/Waitlist/{id}` | Atualizar entrada da lista | Sim |
| DELETE | `/Waitlist/{id}` | Remover da lista de espera | Sim |

### Clínicas (`/Clinics`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/Clinics` | Listar todas as clínicas | Sim |
| GET | `/Clinics/{id}` | Obter clínica por ID | Sim |
| POST | `/Clinics/Create` | Criar nova clínica | Sim |
| PUT | `/Clinics/{id}` | Atualizar clínica | Sim |
| DELETE | `/Clinics/{id}` | Excluir clínica (soft delete) | Sim |

### Associações Clínica-Usuário (`/ClinicUsers`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/ClinicUsers/GetById/{id}` | Obter associação por ID | Sim |
| GET | `/ClinicUsers/GetByUserId/{userId}` | Obter associações do usuário | Sim |
| GET | `/ClinicUsers/GetByClinicId/{clinicId}` | Obter associações da clínica | Sim |
| POST | `/ClinicUsers/Create` | Criar nova associação | Sim |
| PUT | `/ClinicUsers/{id}` | Atualizar associação | Sim |
| DELETE | `/ClinicUsers/{id}` | Remover associação | Sim |

### Planos de Saúde (`/HealthPlans`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/HealthPlans` | Listar todos os planos | Sim |
| GET | `/HealthPlans/{id}` | Obter plano por ID | Sim |
| POST | `/HealthPlans/Create` | Criar novo plano | Sim |
| PUT | `/HealthPlans/{id}` | Atualizar plano | Sim |
| DELETE | `/HealthPlans/{id}` | Excluir plano | Sim |

### Associações Médico-Plano (`/DoctorHealthPlans`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/DoctorHealthPlans` | Listar todas as associações | Sim |
| POST | `/DoctorHealthPlans/Create` | Associar médico com plano | Sim |
| DELETE | `/DoctorHealthPlans/{id}` | Remover associação | Sim |

### Notificações (`/Notifications`) ⚠️

> **Nota**: Esta funcionalidade está implementada apenas no backend. Não há interface no frontend ainda.

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/Notifications` | Listar notificações | Sim |
| GET | `/Notifications/{id}` | Obter notificação por ID | Sim |
| POST | `/Notifications/Create` | Criar notificação | Sim |
| PUT | `/Notifications/{id}` | Atualizar notificação | Sim |
| DELETE | `/Notifications/{id}` | Excluir notificação | Sim |

### Avaliações (`/Reviews`) ⚠️

> **Nota**: Esta funcionalidade está implementada apenas no backend. Não há interface no frontend ainda.

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/Reviews/{id}` | Obter avaliação por ID | Sim |
| POST | `/Reviews/Create` | Criar nova avaliação | Sim |
| PUT | `/Reviews/{id}` | Atualizar avaliação | Sim |
| DELETE | `/Reviews/{id}` | Excluir avaliação | Sim |

### Anamnese (`/Anamnese`) ⚠️

> **Nota**: Esta funcionalidade está implementada apenas no backend. Não há interface no frontend ainda.

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/Anamnese/{id}` | Obter anamnese por ID | Sim |
| POST | `/Anamnese/Create` | Criar nova anamnese | Sim |
| PUT | `/Anamnese/{id}` | Atualizar anamnese | Sim |
| DELETE | `/Anamnese/{id}` | Excluir anamnese | Sim |

## Padrões de Desenvolvimento

### Repository Pattern

Todos os repositórios seguem o padrão de interface:

```csharp
public interface IEntityRepository
{
    Task<IEnumerable<Entity>> GetAllAsync();
    Task<Entity> GetByIdAsync(int id);
    Task<bool> CreateAsync(Entity entity);
    Task<bool> UpdateAsync(Entity entity);
    Task<bool> DeleteAsync(int id);
}
```

### Nomenclatura

- **Controllers**: `[Entity]Controller.cs` (ex: `UsersController.cs`)
- **Repositories**: `[Entity]Repository.cs` (ex: `UsersRepository.cs`)
- **Interfaces**: `I[Entity]Repository.cs` (ex: `IUsersRepository.cs`)
- **Models**: `[Entity].cs` (ex: `Users.cs`)
- **DTOs**: `[Purpose]Dto.cs` (ex: `RegisterUserDto.cs`)

### Tratamento de Erros

```csharp
try
{
    // Lógica de negócio
    return Ok(result);
}
catch (NotFoundException ex)
{
    return NotFound(new { error = "Not found", message = ex.Message });
}
catch (Exception ex)
{
    _logger.LogError(ex, "Error in {Method}", nameof(Method));
    return StatusCode(500, new { error = "Server error", message = ex.Message });
}
```

### Soft Delete

Entidades que suportam soft delete usam campos `deleted_at` ou `canceled_at`:

```csharp
// No Repository
public async Task<bool> DeleteAsync(int id)
{
    var entity = await _db.Entities.FindAsync(id);
    if (entity == null) return false;
    
    entity.DeletedAt = DateTimeOffset.UtcNow;
    await _db.SaveChangesAsync();
    return true;
}

// Nas queries, filtrar entidades deletadas
return await _db.Entities
    .Where(e => e.DeletedAt == null)
    .ToListAsync();
```

### Conversão de Enums

Enums são convertidos automaticamente para strings no banco de dados via `HasConversion` no `AppDbContext`:

```csharp
// Exemplo: Weekday enum → "monday", "tuesday", etc.
modelBuilder.Entity<Schedules>()
    .Property(s => s.Weekday)
    .HasConversion(
        v => v.ToString().ToLowerInvariant(),  // C# → DB
        v => (SystemEnums.Weekday)Enum.Parse(typeof(SystemEnums.Weekday), v, true)  // DB → C#
    );
```

## Migrations

### Criando uma Nova Migration

```bash
# Criar migration com nome descritivo
dotnet ef migrations add NomeDescritivoDaAlteracao

# Exemplo:
dotnet ef migrations add AddEmailToUsers
```

### Aplicando Migrations

```bash
# Aplicar todas as migrations pendentes
dotnet ef database update

# Aplicar até uma migration específica
dotnet ef database update NomeDaMigration
```

### Revertendo Migrations

```bash
# Reverter última migration
dotnet ef database update NomeDaMigrationAnterior

# Remover última migration (sem aplicar no banco)
dotnet ef migrations remove
```

### Estrutura de uma Migration

```csharp
public partial class NomeDaMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Alterações a serem aplicadas
        migrationBuilder.AddColumn<string>(
            name: "nova_coluna",
            table: "Tabela",
            type: "text",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Reversão das alterações
        migrationBuilder.DropColumn(
            name: "nova_coluna",
            table: "Tabela");
    }
}
```

## Desenvolvimento

### Executando em Modo de Desenvolvimento

```bash
# Executar com hot reload
dotnet watch run --project medical-appointment-scheduling-api

# Executar normalmente
dotnet run --project medical-appointment-scheduling-api
```

### Acessando Swagger UI

Após iniciar a aplicação, acesse:
- **Swagger UI**: `http://localhost:5298/swagger` ou `https://localhost:7213/swagger`
- **Swagger JSON**: `http://localhost:5298/swagger/v1/swagger.json` ou `https://localhost:7213/swagger/v1/swagger.json`

### Adicionando um Novo Endpoint

1. **Criar/Atualizar Model** (se necessário):
```csharp
public class NovaEntidade
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    // ...
}
```

2. **Adicionar ao DbContext**:
```csharp
public DbSet<NovaEntidade> NovaEntidade { get; set; }
```

3. **Criar Interface do Repository**:
```csharp
public interface INovaEntidadeRepository
{
    Task<IEnumerable<NovaEntidade>> GetAllAsync();
    // ...
}
```

4. **Implementar Repository**:
```csharp
public class NovaEntidadeRepository : INovaEntidadeRepository
{
    // Implementação
}
```

5. **Registrar no Program.cs**:
```csharp
builder.Services.AddScoped<INovaEntidadeRepository, NovaEntidadeRepository>();
```

6. **Criar Controller**:
```csharp
[ApiController]
[Route("[controller]")]
public class NovaEntidadeController : ControllerBase
{
    private readonly INovaEntidadeRepository _repo;
    // ...
}
```

### Logging

A aplicação usa o sistema de logging do ASP.NET Core:

```csharp
private readonly ILogger<MeuController> _logger;

_logger.LogInformation("Operação realizada com sucesso");
_logger.LogWarning("Aviso: {Mensagem}", mensagem);
_logger.LogError(ex, "Erro ao processar: {Detalhes}", detalhes);
```

## Deploy

### Preparação para Produção

1. **Configurar Variáveis de Ambiente**:
```bash
export ConnectionStrings__DefaultConnection="..."
export Supabase__Url="..."
export Supabase__AnonKey="..."
```

2. **Build de Produção**:
```bash
dotnet publish -c Release -o ./publish
```

3. **Executar Migrations**:
```bash
dotnet ef database update --project medical-appointment-scheduling-api
```

### Docker (Opcional)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY publish/ .
ENTRYPOINT ["dotnet", "medical-appointment-scheduling-api.dll"]
```

### Configurações de Segurança

- ✅ Use HTTPS em produção
- ✅ Configure CORS adequadamente
- ✅ Use variáveis de ambiente para secrets
- ✅ Habilite rate limiting
- ✅ Configure logging adequado
- ✅ Use soft delete para dados sensíveis

## Troubleshooting

### Erro: "Unable to connect to database"

**Causa**: Connection string incorreta ou banco inacessível.

**Solução**:
1. Verifique a connection string em `appsettings.json`
2. Teste conexão com `psql` ou cliente PostgreSQL
3. Verifique firewall/rede

### Erro: "JWT validation failed"

**Causa**: Token expirado ou inválido.

**Solução**:
1. Verifique se o token está no formato `Bearer <token>`
2. Verifique expiração do token
3. Refaça login para obter novo token

### Erro: "Migration already applied"

**Causa**: Migration já foi aplicada no banco.

**Solução**:
```bash
# Verificar migrations aplicadas
dotnet ef migrations list

# Se necessário, remover migration
dotnet ef migrations remove
```

### Erro: "Enum conversion failed"

**Causa**: Valor no banco não corresponde ao enum.

**Solução**:
1. Verifique valores no banco de dados
2. Verifique configuração de `HasConversion` no `AppDbContext`
3. Certifique-se de que valores estão em lowercase (se aplicável)

### Performance: Queries Lentas

**Soluções**:
1. Adicione índices no banco de dados
2. Use `AsNoTracking()` para queries somente leitura
3. Implemente paginação para listagens grandes
4. Use `Include()` para eager loading quando necessário

## Recursos Adicionais

### Documentação Oficial

- [.NET 8.0 Documentation](https://learn.microsoft.com/dotnet/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/aspnet/core/)
- [Entity Framework Core Documentation](https://learn.microsoft.com/ef/core/)
- [Supabase Documentation](https://supabase.com/docs)

### Documentação Relacionada
- [Documentação do Frontend](https://github.com/computing-projects/medical-appointment-scheduling-frontend/tree/main)
    
### Ferramentas Úteis

- **Postman**: Testar endpoints da API
- **pgAdmin**: Gerenciar banco PostgreSQL
- **DBeaver**: Cliente universal de banco de dados
- **Swagger UI**: Documentação interativa da API

---

**Última Atualização**: 2025  
**Versão**: 1.0.0  
**.NET Version**: 8.0  
**Entity Framework Core**: 9.0.9
