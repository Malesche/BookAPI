# BookAPI

## REST API 
The core of this project is a REST API storing and retrieving simple book and author data to an Entity Framework Core Code First Database. This part is fully unit tested.

Swagger Page                |  Response
:-------------------------:|:-------------------------:
![BookApiSwaggerPage](https://github.com/Malesche/BookAPI/assets/32207690/74e1774e-c03f-49d9-99b2-7659598e57e8)  |  ![BookApiSwaggerResponse](https://github.com/Malesche/BookAPI/assets/32207690/9e670fe2-4477-45bc-b060-55050503493e)

## Data Collection

The more experimental Data Collection Part started out retrieving Book Data from public APIs (OpenLibrary and GoogleBooks). I then moved on to processing bulk downloads from OpenLibrary, 
In order to import larger amounts of data, bulk downloads from OpenLibrary are processed. 

### WIP: 
Batch processing of the bulk downloads to speed up writing to the database.

<!--- ## Installation

Run `install.bat` from commandline.

## Start

Run `start.bat` from commandline.

clone repo
dotnet tool install --global dotnet-ef --version 7.0.10
dotnet ef database update


\library-api\DataCollectionPrototype> dotnet user-secrets init
dotnet user-secrets set apikey mmmmm

## See swagger site

Visit [http://localhost:5080/swagger](http://localhost:5080/swagger) -->



<!---
clone repo
dotnet tool install --global dotnet-ef --version 7.0.10
dotnet ef database update


\library-api\DataCollectionPrototype> dotnet user-secrets init
dotnet user-secrets set apikey mmmmm
-->
