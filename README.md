# Prueba Técnica – API REST .NET 10 (Minimal API)

## Descripción general

Este proyecto implementa una **API REST desarrollada en .NET 10 usando Minimal APIs**, como parte de una prueba técnica.  
La API incluye:

- CRUD de **Users**
- CRUD de **Addresses** relacionados a Users (1:N)
- Módulo de **conversión de divisas**
- Seguridad mediante **API Key**
- **Entity Framework Core + SQLite**
- **FluentValidation** para validación de requests
- Implementación del patrón **CQRS (Commands / Queries)**

El proyecto **compila y levanta correctamente**, cumpliendo los requisitos funcionales solicitados.

---

---

## Referencias técnicas

Versiones principales usadas:
.NET 10
Entity Framework Core 10.0.1
FluentValidation 12.1.1
SQLite
Patrón CQRS simplificado: Commands y Queries separados por carpeta.
Hash de contraseñas: BCrypt 4.0.3

---

## Cómo ejecutar el proyecto

### 1. Requisitos

- .NET SDK 10 instalado
- Git
- Opcional: `dotnet-ef` para migraciones

### 2. Clonar el repositorio

git clone https://github.com/Kevin-March/CurrencyConverter.git
cd CurrencyConverter

### 3. Crear Migraciones y aplicar las dependencias

dotnet ef migrations add InitialCreate
dotnet ef database update

### 3. Restaurar dependencias

dotnet restore

### 4. Ejecutar la API

dotnet run

#### La API estara disponible en localhost:5092

#### La base de datos se crea automáticamente al aplicar las migraciones.

## 🔑 API Key de prueba

Toda la API requiere enviar el header `X-API-KEY` con la API Key.  
La API Key de prueba configurada en `appsettings.json` es: api-key-prueba-akakakaka-456

> Si el header no está presente o es incorrecto, la API devolverá **401 Unauthorized**.

## Implementación

    •	Usuarios (Users) -> Crear, listar, obtener por Id, modificar y eliminar
    •	Direcciones (Addresses)-> Crear, listar por usuario, modificar y eliminar
    •	Monedas (Currencies)-> Listar, crear y conversión de divisas
    •	Seguridad-> Middleware de API Key

#### Escribi el HttpRequests.md para que puedan copiar y pegar los curls asi no tienen que escribir todos las pruebas de endpoints
