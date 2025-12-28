using System.Numerics;

namespace MiniCactpotSolver;

public class Configuration
{
    public bool EnableAnimations = true;

    public Vector4 ButtonColor = new(1.0f, 1.0f, 1.0f, 0.80f);
    public Vector4 LaneColor = new(1.0f, 1.0f, 1.0f, 1.0f);

    public uint IconId = 61332;

    public static Configuration Load()
    {
        return new();
    }

    public void Save()
    {

    }
}