# Gestión ITM – Módulo Profesores

## Asignatura

Programación de Software

## Objetivo del Taller

Implementar el ciclo de vida completo de una entidad utilizando *Arquitectura N-Capas (Clean Architecture)* aplicando:

* DTOs
* Servicios
* Repositorios
* Middleware global de excepciones
* AutoMapper

---

# Arquitectura del Proyecto

El proyecto sigue el patrón *Clean Architecture* dividido en capas:

GestionITM.API → Controladores, Middleware
GestionITM.Domain → Entidades, DTOs, Interfaces, Exceptions
GestionITM.Infrastructure → Repositorios, Servicios, Acceso a datos

Flujo de la aplicación:

Cliente → Controller → Service → Repository → Base de datos

---

# Entidad Implementada

Profesor

Campos:

* Id
* Nombre (requerido, máximo 100 caracteres)
* Especialidad
* Email (requerido)
* FechaContratacion

---

# DTOs

Se implementaron DTOs para separar la lógica de dominio de la exposición de datos.

ProfesorDto
Usado para lectura de datos.
No expone el campo *FechaContratacion*.

ProfesorCreateDto
Usado para registrar nuevos profesores.

---

# Reglas de Negocio

Durante el registro de profesores se aplican las siguientes validaciones:

* La especialidad no puede estar vacía.
* Si la especialidad es *Arquitectura* se registra en consola:

Perfil Senior Detectado

* Si el nombre del profesor es *Error* se genera una excepción controlada.

---

# Middleware Global de Excepciones

Se implementó un *ExceptionMiddleware* que captura errores en toda la API y devuelve una respuesta JSON estandarizada.

También se crearon excepciones personalizadas:

BadRequestException
NotFoundException
ConflictException
UnauthorizedException

---

# Manejo de Errores

| Error                  | Código HTTP |
| ---------------------- | ----------- |
| Especialidad vacía     | 400         |
| Error de prueba        | 400         |
| Profesor no encontrado | 404         |
| Email duplicado        | 409         |

---

# Capturas de Evidencia

## Tabla en SQL Server

(Insertar captura de la tabla Profesor creada)

## Registro exitoso en Swagger

(Insertar captura del POST funcionando)

## Manejo de errores por Middleware

(Insertar captura mostrando el mensaje *Error de prueba*)

---

# Bonus Implementado

Se agregó validación para evitar registrar profesores con el *mismo email*.

En caso de duplicado se lanza:

ConflictException → HTTP 409

---

# Tecnologías utilizadas

* ASP.NET Core 8
* Entity Framework Core
* AutoMapper
* SQL Server
* Swagger
* Clean Architecture