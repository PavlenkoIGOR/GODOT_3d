using Godot;
using System;

public partial class staff_scene : Node3D
{
	[Export]
	public AnimationPlayer FireingStaff_Anim;
	bool _staff_is_fireing = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("this is staff_scene");
        //FireingStaff_Anim = GetNode<AnimationPlayer>("FireingStaff_Anim");
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		//FireingStaff();

    }
	public void FireingStaff(AnimationPlayer FireingStaff_Anim)
	{
		if (!_staff_is_fireing)
		{
			if (FireingStaff_Anim != null)
			{
				GD.Print("FireingStaff_Anim was found");
			}
			else { GD.Print("FireingStaff_Anim was not found"); }

			FireingStaff_Anim.Play("Fire_Staff");
			_staff_is_fireing = true;
		}
		else
		{
			FireingStaff_Anim.Play("StopFire_Staff");
			_staff_is_fireing = false;
		}
	}
}
