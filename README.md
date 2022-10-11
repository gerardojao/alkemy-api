# CHALLENGE BACKEND - C# .NET alkemy-api
<h3 align="center">
:construction: Proyecto en construcción :construction:
</h3>

## Indicaciones para inicializar el Proyecto
### En este proyecto, se utilizó:
 - Visual Studio
 - .NET
 - SQL SERVER
 - SENDGRID
 
## :hammer:Funcionalidades del proyecto

1. `Autenticación de Usuario`: Descripción de la funcionalidad 
 - endpoint login: /auth/register
   Deberá registrarse con un **email válido** y un **username**, con esta data podrá hacer el posterior login.
  - endpoint login: /auth/login
   Con el email y username registrado podrá hacer login, con lo que a su correo registrado le llegará un **token de seguridad** para el funcionameinto de la API.
3. `Listado de Personajes`: (/characters)
 - Listará los personajes con Imagen y Nombre. 
4. `Creación, Edición, Eliminación de Personajes (CRUD)`: POdrá crear nuevos personajes, editar y/o eliminar personajes existentes.
5.  `Detalle de Personaje`: En el detalle deberán listarse todos los atributos del personaje, como así también sus películas o series relacionadas.
6.  `Búsqueda de Personajes`: 
 -  GET /characters?name=nombre
 -  GET /characters?age=edad
 -  GET /characters?movies=idMovie)
 A traaves de query parameter podra hacer una busqueda con los endpoints anteriores
 7. `Listado de Peliculas o Series` : Deberá mostrar solamente los campos imagen, título y fecha de creación.
El endpoint a utilizar: GET /movies

