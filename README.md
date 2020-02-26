# FMM Chanlenge

This was a coding challenge to do crud operations on files. I used Blazor web assembly in this challenge.
this code you can download a file/upload/view/delete/edit

## Getting Started

You can just download or cloen the project. You can run database migrations or create the db yourself I used my local db with one table
this is the code for creating db and the table. Then just make sure you change appsettings.json
```sql
create table FileData (
FileId int identity(1,1) primary key,
FileName varchar(max)
)
```


### Prerequisites

1. Visual studio 2019 preview
2. .net core 3.1
3 MicrosoftSql server

`

End with an example of getting some data out of the system or using it for a little demo

## Running the tests

I should have added test with x unit.


### And coding style tests


## Deployment

you could deploy this to azure app service and the dp to azure and it would be a live app

## Built With

* [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) - The web framework used
* [.netcore](https://docs.microsoft.com/en-us/dotnet/core/) - server/middleware
* [MSSQL](https://www.microsoft.com/en-us/sql-server/default.aspx) - microsoft sql server



## Acknowledgments

* steve sanderson for the tips on uploading files and javascript introp
* For the love of c# and using blazor
* I would of made the UI better added tests to the project. did more testing. 
