using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class GenerateHexes : Node3D
{
	[Export] public PackedScene Hex { get; set; }
	[Export] public float StepX { get; set; } = 0.43f;
	[Export] public float StepY { get; set; } = 0.75f;

	[Export] public int GenerateFromX { get; set; } = -25;
	[Export] public int GenerateFromY { get; set; } = -25;
	[Export] public int GenerateToX { get; set; } = 25;
	[Export] public int GenerateToY { get; set; } = 25;

	[Export] public int CurrentLevel { get; set; } = 1;
	private List<Node3D> GeneratedLevels { get; set; } = new();
	
	[Export] public float HeightOffsetFactor { get; set; } = 0.1f;

	[Export] bool Generate
	{
		get => false;
		set
		{
			if (value)
			{
				GenerateHexGrid();
			}
		}
	}

	[Export] bool Clear
	{
		get => false;
		set
		{
			if (value)
			{
				foreach (Node child in GetChildren())
				{
					child.QueueFree();
				}
			}
		}
	}

	[Export] bool ClearLevelSafe
	{
		get => false;
		set
		{
			Node LevelSafe = GetParent().GetNode("LevelSafe");
			if (value)
			{
				foreach (Node child in LevelSafe.GetChildren())
				{
					child.QueueFree();
				}
			}
		}
	}
	
	[Export] bool Playgrid
	{
		get => false;
		set
		{
			if (value)
			{
				generateNextLevel();
			}
		}
	}

	public void generateNextLevel()
	{
		if (CurrentLevel == 0)
		{
			GenerateStartGrid();
		}
		else
		{
			GeneratePlayGrid();
		}
		SaveCurrentLevel();
	}

	private void GenerateStartGrid()
	{
		var BaseLength = -20;
		var BaseWidth = 30;

		for (int y = 0; y > BaseLength; y--)
		{
			for (int x = 0; x < BaseWidth; x++)
			{
				PlaceHexAt(x, y);
			}        
		}
	}

	private void GeneratePlayGrid()
	{
		var BaseLength = -20;
		var BaseWidth = 30;

		for (int y = BaseLength*CurrentLevel; y > BaseLength*(CurrentLevel + 1); y--)
		{
			for (int x = 0; x < BaseWidth; x++)
			{
				PlaceHexAt(x, y);
			}        
		}
	}

	private void SaveCurrentLevel()
	{
		Node LevelSafe = GetParent().GetNode("LevelSafe");

		var levelNode = new Node3D();
        levelNode.Name = $"Level_{CurrentLevel}";

        var childrenToMove = new List<Node>(GetChildren());
		foreach (Node child in childrenToMove)
		{
			RemoveChild(child);
			levelNode.AddChild(child);
			child.Owner = LevelSafe;
		}
		
		LevelSafe.AddChild(levelNode);
		levelNode.Owner = GetTree().EditedSceneRoot;
		LevelSafe.Owner = GetTree().EditedSceneRoot; 
	{
		for (int y = GenerateFromY; y < GenerateToY; y++)
		{
			for (int x = GenerateFromX; x < GenerateToX; x++)
			{
				PlaceHexAt(x, y);
			}        
		}
	}

	private void PlaceHexAt(int x, int y)
	{
		var newHex = Hex.Instantiate<Node3D>();
		newHex.Name = $"Hex_{x}_{y}";   
		this.AddChild(newHex);	
		
		var xPos = StepX * 2 * x;
		var yPos = GD.Randf() * HeightOffsetFactor;
		var zPos = StepY * y;

		if (y % 2 == 0)
		{
			xPos += StepX;
		}

		newHex.GlobalPosition = new(xPos, yPos, zPos);
		newHex.Owner = GetTree().EditedSceneRoot;
		
	}
}
