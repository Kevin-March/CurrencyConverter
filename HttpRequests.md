# HTTP Requests – API REST .NET 10 (Minimal API) (curl)

API Key de prueba: `api-key-prueba-akakakaka-456`  
Base URL: `http://localhost:5000` (ajustar según tu configuración)

---

## USERS

### Crear usuario (POST /users)

**Request válido**

```bash
curl -X POST http://localhost:5000/users \
  -H "X-API-KEY: api-key-prueba-akakakaka-456" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Juan",
    "email": "juan@test.com",
    "password": "123456"
  }'
```

**Request con errores (campos vacíos / email inválido)**

```bash
curl -X POST http://localhost:5000/users \
  -H "X-API-KEY: api-key-prueba-akakakaka-456" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "",
    "email": "correo-no-valido",
    "password": "123456"
  }'
```

**Email ya existe**

```bash
curl -X POST http://localhost:5000/users \
  -H "X-API-KEY: api-key-prueba-akakakaka-456" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Juan",
    "email": "juan@test.com",
    "password": "123456"
  }'
```

---

### Obtener usuario por id (GET /users/{id})

**Request válido**

```bash
curl -X GET http://localhost:5000/users/1 \
  -H "X-API-KEY: api-key-prueba-akakakaka-456"
```

**Request con id inexistente**

```bash
curl -X GET http://localhost:5000/users/9999 \
  -H "X-API-KEY: api-key-prueba-akakakaka-456"
```

---

### Login (POST /login)

**Request válido**

```bash
curl -X POST http://localhost:5000/login \
  -H "X-API-KEY: api-key-prueba-akakakaka-456" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "juan@test.com",
    "password": "123456"
  }'
```

**Request con credenciales incorrectas**

```bash
curl -X POST http://localhost:5000/login \
  -H "X-API-KEY: api-key-prueba-akakakaka-456" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "juan@test.com",
    "password": "claveincorrecta"
  }'
```

**Request con campos vacíos**

```bash
curl -X POST http://localhost:5000/login \
  -H "X-API-KEY: api-key-prueba-akakakaka-456" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "",
    "password": ""
  }'
```

---

## CURRENCIES

### Obtener todas las monedas (GET /currencies)

**Request válido**

```bash
curl -X GET http://localhost:5000/currencies \
  -H "X-API-KEY: api-key-prueba-akakakaka-456"
```

**Request sin API KEY**

```bash
curl -X GET http://localhost:5000/currencies
```

---

### Obtener moneda por código (GET /currencies/{code})

**Request válido**

```bash
curl -X GET http://localhost:5000/currencies/USD \
  -H "X-API-KEY: api-key-prueba-akakakaka-456"
```

**Request con código inexistente**

```bash
curl -X GET http://localhost:5000/currencies/XYZ \
  -H "X-API-KEY: api-key-prueba-akakakaka-456"
```

---

## CONVERSIONS

### Realizar conversión (POST /conversions)

**Request válido**

```bash
curl -X POST http://localhost:5000/conversions \
  -H "X-API-KEY: api-key-prueba-akakakaka-456" \
  -H "Content-Type: application/json" \
  -d '{
    "from": "USD",
    "to": "EUR",
    "amount": 100
  }'
```

**Request con moneda origen inválida**

```bash
curl -X POST http://localhost:5000/conversions \
  -H "X-API-KEY: api-key-prueba-akakakaka-456" \
  -H "Content-Type: application/json" \
  -d '{
    "from": "XXX",
    "to": "EUR",
    "amount": 100
  }'
```

**Request con cantidad inválida**

```bash
curl -X POST http://localhost:5000/conversions \
  -H "X-API-KEY: api-key-prueba-akakakaka-456" \
  -H "Content-Type: application/json" \
  -d '{
    "from": "USD",
    "to": "EUR",
    "amount": -50
  }'
```

---

## ERRORES GENERALES

### Request sin API KEY

```bash
curl -X GET http://localhost:5000/currencies
```

### Request con API KEY inválida

```bash
curl -X GET http://localhost:5000/currencies \
  -H "X-API-KEY: api-key-invalida"
```
