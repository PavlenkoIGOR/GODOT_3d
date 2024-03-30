using Godot;
using System;

public partial class MovingObstacle : Node3D
{
	[ExportGroup("Moving Platform")]
	[Export]
	public Vector3 end_pos;// = new Vector3(3,0,0);
	[Export]
	public float length_path;
	private bool Is_Moving_Backward = false;
	
	public Vector3 start_pos;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		start_pos = GlobalPosition;
		end_pos.X = start_pos.X + length_path;
		end_pos.Y = start_pos.Y;
		end_pos.Z = start_pos.Z;
		GD.Print($"coord obstacle {start_pos.X} {start_pos.Z}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		MoveObstacleForeward(delta);
    }

	public void MoveObstacleForeward(double delta) 
	{
		if (Is_Moving_Backward == false)
		{
            this.Position = this.Position.Lerp(end_pos, 0.05f);
            if (this.Position.X <= end_pos.X & this.Position.X >= (end_pos.X - 0.001f))
            {
				Is_Moving_Backward = true;
				start_pos = this.Position;
				end_pos = this.Position;
				end_pos.X = this.Position.X - 2 * length_path;
			}
        }
		if (Is_Moving_Backward == true)
		{
			this.Position = this.Position.Lerp(end_pos, 0.05f);
			if (this.Position.X <= -length_path & this.Position.X >= -length_path - 0.1)
			{
				Is_Moving_Backward = false;
				start_pos = this.Position;
                end_pos = this.Position;
                end_pos.X = this.Position.X + 2 * length_path;
			}
		}


	}

    public void _on_area_3d_body_entered(Node3D node)
	{
		GD.Print($"{node.Name} is entered");
		node.Reparent(this, true);
	}

    public void _on_area_3d_body_exited(Node3D node)
	{
        GD.Print($"{node.Name} is exited");
        //node.Reparent(this, true);
    }
}
