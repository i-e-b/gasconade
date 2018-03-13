# gasconade
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
                       "Sent as a warning if it is not customer-impacting, as an error otherwise.",
                       "Check configuration matches accounts if one client gets repeated errors.")]
public class FailedToSend {
    [LogParam("The 3rd party we were trying to send to")]
    public string Client {get;set;}

    [LogParam("Human-readable reason the message failed")]
    public string Reason {get;set;}
}
```

### Sample Result:
> Could not send message to WhizbangSvc because it was rejected by client


TODO
----

* [ ] Historical/Obsolete marker
* [ ] Fluent config
