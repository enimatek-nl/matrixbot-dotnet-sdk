# **MatrixBot .NET SDK**

This open-source library allows you to build .NET chat bots compatible with [Matrix Protocol](http://www.matrix.org).
It has support for a limited subset of the APIs. 

# Use the SDK in your code
You can clone this repository and run `MatrixBot.Sdk.Console`.

Quickstart guide:

## 1. Create the Bot
```cs
var matrixBot = new MatrixBot.Sdk.MatrixBot();
```

Run it, and it will throw an error because there was no configuration available (yet).

## 2. Configure the Bot

Now you will have a `matrixbot.json` on disk, fill the blanks (serverUri, Username, Password ...) and run the sample again.

```json
{
    "Since": "",
    "AccessToken": "",
    "UserId": "",
    "ServerUri": "https://matrix.org",
    "Username": "john.doe",
    "Password": "s3cr3t"
}
```

You can also Implement your own storage system (eg. database, xml etc.) by passing your own class implementing `IMatrixBotStore` in the `MatrixBot` constructor.

## 3. Add Event handler

```cs
matrixBot.OnEvent += (object? sender, MatrixBot.Sdk.MatrixBotEventArgs e) =>
{
    Console.WriteLine($"{e.RoomId} : {e.Event.Sender}");
};
```

### 4. Start the Bot!

```cs
matrixBot.Start();
```

And we are done.