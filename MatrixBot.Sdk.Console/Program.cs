var matrixBot = new MatrixBot.Sdk.MatrixBot();

matrixBot.OnEvent += (object? sender, MatrixBot.Sdk.MatrixBotEventArgs e) =>
{
    Console.WriteLine($"{e.RoomId} : {e.Event.Sender}");
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
    
    //matrixBot.GetJoinedRooms();
    //matrixBot.GetProfile();
    //matrixBot.PostRoomMessage();
}