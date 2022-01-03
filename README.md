# EasyExceptions #

Provides a method that recursively dumps all exception stack traces and properties into a string.

## Quick Start

Run the following command in the NuGet Package Manager Console:

```
Install-Package EasyExceptions
```

Use method `ExceptionDumpUtil.Dump`:

```
try
{
    throw new Exception();
}
catch (Exception ex)
{
    Console.WriteLine(ExceptionDumpUtil.Dump(ex));
}
```

## Build

```powershell
$env:ASSEMBLY_KEY_PATH=$AssemblySigningKeyPath + '\key.snk'
$env:BUILD_COUNTER='1234'
python prebuild.py
nuget restore src/EasyExceptions.sln
msbuild src/EasyExceptions.sln \
    -p:Configuration=Release \
    -p:DefineConstants=STRONG_NAME \
    -p:Version=$SEMANTIC_VERSION \
    -p:BuildCounter=$env:BUILD_COUNTER
```

## Deploy

Release deploy is made via
[NuGet.org web GUI](https://www.nuget.org/packages/manage/upload).

Test deploy (more info at
<https://docs.microsoft.com/ru-ru/azure/devops/artifacts/get-started-nuget>):
```powershell
nuget.exe push -Source "alexeykuptsov" -ApiKey az bin/EasyExceptions.$VERSION.nupkg
```

## Test

`1.` Run unit tests in EasyExceptions.Tests.dll

`2.` This test is parametrized by one of the following .NET versions (these  
are actual according to Google Trends):

* `.NET 5`,
* `.NET Core 3.1`,
* `.NET Framework 4.8`,
* `.NET Framework 4.7.2`.

`2.1` Create a console app project with the selected .NET version.
Remember its dependencies.

`2.2` Copy file `testData/nuget.config` to repository root.
It configures NuGet to work with test NuGet repository managed in Azure DevOps
at <https://dev.azure.com/alexeykuptsov/easy-exceptions/_packaging>.

`2.3` Install NuGet package `EasyExceptions`.

`2.4` *Expected result:*
Only `EasyExceptions.dll` is added.
