using Godot;
using System;

public partial class DebugLevelGenerator : Node
{
    [Export] public PackedScene Hex { get; set; }
    [Export] public GridManager GridManager { get; set; }


    public Hex StartHex => startHex;
    private Hex startHex;
    private Hex endHex;

    public int CurrentLevel { get; set; }

    private int baseLength = 10;
    private int baseWidth = 15;
    private float stepX = 0.43f;
    private float stepY = 0.75f;
    private float heightOffsetFactor = 0.05f;
    

    public Hex GenerateLevel()
    {
        var fromY = GridManager.MaxY + 1;
        var toY = (endHex != null ? endHex.Coordinates.Y + 1 : 0) + baseLength;

        for (int y = fromY; y < toY; y++)
        {
            for (int x = 0; x < baseWidth; x++)
            {
                PlaceHexAt(x, y);
            }        
        }

        if (endHex != null)
        {
            startHex = endHex;
            startHex.UpdateState(global::Hex.HexState.FromState(global::Hex.HexStateType.Start));
            
            var endX = GD.RandRange(0, baseWidth - 1);
            var endY = GD.RandRange(startHex.Coordinates.Y + 2, startHex.Coordinates.Y + baseLength - 2);

            endHex = GridManager.GetAt(endX, endY);
            endHex.UpdateState(global::Hex.HexState.FromState(global::Hex.HexStateType.End));
        }
        else
        {
            startHex = GridManager.GetAt(5, 5);
            startHex.UpdateState(global::Hex.HexState.FromState(global::Hex.HexStateType.Start));

            endHex = GridManager.GetAt(10, 5);
            endHex.UpdateState(global::Hex.HexState.FromState(global::Hex.HexStateType.End));
        }
        
        return endHex;
    }

    private void PlaceHexAt(int x, int y)
    {
        var hex = Hex.Instantiate<Node3D>();
        hex.Name = $"Hex_{x}_{y}";
        this.AddChild(hex);

        var xPos = stepX * 2 * x;
        var yPos = GD.Randf() * heightOffsetFactor;
        var zPos = stepY * y;

        if (y % 2 == 0)
        {
            xPos += stepX;
        }

        hex.GlobalPosition = new(xPos, yPos, zPos);
        hex.Owner = GetTree().EditedSceneRoot;

        if (hex is Hex hexData)
        {
            hexData.SetBasePosition(hex.GlobalPosition);
            GridManager.RegisterHex(x, y, hexData);

            var randomOffset = GD.RandRange(5, 15);

            var spawnZ = 15f + randomOffset;
            var spawnY = -(spawnZ / 2.5f);

            hex.GlobalPosition += new Vector3(0, spawnY, spawnZ);
        }
    }
}
