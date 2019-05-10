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

```console
python3 prebuild.py
nuget restore src/EasyExceptions.sln
msbuild src/EasyExceptions.sln /p:Configuration=Release /p:DefineConstants=STRONG_NAME /p:Version=$VERSION
```
