# Payment API with Redis

A .NET 8 API for payment processing with Redis as cache/storage, following Clean Architecture and SOLID principles.

## 🚀 Features

- **Cash Out**: Brazilian bank transfer processing
- **Transaction Query**: Search by transaction ID
- **Health Check**: API and Redis monitoring
- **Validations**: Complete Brazilian banking data validation
- **Docker**: Full containerization with docker-compose

## 🏗️ Architecture

```
Payment.API/
├── Controllers/        # API endpoints
├── Services/          # Business logic
├── Interfaces/        # Contracts/Abstractions
├── Data/
│   ├── Context/       # Redis context
│   └── Repositories/  # Data access
├── Models/
│   ├── Requests/      # Input DTOs
│   ├── Responses/     # Output DTOs
│   └── Entities/      # Domain entities
├── Validators/        # FluentValidation validators
├── Extensions/        # DI extension methods
├── Configurations/    # Settings
└── Middlewares/       # Custom middlewares
```

## 📋 Prerequisites

- Docker and Docker Compose
- .NET 8 SDK (for development)
- Redis (via Docker)

## 🐳 Docker Execution (Production)

### Start Complete Stack
```bash
# Build and start all services
docker-compose up -d --build

# Check container status
docker ps

# View logs
docker-compose logs -f payment-api
docker-compose logs -f redis

# Stop all services
docker-compose down

# Stop and remove volumes
docker-compose down -v
```

### Available Services
| Service | URL | Description |
|---------|-----|-----------|
| Payment API | http://localhost:5000 | Main API |
| Swagger UI | http://localhost:5000/swagger | Interactive documentation |
| Health Check | http://localhost:5000/health | Application status |
| Redis Commander | http://localhost:8081 | Redis web interface |
| Redis | localhost:6379 | Database (internal) |

## 💻 Local Development

### 1. Start Redis Only
```bash
# Start Redis and Redis Commander for development
docker-compose up redis redis-commander -d
```

### 2. Run API Locally
```bash
# Navigate to API directory
cd Payment.API

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

### Development Services
| Service | URL | Description |
|---------|-----|-----------|
| Payment API | http://localhost:5004 | API in development |
| Swagger UI | http://localhost:5004/swagger | Local documentation |
| Redis | localhost:6379 | Redis via Docker |
| Redis Commander | http://localhost:8081 | Redis interface |

## 📡 API Endpoints

### Cash Out (Transfer)
**POST** `/api/payment/cashout`

**Request Body:**
```json
{
  "transactionId": "550e8400-e29b-41d4-a716-446655440000",
  "sourceAccount": {
    "bankCode": "001",
    "agency": "1234", 
    "agencyDigit": "5",
    "accountNumber": "567890",
    "accountDigit": "1",
    "accountType": "CC",
    "documentNumber": "12345678901",
    "accountHolderName": "João da Silva"
  },
  "destinationAccount": {
    "bankCode": "237",
    "agency": "5678",
    "agencyDigit": "9", 
    "accountNumber": "123456",
    "accountDigit": "7",
    "accountType": "CC",
    "documentNumber": "98765432100",
    "accountHolderName": "Maria dos Santos"
  },
  "amount": 1500.50,
  "paymentDate": "2024-12-31T10:30:00Z"
}
```

**Response:** `202 Accepted`

### Query Transaction
**GET** `/api/payment/cashout/{transactionId}`

**Response:** `200 OK` with transaction data or `404 Not Found`

### Health Check
**GET** `/health`

**Response:** `200 OK` if healthy, `503 Service Unavailable` if Redis offline

## 🔧 Configuration

### Environment Variables

#### Production (Docker)
```bash
ASPNETCORE_ENVIRONMENT=Production
Redis__ConnectionString=redis:6379,defaultDatabase=0,connectTimeout=5000,syncTimeout=250,asyncTimeout=250,connectRetry=3,abortConnect=false,keepAlive=180
```

#### Development
Settings are in `appsettings.Development.json`:
```json
{
  "Redis": {
    "ConnectionString": "localhost:6379,defaultDatabase=0,connectTimeout=5000,syncTimeout=250,asyncTimeout=250,connectRetry=3,abortConnect=false,keepAlive=180"
  }
}
```

### Redis Settings
- **Connection Timeout**: 5000ms
- **Operation Timeout**: 250ms (sync and async)
- **Retry**: 3 attempts
- **Keep Alive**: 180 seconds

## 📊 Validations

The API automatically validates:

### Banking Data
- **Bank Code**: 3 required digits
- **Agency**: 4 required digits
- **Agency Digit**: 0 or 1 character
- **Account Number**: Required
- **Account Digit**: 1 required character
- **Account Type**: CC (Current Account) or CP (Savings)
- **CPF/CNPJ**: 11 or 14 characters
- **Account Holder Name**: Minimum 2 characters

### Transaction
- **TransactionId**: Required UUID
- **Amount**: Greater than zero
- **Payment Date**: Cannot be in the past

## 🧪 Testing

### Using HTTP file
The project includes `Payment.API.http` with request examples:

```http
### Production - Cash Out
POST http://localhost:5000/api/payment/cashout
Content-Type: application/json

### Production - Query
GET http://localhost:5000/api/payment/cashout/{guid}

### Health Check  
GET http://localhost:5000/health
```

### Using cURL
```bash
# Health Check
curl -X GET http://localhost:5000/health

# Cash Out
curl -X POST http://localhost:5000/api/payment/cashout \
  -H "Content-Type: application/json" \
  -d '{
    "transactionId": "550e8400-e29b-41d4-a716-446655440000",
    "sourceAccount": { ... },
    "destinationAccount": { ... },
    "amount": 1500.50,
    "paymentDate": "2024-12-31T10:30:00Z"
  }'

# Query transaction
curl -X GET http://localhost:5000/api/payment/cashout/550e8400-e29b-41d4-a716-446655440000
```

## 🔍 Monitoring

### Logs
```bash
# API logs
docker-compose logs -f payment-api

# Redis logs
docker-compose logs -f redis

# All services logs
docker-compose logs -f
```

### Redis Commander
Access http://localhost:8081 to:
- View stored keys
- Monitor performance
- Execute Redis commands

### Health Checks
- **Endpoint**: `/health`
- **Checks**: API status + Redis connectivity
- **Docker**: Automatic health check configured

## 🛠️ Technologies

- **.NET 8** - Main framework
- **ASP.NET Core** - Web API
- **Redis** - Cache/Storage with StackExchange.Redis
- **FluentValidation** - Validations
- **Docker** - Containerization
- **Swagger/OpenAPI** - Documentation

## 📝 Implementation Notes

### Clean Architecture
- **Controllers**: Only receive requests and return responses
- **Services**: Business logic
- **Repositories**: Data access (Redis)
- **Interfaces**: Contracts between layers

### SOLID Principles
- **S**: Each class has single responsibility
- **O**: Extensible via interfaces
- **L**: Substitution via abstractions
- **I**: Segregated interfaces by responsibility  
- **D**: Dependencies inverted via DI

### Performance
- **Connection Pooling**: StackExchange.Redis manages automatically
- **Optimized Timeouts**: 250ms for Redis operations
- **Serialization**: JsonSerializer with optimized options
- **Health Checks**: Continuous monitoring