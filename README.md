# 🚀 Sistema Distribuído de Monitoramento e Alertas

## 📋 Sobre o Projeto

Este é um sistema completo de monitoramento e alertas construído com arquitetura de microserviços. O projeto foi desenvolvido para demonstrar conceitos avançados de desenvolvimento backend, incluindo comunicação assíncrona, cache distribuído, observabilidade e orquestração de containers.

O sistema permite:
- ✅ Monitorar recursos e serviços em tempo real
- 🔔 Enviar alertas quando limites são ultrapassados
- 📊 Visualizar métricas e logs centralizados
- 🔍 Rastrear requisições através de múltiplos serviços
- 💾 Cache distribuído para alta performance

## 🛠️ Stack Tecnológica

### Backend
- **.NET 8** - Framework principal para desenvolvimento dos microserviços
- **C# 12** - Linguagem de programação
- **ASP.NET Core Web API** - Para criação das APIs RESTful

### Comunicação e Mensageria
- **RabbitMQ** - Message broker para comunicação assíncrona entre microserviços
- **Azure Service Bus** (alternativa) - Serviço de mensageria gerenciado

### Cache e Persistência
- **Redis** - Cache distribuído em memória
- **SQL Server** (ou PostgreSQL) - Banco de dados relacional
- **MongoDB** - Banco de dados NoSQL para logs e métricas

### API Gateway
- **Ocelot** - API Gateway para roteamento e agregação de requisições

### Observabilidade (ELK Stack)
- **Elasticsearch** - Motor de busca e análise de dados
- **Logstash** - Pipeline de processamento de logs
- **Kibana** - Visualização de dados e dashboards

### Infraestrutura
- **Docker** - Containerização dos serviços
- **Docker Compose** - Orquestração local dos containers

### Documentação e Testes
- **Swagger/OpenAPI** - Documentação interativa das APIs
- **xUnit** - Framework de testes unitários
- **Testcontainers** - Testes de integração com containers

## 🏗️ Arquitetura

```
┌─────────────┐
│   Cliente   │
└──────┬──────┘
       │
       ▼
┌─────────────────────────────────────┐
│         API Gateway (Ocelot)        │
└─────────────────────────────────────┘
       │
       ├──────────────┬──────────────┬──────────────┐
       ▼              ▼              ▼              ▼
┌──────────┐   ┌──────────┐   ┌──────────┐   ┌──────────┐
│ Monitor  │   │  Alert   │   │  Metric  │   │  Report  │
│ Service  │   │ Service  │   │ Service  │   │ Service  │
└────┬─────┘   └────┬─────┘   └────┬─────┘   └────┬─────┘
     │              │              │              │
     └──────────────┴──────────────┴──────────────┘
                    │
          ┌─────────┴─────────┐
          ▼                   ▼
    ┌──────────┐        ┌──────────┐
    │ RabbitMQ │        │  Redis   │
    └──────────┘        └──────────┘
          │
          ▼
    ┌──────────────────────────┐
    │ ELK Stack (Observability)│
    └──────────────────────────┘
```

## 📦 Microserviços

### 1. Monitor Service
- Coleta métricas de recursos (CPU, memória, disco)
- Monitora disponibilidade de serviços
- Publica eventos no RabbitMQ

### 2. Alert Service
- Consome eventos de monitoramento
- Avalia regras de alertas
- Envia notificações (email, SMS, webhook)

### 3. Metric Service
- Armazena métricas históricas
- Fornece APIs para consulta de dados
- Implementa agregações e estatísticas

### 4. Report Service
- Gera relatórios periódicos
- Exporta dados em diferentes formatos
- Agenda tarefas com background jobs

### 5. API Gateway
- Ponto único de entrada
- Roteamento de requisições
- Rate limiting e autenticação

## 🚀 Como Executar

### Pré-requisitos
- Docker Desktop instalado
- .NET 8 SDK instalado
- Git

### Passos

1. **Clone o repositório**
```bash
git clone <seu-repositorio>
cd distributed-monitoring-system
```

2. **Execute com Docker Compose**
```bash
docker-compose up -d
```

3. **Acesse os serviços**
- API Gateway: http://localhost:5000
- Swagger UI: http://localhost:5000/swagger
- Kibana: http://localhost:5601
- RabbitMQ Management: http://localhost:15672 (user: guest, pass: guest)

## 📚 Documentação Adicional

- [Conceitos e Fundamentos](./docs/CONCEITOS.md) - Explicação detalhada de todos os conceitos
- [Checklist de Desenvolvimento](./docs/CHECKLIST.md) - Passo a passo para construir o projeto
- [Guia de Arquitetura](./docs/ARQUITETURA.md) - Decisões arquiteturais e padrões

## 🎯 Objetivos de Aprendizado

Este projeto cobre:
- ✅ Fundamentos de C# e .NET
- ✅ Arquitetura de microserviços
- ✅ Comunicação assíncrona com message brokers
- ✅ Padrões de design (Repository, CQRS, Event-Driven)
- ✅ Cache distribuído
- ✅ Observabilidade e logging
- ✅ Containerização com Docker
- ✅ API Gateway e roteamento
- ✅ Health checks e resiliência
- ✅ Testes automatizados

## 📈 Roadmap

- [x] Estrutura inicial do projeto
- [ ] Implementação do Monitor Service
- [ ] Implementação do Alert Service
- [ ] Implementação do Metric Service
- [ ] Implementação do Report Service
- [ ] Configuração do API Gateway
- [ ] Integração com RabbitMQ
- [ ] Configuração do Redis
- [ ] Setup do ELK Stack
- [ ] Testes automatizados
- [ ] Documentação completa

## 🤝 Contribuindo

Este é um projeto de portfólio pessoal, mas sugestões são bem-vindas!

## 📝 Licença

MIT License

---

**Desenvolvido por:** [Seu Nome]
**Objetivo:** Projeto de portfólio para demonstrar habilidades em .NET e arquitetura de microserviços
"# distributed-monitoring-system" 
