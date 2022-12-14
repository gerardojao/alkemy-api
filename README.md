# CHALLENGE BACKEND - C# .NET alkemy-api
<h3 align="center">
:construction: Proyecto en construcci贸n :construction:
</h3>

### En este proyecto, se utiliz贸:
 - Visual Studio
 - .NET
 - SQL SERVER
 - SENDGRID

## :mega:Indicaciones para inicializar el Proyecto
1. Utiliza para este proyecta Visual Studio 2019.
2. Crea un usuario en SENGRID para poder obtener la ApiKey que utilizaras ene el proyecto para el envio de correos.
3. Clona el repositorio desde esta url: https://github.com/gerardojao/alkemy-api.git.
4. Checa que las dependencias esten instaladas.
5. Crea un elemento en la soluci贸n llamado wwwroot y dentro de este archivo una carpeta 馃搧 **File**, respetando may煤sculas y min煤sculas.
6. Esta es la cadena de conexi贸n para con la Base de Datos, **"DevConnection": "Server=DESKTOP-O0NC63R\\SQLEXPRESS; database=alkemyProject; Trusted_Connection=true; MultipleActiveResultSets=true"**. Debes configurar tu archivo **appsettings.json**.
7. Para resguardar datos sensibles, utilice administrador de secretos, por esto, es importante que te posiciones sobe el nombre de la Solucion, click boton derecho del mouse, administrador de secretos.
8. En secretos, como key utiliza "API_KEY" y como valor el codigo de seguridad que te facilita SENGRID.
9. Procede a correr el proyector y a utilizar la ALKEMY_API
 
## :hammer:Funcionalidades del proyecto

1. :key:`Autenticaci贸n de Usuario`: Descripci贸n de la funcionalidad 
 - endpoint login: /auth/register
   Deber谩 registrarse con un **email v谩lido** y un **username**, con esta data podr谩 hacer el posterior login.
  - endpoint login: /auth/login
   Con el email y username registrado podr谩 hacer login, con lo que a su correo registrado le llegar谩 un **token de seguridad** para el funcionameinto de la API.
3. `Listado de Personajes`: (/characters)
 - Listar谩 los personajes con Imagen y Nombre. 
4. `Creaci贸n, Edici贸n, Eliminaci贸n de Personajes (CRUD)`: Podr谩 crear nuevos personajes, editar y/o eliminar personajes existentes. 
 - POST /character/id
 - PUT /character/id 
 - DELETE /character/id.
5.  `Detalle de Personaje`: En el detalle deber谩n listarse todos los atributos del personaje, como as铆 tambi茅n sus pel铆culas o series relacionadas. El endpoint a utilizar: GET /character/id.
6.  `B煤squeda de Personajes`: A trav茅s de query parameter podra hacer una busqueda con los endpoints anteriores
 -  GET /characters?name=nombre
 -  GET /characters?age=edad
 -  GET /characters?movies=idMovie) 
 7. `Listado de Peliculas o Series` : Deber谩 mostrar solamente los campos imagen, t铆tulo y fecha de creaci贸n.
El endpoint a utilizar: GET /movies.
8. `Detalle de Pel铆cula / Serie con sus personajes`: Devolver谩 todos los campos de la pel铆cula o serie junto a los personajes asociados a la misma. El endpoint a utilizar: GET /movies/id.
9. `Creaci贸n, Edici贸n, Eliminaci贸n de Peliculas o Series (CRUD)`: Podr谩 crear nuevos personajes, editar y/o eliminar personajes existentes. 
 - POST /movies/id
 - PUT /movies/id 
 - DELETE /movies/id.
10. `B煤squeda de Personajes`: A trav茅s de query parameter podra hacer una busqueda con los endpoints anteriores
 -  GET /movies?name=nombre
 -  GET /movies?genre=idGenero
 -  GET /movies?order=ASc | DESC

