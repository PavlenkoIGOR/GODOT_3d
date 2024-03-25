using Godot;
using System;

public partial class Pyramid : Node
{
	[Export] public MeshInstance3D pyramydTip;
	[Export] public Vector3 StartPosition;
    [Export] public Vector3 TargetPosition;
	private float LerpWeight;
	[Export] public float RotationSpeed;
	sbyte _direction = 1;
    [Export] public float Amplitude;
    [Export] public float Speed;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        /*Lerp*/
        LerpWeight = Speed * (float)delta;
        StartPosition.Y = pyramydTip.Position.Y;
        StartPosition.Y = Mathf.Lerp(StartPosition.Y, _direction * Amplitude, LerpWeight);

        if (StartPosition.Y >= Amplitude - 0.1 || StartPosition.Y <= -Amplitude + 0.1)
        {
            _direction *= -1;
        }

        pyramydTip.Position = new Vector3(0, StartPosition.Y, 0);

        /*ease-in-out*/
        //var inn = 0.0f;
        //inn = Mathf.Ease(inn, -1);  
        //GD.Print($"{inn}");

        Vector3 rotate = pyramydTip.Rotation;
		rotate.Y += (float)delta * RotationSpeed / 10;
		pyramydTip.Rotation = rotate;
	}
}
