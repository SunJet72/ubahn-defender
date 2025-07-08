using System.Numerics;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    public const byte X = 1;
    public const byte C = 2;

    public NetworkButtons Buttons;
}