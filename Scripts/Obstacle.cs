using Godot;
using System;

public partial class Obstacle : Node3D
{
    private Vector3 _startPosition;
    [Export]
    public double distance_to_stop = 0.2;
    public bool is_moving_forward = true;
    public Vector3 start_position;
    public Vector3 end_position;
    public Vector3 target_position;
    [Export]
    public Vector3 move_length = new Vector3(3,0,0);
    [Export]
    public float speed = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        start_position = GlobalPosition;
        end_position = start_position + move_length;
        target_position = end_position;
        //ShowPosition();
        //_targetPosition = new Vector3(30, _startPosition.Y, _startPosition.Z); // Setting the target position
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        handle_movement(delta);
        handle_direction();

        //// Nesessary object, for example CharacterBody3D in the scenetree
        //var character = GetNode<CharacterBody3D>("CharacterBody3D");

        //Transform3D transform = Transform;
        //Vector3 axis = new Vector3(1, 0, 0); // Or Vector3.Right
        //float rotationAmount = 0.1f;

        //// Rotate the transform around the X axis by 0.1 radians.
        //transform.Basis = new Basis(axis, rotationAmount) * transform.Basis;
        //// shortened
        //transform.Basis = transform.Basis.Rotated(axis, rotationAmount);

        //Transform = transform;
    }
    public void ShowPosition()
    {
        _startPosition = GlobalTransform.Origin;
        GD.Print($"\"Obstacle\" start position is x: {_startPosition.X} y: {_startPosition.Y} z: {_startPosition.Z}");
    }

    public void handle_movement(double delta)
    {
        GlobalPosition = GlobalPosition.Lerp(target_position, (float)(speed * delta));
    }

    public void handle_direction()
    {
        if (is_moving_forward) {
            if (GlobalPosition.DistanceTo(end_position) <= distance_to_stop)
            {
                target_position = start_position;
                is_moving_forward = false;
            }
        }

        else if (GlobalPosition.DistanceTo(start_position) <= distance_to_stop)
        {
            target_position = end_position;
            is_moving_forward = true;
        }
    }

    public void _on_area_3d_body_entered(Node3D node)
    {
        //GD.Print($"entered {node.Name}");
        //var parNode = node.GetParent<MainScene>();
        //node.GetParent().RemoveChild(node);
        //if (parNode != null)
        //{
        //    GD.Print($"{parNode.Name}");
        //}
        this.AddChild(node);
        node.Reparent(this, false);
    }
}
