OpenCover.Console.exe  -target:"c:\program files\dotnet\dotnet.exe" ^
-targetargs:"test \"Library.Tests\Library.Tests.csproj\" -c Debug --no-build" ^
-mergeoutput -hideskipped:File ^
-output:coverage\coverage.xml ^
-oldStyle ^
-filter:"+[Lib*]* -[Library.Tests*]* -[Library.Data*]*" ^
-register:user
