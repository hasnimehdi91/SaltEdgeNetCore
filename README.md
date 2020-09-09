# SaltEdgeNetCore

SaltEdgeNetCore is a  client for Salt Edge spectre API.

## License

This is released under an Apache 2.0 license. See the file LICENSE.txt for more information.

## Documentation
The API corresponds to the [Salt Edge Documentation](https://docs.saltedge.com/)

### Usage
1- Download nuget package
```
<PackageReference Include="SaltEdgeNetCore" Version="1.2.0" />
``` 
2- In the ConfigureServices method of Startup.cs, register the SaltEdgeNetCore..
###### Test Mode 
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSaltEdge(options =>
    {
        options.AppId = "Your App Id";
        options.Secret = "Your secret";
        options.LiveMode = false;
     });
}
```
###### Live Mode ([Before you go live](https://docs.saltedge.com/general/#signature))
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSaltEdge(options =>
    {
        options.AppId = "Your App Id";
        options.Secret = "Your secret";
        options.LiveMode = true;
        options.WithExpiration = 10; // by default  expire  at is set to 10 minute 
        options.PrivateKeyPath = "private.pem" // your private RSA key path;
     });
}
```
3- Inject the service on the desired controller..
```csharp
public class TestController : Controller
    {
        private readonly ISaltEdgeClientV5 _clientV5;
        public TestController(ISaltEdgeClientV5 clientV5)
        {
            _clientV5 = clientV5;
        }
        
        // GET
        public IActionResult Index()
        {
            // list all categories supported by salt edge
            return Ok(_clientV5.CategoryList());
        }
    }
```
