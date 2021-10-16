# Middleware Implementation

## Goal

- To understand how middleware/filter works under the hood.
- Write Unit test w/ xUnit and Moq.
- Calculate test coverage w/ coverlet.
- Learn how to use `IDisposal`.
- Learn how to achieve NUnit `[TestFixture]` attribute in XUnit.

## Notes

- **Middleware/filter == a function wraps another function**
- Types of middleware/filter
  - before middleware/filter
  ```csharp
  public class HttpClientMiddleware : DelegatingHandler
  {
      protected override async Task<HttpResponseMessage> SendAsync(
          HttpRequestMessage request, 
          CancellationToken cancellationToken)
      {
          // do before
          return await base.SendAsync(request, cancellationToken);
      }
  }
  
  ```
  - before/after middleware/filter
  ```csharp
  public class HttpClientMiddleware : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            // do before
            var response = await base.SendAsync(request, cancellationToken);
            // do after
            return response;
        }
    }
  ```

- Use js and C# to implement middleware/filter.
- About code
    - `Try` is a before middleware/filter
    - `Wrap` is a before/after middleware/filter
    ```csharp
    Action<string> mainAction = (msg) => { Console.WriteLine(msg); };
    PipeBuilder builder = new PipeBuilder(mainAction);

    Action<string> entry = builder
                .AddPipe(typeof(Try))
                .AddPipe(typeof(Wrap))
                .Build();
            entry("HELLO WORLD");

    /*
    Result:
    trying
    start
    HELLO WORLD
    end
    */
    ```

- How to unit test Console application?
    - [use `Console.SetOut` and `Console.SetIn` tricks to redirect STDOUT/STDIN](https://gist.github.com/asierba/ad9978c8b548f3fcef40)


- How to test internal class in another assembly?
    - In business logic code project, add `[assembly: InternalsVisibleTo("TestProject")]` attribute above namespace.
    - e.g., Program.cs
    ```csharp
    [assembly: InternalsVisibleTo("TestProject")]
    namespace simple_middleware
    {
        class Program {

        }
    }
    ```
- How to Solve Moq cannot access internal classes error?
    - Add `[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]` attribute
- Calculate code coverage by
    - Install Nuget package to test project
        - `dotnet add xUnitTestProject/xUnitTestProject.csproj package coverlet.collector` 
    - Start calculate
        - `coverlet.exe xUnitTestProject/bin/Debug/net5.0/xUnitTestProject.dll --target "dotnet" --targetargs "test . --no-build" --format json
        - `dotnet test --collect:"XPlat Code Coverage"`
`
- C# `using` syntax  == try-finally block
- Destructor/Finalizer
  - Dispose(false); // deal with unmanaged code 
    - is called by GC (after GC disposed all managed objects)
    - performance problem -> leads to two cycles of GC in an object
    - 1st cycle: mark objects for collection
    - 2nd cycle: collect objects
    - We can improve it by suppressing destructor if `Dispose` method was called


## Reference

- [In/Out Middleware Explained (C# ASP.NET Core & JS Examples)](https://www.youtube.com/watch?v=xWWj0zGKS-k)
- [IDisposable Design Pattern (.Net Core)](https://www.youtube.com/watch?v=9lmDphUMVsg)
- [Docker 教學 - .NET Core 測試報告 (Coverlet + ReportGenerator)](https://blog.johnwu.cc/article/docker-dotnet-coverage-report-generator.html)
- [xUnit issues - Add support for parametrixed test fixtures](https://github.com/xunit/xunit/issues/352#issuecomment-368135733)
- [mocking-system-console-behaviour](https://stackoverflow.com/questions/5852862/mocking-system-console-behaviour)
