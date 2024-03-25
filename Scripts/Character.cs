using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Character : CharacterBody3D
{
	private const float Speed = 6.0f;
	private const float JumpVelocity = 4.0f;
	const float acceleration = 0.5f;
	[Export] public float MouseSensitivity;

	// accumulators
	private float _rotationX = 0f;
	private float _rotationY = 0f;
	private float LookAroundSpeed = -4f;

	public Node3D Head;
	public Camera3D Camera;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		Head = GetNode<Node3D>("Head");
		Camera = GetNode<Camera3D>("Head/Camera3D");
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("MoveLeft", "MoveRight", "MoveForeward", "MoveBackwards");
		Vector3 direction = (Head.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized(); //we're going in direction faces
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, acceleration);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, acceleration);
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			InputEventMouseMotion mouseMotion = (@event as InputEventMouseMotion);
			Input.MouseMode = Input.MouseModeEnum.Captured;
			
			//modify accumulated mouse rotation
			Head.RotateY(-mouseMotion.Relative.X * MouseSensitivity/1000);
			Camera.RotateX(-mouseMotion.Relative.Y * MouseSensitivity/1000);
			Vector3 cameraRot = Camera.Rotation;
			cameraRot.X = Mathf.Clamp(cameraRot.X, Mathf.DegToRad(-80f), Mathf.DegToRad(85f));
			Camera.Rotation = cameraRot;




			//// or like this :   (modify accumulated mouse rotation)
			
			//_rotationX += mouseMotion.Relative.X * LookAroundSpeed / 5000;
			//_rotationY += mouseMotion.Relative.Y * LookAroundSpeed / 5000;

			//// reset rotation
			//Transform3D transform = Transform;
			//transform.Basis = Basis.Identity;
			//Transform = transform;

			//RotateObjectLocal(Vector3.Up, _rotationX); // first rotate about Y
			//RotateObjectLocal(Vector3.Right, _rotationY); // then rotate about X
		}
	}
}
