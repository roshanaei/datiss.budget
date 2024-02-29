dotnet tool update --global dotnet-ef --version 6.0.27
dotnet build
dotnet ef --startup-project ../Datiss.Budget.Web/ database update
pause