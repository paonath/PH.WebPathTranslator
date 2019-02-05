# PH.WebPathTranslator [![NuGet Badge](https://buildstats.info/nuget/PH.WebPathTranslator)](https://www.nuget.org/packages/PH.WebPathTranslator/)

A tiny c# utility for Translate a web-relative path to a filesystem path (useful in scope where there is not a web server, for example inside services). The code is written in .NET C#. 

The package is available on  [nuget](https://www.nuget.org/packages/PH.WebPathTranslator) 

## Feature

-  Translate a Web-Relative path to FileSystem-Path:

    ```c#
    string ToFileSystemPath(string webrelativePath);
    ```

- Translate a FileInfo full path to web-relative:
    
    ```c#
    string ToWebRelativePath(FileInfo file);
    ```

- Translate a DirectoryInfo full path to web-relative

    ```c#
    string ToWebRelativePath(DirectoryInfo directory);
    ```
      
- Return a FileInfo from its web-relative position:

    ```c#
    FileInfo GetFile(string webrelativePath);
    ```

- Return a DirectoryInfo from its web-relative position:

    ```c#
    DirectoryInfo GetDirectory(string webrelativePath);
    ```
       

## Example

**Translate web-path to filesystem path**
```c#

IWebPathTranslator svc = new WebPathTranslator(@"c:\temp");
var result = svc.ToFileSystemPath("~/Folder1/Folder2");
//result is 'c:\temp\Folder1\Folder2'

```


