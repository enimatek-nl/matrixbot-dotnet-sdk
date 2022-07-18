var matrixBot = new MatrixBot.Sdk.MatrixBot();

matrixBot.OnEvent += async (object? sender, MatrixBot.Sdk.MatrixBotEventArgs e) =>
{
    Console.WriteLine($"{e.RoomId} : {e.Event.Sender} : {e.Event.Content?.Body}");
    if (sender is MatrixBot.Sdk.MatrixBot matrixBot)
    {
        await matrixBot.PostRoomMessage(e.RoomId, "hello world!", @"<b>hello world!</b>");
        var state = await matrixBot.StateEvent(e.RoomId, "m.room.name", "");
        var profile = await matrixBot.GetProfile(e.Event.Sender);
    }
};

matrixBot.Start();

Console.WriteLine("Started MatrixBot.");

var keepAlive = true;
Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) =>
{
    matrixBot.Stop();
    keepAlive = false;
    Console.WriteLine("Stopped MatrixBot.");
    e.Cancel = true;
};

while (keepAlive)
{
    Thread.Sleep(5000);
}
