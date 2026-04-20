using Godot;
using System;

[Tool]
public partial class DebugOutput : Node
{
    [Export] public Node3D CloneTarget { get; set; }
    [Export] public Node3D NodeToClone { get; set; }
    [Export] public int GridX
    {
        get => gridX;
        set
        {
            gridX = value;
            Update();
        }
    }
    [Export] public int GridY
    {
        get => gridY;
        set
        {
            gridY = value;
            Update();
        }
    }
    [Export] public Vector3 Result { get; set; } 

    private float stepX = 0.43f;
    private float stepY = 0.75f;
    private int gridX;
    private int gridY;

    private void Update()
    {
        var xPos = stepX * 2 * gridX;
        var yPos = Result.Y;
        var zPos = stepY * gridY;

        if (GridY % 2 == 0)
        {
            xPos += stepX;
        }

        Result = new(xPos, yPos, zPos);
    }

    [Export] public bool Place
    {
        get => false;
        set
        {
            if (value)
            {
                var clone = NodeToClone.Duplicate() as Node3D;
                CloneTarget.AddChild(clone);
                clone.Owner = GetTree().EditedSceneRoot;
                clone.Name = NodeToClone.Name;
                clone.Position = Result;
            }
        }
    }
}
