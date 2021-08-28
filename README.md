# Middleware Implementation

## Goal

To understand how middleware/filter works under the hood.

> Ref: [In/Out Middleware Explained (C# ASP.NET Core & JS Examples)](https://www.youtube.com/watch?v=xWWj0zGKS-k)

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


