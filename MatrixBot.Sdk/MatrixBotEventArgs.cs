namespace MatrixBot.Sdk;

public class MatrixBotEventArgs : EventArgs
{
    public string RoomId { get; }
    public MatrixBotJsonSyncEvent Event { get; }

    public MatrixBotEventArgs(string roomId, MatrixBotJsonSyncEvent ev)
    {
        RoomId = roomId;
        Event = ev;
    }
}
