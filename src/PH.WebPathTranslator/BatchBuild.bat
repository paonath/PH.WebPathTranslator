echo "Build nuget packages"



dotnet pack "P:\Dev\Gitlab\PH.WebPathTranslator\src\PH.WebPathTranslator\PH.WebPathTranslator\PH.WebPathTranslator.csproj" -c Release --include-symbols --include-source  -o "P:\Dev\Gitlab\PH.WebPathTranslator\BuildPackages"





echo "done"
