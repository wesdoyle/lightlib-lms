OpenCover.Console.exe  -target:"c:\program files\dotnet\dotnet.exe" ^
-targetargs:"test \"Library.Tests\Library.Tests.csproj\" -c Debug --no-build" ^
-hideskipped:File ^
-output:coverage\coverage.xml ^
-oldStyle ^
-filter:"+[Lib*]* -[Library.Tests*]* -[Library.Data*]*" ^
-register:user

codecov.exe -f "coverage\coverage.xml" -t 0ef905d6-a58d-4931-8519-cddaab4e2a6a
