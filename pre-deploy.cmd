dotnet restore

dotnet build TauCode.Messaging.Testing.sln -c Debug
dotnet build TauCode.Messaging.Testing.sln -c Release

dotnet test TauCode.Messaging.Testing.sln -c Debug
dotnet test TauCode.Messaging.Testing.sln -c Release

nuget pack nuget\TauCode.Messaging.Testing.nuspec