using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class GridManager : Node
{
    [Export] public Label DebugInfo { get; set; }

    private readonly Dictionary<(int x, int y), Hex> hexStore = new();

    public void RegisterHex(int x, int y, Hex hex)
    {
        hexStore.Add((x, y), hex);
    }

    public Hex GetAt(int x, int y)
    {
        return hexStore.GetValueOrDefault((x, y));
    }

    public Hex[] GetNeighbours(int x, int y)
    {
        return
        [
            GetAt(x - 1, y - 1),
            GetAt(x    , y - 1),
            GetAt(x + 1, y    ),
            GetAt(x    , y + 1),
            GetAt(x - 1, y + 1),
            GetAt(x - 1, y    ),
        ];
    }

    public (bool Success, Hex[] Path) CheckConnection((int x, int y) start, (int x, int y) end)
    {











        



        return (false, []);
    }
}
