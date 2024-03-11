$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
cd $dir

dotnet pack .\TemplatePack.csproj
dotnet new -u Rhyous.EntityAnywhere.Templates
dotnet new -i .\bin\Debug\Rhyous.EntityAnywhere.Templates.1.0.0.nupkg
