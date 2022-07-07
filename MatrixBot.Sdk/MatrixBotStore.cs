using System;
using System.Text.Json;

namespace MatrixBot.Sdk;


public interface IMatrixBotStore
{
    MatrixBotConfig? Read();
    void Write(MatrixBotConfig config);
}

public class MatrixBotConfig
{
    public string? Since { get; set; }
    public string? AccessToken { get; set; }
    public string? UserId { get; set; }
    public string ServerUri { get; set; } = "https://matrix.org";
    public string Username { get; set; } = "john.doe";
    public string Password { get; set; } = "s3cr3t";
}

class MatrixBotSimpleFileStorage : IMatrixBotStore
{
    MatrixBotConfig? IMatrixBotStore.Read()
    {
        try
        {
            var json = File.ReadAllText(@"matrixbot.json");
            return JsonSerializer.Deserialize<MatrixBotConfig>(json);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    void IMatrixBotStore.Write(MatrixBotConfig config)
    {
        File.WriteAllText(@"matrixbot.json", JsonSerializer.Serialize(config));
    }
}