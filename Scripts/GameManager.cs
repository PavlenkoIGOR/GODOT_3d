using Godot;
using System;

public partial class GameManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsPhysicalKeyPressed(Key.Escape))
		{
			var currentScene = GetTree().CurrentScene; 
			if (currentScene != null) 
			{
				GD.Print($"{currentScene.Name}");
			}

			GD.Print("Escape?!");
            var packedScene = GD.Load<PackedScene>("res://Scenes/MainMenu.tscn").Instantiate();
            GetTree().Paused = true;
            currentScene.AddChild(packedScene);

            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
	}
}
