using Godot;
using System;

public partial class MainScene : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _on_portal_3d_body_entered(Node3D node)
	{
		GD.Print("entered!");
	}
    public void _on_water_in_the_pool_area_body_entered(Node3D node)
	{
        GD.Print("in the pool!");
    }
}
