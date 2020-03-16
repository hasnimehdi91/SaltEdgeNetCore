# SaltEdgeNetCore

SaltEdgeNetCore is a  client for Salt Edge spectre API.

## License

This is released under an Apache 2.0 license. See the file LICENSE.txt for more information.

## Documentation
The API corresponds to the [Salt Edge Documentation](https://docs.saltedge.com/)

### Usage
1- Download nuget package
```
<PackageReference Include="SaltEdgeNetCore" Version="1.0.2" />
``` 
2- In the ConfigureServices method of Startup.cs, register the SaltEdgeNetCore..
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSaltEdge();
}
```

3- Inject the service on the controller..
```csharp
public class TestController : Controller
    {
        private readonly ISaltEdgeClientV5 _clientV5;
        public TestController(ISaltEdgeClientV5 clientV5)
        {
            _clientV5 = clientV5;
            clientV5.SetHeaders(new Dictionary<string, string>
            {
                {
                    "App-id", "your app id"
                },
                {
                    "Secret", "your secret"
                }
            });
        }
        
        // GET
        public IActionResult Index()
        {
            // list all categories supported by salt edge
            return Ok(_clientV5.CategoryList());
        }
    }
```
