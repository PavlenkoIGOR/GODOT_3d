using Godot;
using System;

public partial class platformMove : Node3D
{
    [Export] public int Rotation_Speed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        var rotation = Rotation;
        rotation.Y += Rotation_Speed / 100;
        Rotation = rotation;
    }
}
