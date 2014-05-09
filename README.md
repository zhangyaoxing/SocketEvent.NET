SocketEvent.NET
===============
.NET SDK for my project SocketEvent

How to use
===============
- Connect to a server

```c#
var client = new SocketEventClient(id, url);
```

id: is a string identifier to indicate who the client is.
url: SocketEvent server address.

- Subscribe

```c#
SocketEventClient.Subscribe(
    "EventName",
    new Func<ISocketEventRequest, RequestResult>(
        (request) =>
        {
            // what do you want to do when event triggered.
            return RequestResult.Success;
        }),
    (response) =>
    {
        // event subscription succeeded.
    });
```

- Enqueue

Trigger an event

```c#
using (var client = new SocketEventClient(id, url))
{
    client.Enqueue(
        "EventName",
        1,  // try time(s)
        60, // timeout
        new
        {
            // dynamic object which you can use to pass some parameters.
        },
        (response) =>
        {
            // enqueue succeeded.
        });

    result = ResponseFactory.CreateInstance<bool>(
        ResponseStatus.Success,
        null,
        true);
}
```
