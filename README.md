# .NETCore3.1BatchJob

### Framework & Tools
- .NET Core 3.1
- Visual Studio Code
- C#
- SQL Server 2017 Express Edition

### Model Validation with [FluentValidation](https://fluentvalidation.net/)

Normally, in case of CSV Import, we first import data in raw format in DB in a staging table and then process the data from there to main data table with all validations enforced.
But there may be a scenario where we need to put validations before import
This is where FluentValidation package comes in handy.
```
 RuleFor(x => x.ORDER_ID).NotEmpty().MaximumLength(20);
 ```

### Csv Processing with [CsvHelper](https://joshclose.github.io/CsvHelper/)
To process CSV file with more enhanced capapbilities and control.
```
 Map(m=> m.ID).Ignore();
 Map(m=> m.ORDER_ID).Name("Order ID");
```            

### Logging with [Serilog](https://serilog.net/)
Logging has been implemented using Serilog with all logs written on a local file for every exception & wherever needed.
Also, all exception are thrown back to the caller, logged at common place in Start() method in App.cs

### DI Approach with AppSettings

AppSettings injected in DI container to make these settings available across the application with ease.
Also give more flexibility towards any possible extension with minimum code change or altering existing implementation.

### List of packages used:

- #### For Logging purpose :        
        Microsoft.Extensions.Logging 
        Serilog.Extensions.Logging 
        Serilog.Sinks.File

- #### For DI Service Container:         
        Microsoft.Extensions.DependencyInjection

- #### For configuration:              
        Microsoft.Extensions.Configuration, 
        Microsoft.Extensions.Configuration.Binder, 
        Microsoft.Extensions.Configuration.Json

- #### For SQL Data Client:              
        Microsoft.Data.SqlClient

- #### For CSV File processing:          
        CsvHelper

- #### For Data Validation:              
        FluentValidation

### Commands to make it work:

- build : ```dotnet build```
- build for release: ```dotnet build --configuration Release```
- run : ```dotnet run```
- publish for windows: ```dotnet publish --configuration Release --framework netcoreapp3.1 --runtime win10-x64```
- publish for linux: ```dotnet publish --configuration Release --framework netcoreapp3.1 --runtime linux-x64```

### Notes
- Update SQL Connection string from config to your local database.
- Run SQL script to create dependent table.



 



