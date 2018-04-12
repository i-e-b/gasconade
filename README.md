# gasconade

<img src="https://github.com/i-e-b/gasconade/raw/master/icon.png"/>
https://www.nuget.org/packages/Gasconade

A self-documenting logging system for .Net -- like Swagger (UI) for logs.

Concepts
--------

### Configuring:
```csharp
public class MyListenerClass: ILogListener { . . . }
. . .
Log.AddListener(new MyListenerClass());
```

### Calling:
```csharp
Log.Warning(new FailedToSend{Client="WhizbangSvc", Reason="it was rejected by client"});
```

### Message Setup:
```csharp
// This is the structure of the message, with replacement blocks:
[LogMessageTemplate("Could not send message to {Client} because {Reason}")]
// This text explains why the message would be logged, and what to do about it -- to be used by an Operations team:
[LogMessageDescription("A message was to be sent to a 3rd party client, but a non-network error occured.",
                       Causes = "Sent as a warning if it is not customer-impacting, as an error otherwise.",
                       Actions = "Check configuration matches accounts if one client gets repeated errors.")]
public class FailedToSend {
    [LogParam("The 3rd party we were trying to send to")]
    public string Client {get;set;}

    [LogParam("Human-readable reason the message failed")]
    public string Reason {get;set;}
}
```

### Sample Result:
> Could not send message to WhizbangSvc because it was rejected by client

Gasconade UI
------------

Currently supplied for .Net MVC WebApi projects.
Add in global config like:
```csharp
protected void Application_Start() {
    . . .
    GlobalConfiguration.Configure(MyGasconadeConfig.Register);
}
```

And a class like:
```csharp
public class GasconadeConfig {
    public static void Register(HttpConfiguration config) {
        config.EnableGasconadeUi();
    }
}
```

That should enable a link at `http(s)://. . ./gasconade` that exposes the UI.
You can add a link from your SwaggerUI like this:

```csharp
config.EnableSwagger(c => {
            c.SingleApiVersion("v1", "SampleWebApi")
             .Description("A sample web application.<br/>For logging details, see " + GasconadeUi.Link("here"));
        })
    .EnableSwaggerUi(c => {
            c.DocumentTitle("My Swagger UI");
        });
```

If your message definitions are in a different assembly, you can add them like this:

```csharp
config.EnableGasconadeUi(c =>
    {
        c.AddAssembly(typeof(AnyMessageInTheProject).Assembly);
    });
```

You can add a link back to Swagger UI like this:

```csharp
config.EnableGasconadeUi(c =>
    {
        . . .
        c.AddSwaggerLink();
    });
```
