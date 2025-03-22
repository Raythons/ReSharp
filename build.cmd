@echo off
echo Starting the build process...
REM Add your build commands here

dotnet build
dotnet test ..\Resharp.Tests\Resharp.Tests.csproj

echo Build completed successfully!