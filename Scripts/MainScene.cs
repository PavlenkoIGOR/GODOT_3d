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

    //public void OnArea3D_BodyEntered(Node3D node)
    //{
    //    GD.Print($"==> entered {node.Name} ---");
    //    //var parNode = node.GetParent<MainScene>();
    //    //node.GetParent().RemoveChild(node);
    //    //if (parNode != null)
    //    //{
    //    //    GD.Print($"{parNode.Name}");
    //    //}


    //    //this.AddChild(node);
    //    var obstacle = GetNode<Obstacle>("Obstacle");
    //    if (obstacle != null)
    //    {
    //        GD.Print("Obstacle!!!!!!!!!!!!1");
    //    }
    //    var obstacleOnArea = obstacle.GetNode<Area3D>("InArea");
    //    if (obstacleOnArea != null)
    //    {
    //        GD.Print("obstacle InArea is found");
    //        GD.Print($"node innner is {node.Name}");
    //        GD.PrintErr($"{node.Position.X} {node.Position.Y} {node.Position.Z}");
    //    }
    //    node.Reparent(obstacleOnArea, true);
    //}

    public void OnArea3D_BodyExited(Node3D node)
    {
        GD.Print("--- body exited ===>");
    }
}
