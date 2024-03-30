using Godot;
using System;
using System.Numerics;

public partial class Obelisk : Node3D
{
	[Export] public MeshInstance3D Platform;
	[Export] float rotation_speed;
    int angle_in_radians = 5;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        var rotation = Platform.Rotation;
		rotation.Y += rotation_speed/100;
        Platform.Rotation = rotation;
    }
}
