# CHALLENGE BACKEND - C# .NET alkemy-api
<h3 align="center">
:construction: Proyecto en construcción :construction:
</h3>

### En este proyecto, se utilizó:
 - Visual Studio
 - .NET
 - SQL SERVER
 - SENDGRID

## :mega:Indicaciones para inicializar el Proyecto
1. Utiliza para este proyecta Visual Studio 2019.
2. Crea un usuario en SENGRID para poder obtener la ApiKey que utilizaras ene el proyecto para el envio de correos.
3. Clona el repositorio desde esta url: https://github.com/gerardojao/alkemy-api.git.
4. Checa que las dependencias esten instaladas.
5. Crea un elemento en la solución llamado wwwroot y dentro de este archivo una carpeta 📁 File, respetando mayúsculas y minúsculas.
6. Esta es la cadena de conexión para con la Base de Datos, **"DevConnection": "Server=DESKTOP-O0NC63R\\SQLEXPRESS; database=alkemyProject; Trusted_Connection=true; MultipleActiveResultSets=true"**. Debes configurar tu archivo **appsettings.json**.
7. Para resguardar datos sensibles, utilice administrador de secretos, por esto, es importante que te posiciones sobe el nombre de la Solucion, click boton derecho del mouse, administrador de secretos.
8. En secretos, como key utiliza "API_KEY" y como valor el codigo de seguridad que te facilita SENGRID.
9. Procede a correr el proyector y a utilizar la ALKEMY_API
 
## :hammer:Funcionalidades del proyecto

1. :key:`Autenticación de Usuario`: Descripción de la funcionalidad 
 - endpoint login: /auth/register
   Deberá registrarse con un **email válido** y un **username**, con esta data podrá hacer el posterior login.
  - endpoint login: /auth/login
   Con el email y username registrado podrá hacer login, con lo que a su correo registrado le llegará un **token de seguridad** para el funcionameinto de la API.
3. `Listado de Personajes`: (/characters)
 - Listará los personajes con Imagen y Nombre. 
4. `Creación, Edición, Eliminación de Personajes (CRUD)`: Podrá crear nuevos personajes, editar y/o eliminar personajes existentes. 
 - POST /character/id
 - PUT /character/id 
 - DELETE /character/id.
5.  `Detalle de Personaje`: En el detalle deberán listarse todos los atributos del personaje, como así también sus películas o series relacionadas. El endpoint a utilizar: GET /character/id.
6.  `Búsqueda de Personajes`: A través de query parameter podra hacer una busqueda con los endpoints anteriores
 -  GET /characters?name=nombre
 -  GET /characters?age=edad
 -  GET /characters?movies=idMovie) 
 7. `Listado de Peliculas o Series` : Deberá mostrar solamente los campos imagen, título y fecha de creación.
El endpoint a utilizar: GET /movies.
8. `Detalle de Película / Serie con sus personajes`: Devolverá todos los campos de la película o serie junto a los personajes asociados a la misma. El endpoint a utilizar: GET /movies/id.
9. `Creación, Edición, Eliminación de Peliculas o Series (CRUD)`: Podrá crear nuevos personajes, editar y/o eliminar personajes existentes. 
 - POST /movies/id
 - PUT /movies/id 
 - DELETE /movies/id.
10. `Búsqueda de Personajes`: A través de query parameter podra hacer una busqueda con los endpoints anteriores
 -  GET /movies?name=nombre
 -  GET /movies?genre=idGenero
 -  GET /movies?order=ASc | DESC

