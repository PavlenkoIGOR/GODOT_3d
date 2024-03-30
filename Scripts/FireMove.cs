using Godot;
using System;

public partial class FireMove : OmniLight3D
{
	public Vector3 Start_Pos;
	public Vector3 Finish_Pos;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Start_Pos = this.GlobalPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
		var rand = randomNumberGenerator.Randf();
		Finish_Pos.X = Start_Pos.X + rand/10;
		Finish_Pos.Y = Start_Pos.Y + rand/10;
		Finish_Pos.Z = Start_Pos.Z + rand / 10;
		this.GlobalPosition = this.GlobalPosition.Lerp(Finish_Pos, 0.5f/2);
		if (this.GlobalPosition == Finish_Pos)
		{
			this.GlobalPosition = Start_Pos;
		}
		//this.Position = Start_Pos;
	}
}
