using Godot;
using System;

public partial class CubeMove : Node3D
{
	Transform3D pos;
	Vector3 ObjGP;
	[Export] public float Speed;
	private sbyte Direction = -1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		ObjGP = GlobalPosition;
		//GD.Print($"GlobalPosition: {ObjGP}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        GlobalPosition += new Vector3((float)delta * Speed * Direction, 0, 0);
		//GD.Print($"{GlobalPosition.X}");
		if (GlobalPosition.X <= -20)
		{
			Direction = 1;
		}
		if (GlobalPosition.X >= 20)
		{
			Direction = -1;
		}

    }
}

/*
GlobalPosition -  

*/