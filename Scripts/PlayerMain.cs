 using Godot;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;


public partial class PlayerMain : CharacterBody3D
{
    [Signal]
    public delegate void ClickEventHandler();//1**

    [ExportGroup("Character Speeds")]
    [Export] float JumpVelocity = 4.5f;
    [Export] float MouseSensitivity = 0.25f;
    [Export] float CameraAcceleration = 2.0f;

    [Export] float CurrentSpeed = 5.0f;
    [Export] float WalkingSpeed = 3.0f;
    [Export] float SprintSpeed = 10.0f;
    [Export] float CrouchSpeed = 3.0f;
    [Export] float SlideSpeed = 7.0f;
    float speed;
    float mouse;
    float player_y_axis11;
    // A default value of 0.4 is a good starting point, stay between 0.01 and 1.0

    bool is_walking = false;
    bool _is_sprinting = false;
    bool _is_crouch = false;
    bool is_sliding = false;

    CollisionShape3D collision_shape_normal;
    CollisionShape3D collision_shape_crouch;
    public AnimationPlayer animPl;
    [Export] Node3D head_node;
    [Export] Camera3D camera_node;
    [Export] RayCast3D raycast_node;
    [Export] public Node3D Hand;
    [Export] public SpotLight3D flash_light;
    
    bool flashlight_IsOn = false;

    Key move_foreward_key;
    Key move_backward_key;
    Key move_left_key;
    Key move_right_key;

    bool _isGrounded;
    public CollisionShape3D collSignalizer;
    [Export]
    public CollisionShape3D StandingCollision;
    [Export]
    public CollisionShape3D CrouchedCollision;
    private RichTextLabel richtext;

    #region from https://www.youtube.com/watch?v=9du4wUOGqnc
    Vector3 direction = Vector3.Zero;
    float player_y_axis = 0.0f;
    float head_node_x_axis = 0.0f;
    #endregion

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    public override void _Ready()
    {
        GD.PrintErr($"{this.Position.X} {this.Position.Y} {this.Position.Z}");
        animPl = GetNode<AnimationPlayer>("AnimationPlayer");
        /*
        //OnCollisionSignalaizerTreeEntered();
        //var area = GetNode<Area3D>("AreaSignalizer");
        //collSignalizer = GetNode<CollisionShape3D>("AreaSignalizer/CollisionSignalizer");
        //if (collSignalizer != null) { GD.Print("Signalizer was founded"); }
        //collSignalizer.Connect("area_entered", new Callable(this, MethodName._on_collision_signalizer_child_entered_tree));
        */
        var crossHair = this.GetChild(0, true);
        //var richtext = crossHair.GetChild(1);
        richtext = crossHair.GetNode<RichTextLabel>("Console");
        

        this.Connect("Click", new Callable(this, "On_Tik")); //1**
    }

    public void On_Tik()//1**
    {
        GD.Print("Was jumped");
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity; // if in the tutorials you write in gdscript like "velocity.x = ...." then this is such a speed 

        if (!IsOnFloor())
        {
            velocity.Y -= gravity * (float)delta;
            //this.EmitSignal("Click");
        }
        //handle Jump
        if (Input.IsActionPressed("ui_accept") & IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        //As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("MoveLeft", "MoveRight", "MoveForeward", "MoveBackwards");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized(); ////////original

        #region for head.Rotation.Y lerp rotation. GEMORRROY but its working exactly. from https://www.youtube.com/watch?v=9du4wUOGqnc
        // Creating a new variable to store the rotation angle
        //float newRotationY = Mathf.LerpAngle(head_node.Rotation.Y, -Mathf.DegToRad(head_node_y_axis), 5 * (float)delta);

        //// Creating a new variable to store the current rotation
        //Vector3 currentRotation = head_node.Rotation; // = new Vector3(head_node.Rotation.X, Mathf.LerpAngle(head_node.Rotation.Y, -Mathf.DegToRad(head_node_y_axis/10), 5*(float)delta), head_node.Rotation.Z);

        //// Setting the changed angle to the current rotation variable
        //currentRotation.Y = newRotationY;

        //// Applying a new rotation
        //head_node.Rotation = currentRotation;

        //////or
        //head_node.Rotation = new Vector3(head_node.Rotation.X, Mathf.LerpAngle(head_node.Rotation.Y, -Mathf.DegToRad(head_node_y_axis / 10), 2 * (float)delta), head_node.Rotation.Z);

        //from https://www.youtube.com/watch?v=9du4wUOGqnc
        //camera_node.Rotation = new Vector3( Mathf.LerpAngle(camera_node.Rotation.X, -Mathf.DegToRad(camera_node_x_axis), 5 * (float)delta),
        //                                    camera_node.Rotation.Y,
        //                                    camera_node.Rotation.Z);
        #endregion

        #region problem movement hand with flashlight
        //// Creating a new variable to store the rotation angle
        //float newRotationY = Mathf.LerpAngle(head_node.Rotation.X, -Mathf.DegToRad(head_node_x_axis) / 10, 5 * (float)delta);
        //float newRotationX = Mathf.LerpAngle(-Mathf.DegToRad(player_y_axis) / 10, head_node.Rotation.Y, 5 * (float)delta);

        ////// Creating a new variable to store the current rotation
        //Vector3 currentRotation = new Vector3(newRotationX, newRotationY, Hand.Rotation.Z);

        ////// Applying a new rotation
        //Hand.Rotation = currentRotation;
        #endregion

        // Creating a new variable to store the rotation angle
        float newRotationX = (float)Mathf.LerpAngle(flash_light.Rotation.X, head_node.Rotation.X, 20 * delta);
        float newRotationY = (float)Mathf.LerpAngle(flash_light.Rotation.Y, mouse, 20 * delta);
        float newRotationZ = (float)Mathf.LerpAngle(flash_light.Rotation.Z, head_node.Rotation.Z, 20 * delta);
        //// Applying a new rotation
        flash_light.Rotation = new Vector3(newRotationX, newRotationY, newRotationZ);


        speed = CurrentSpeed;

        #region craunch
        CrounchMethod(CrouchSpeed, CurrentSpeed);
        #endregion

        #region Run
        RunMethod(SprintSpeed, CurrentSpeed);
        #endregion

        #region Flashlight
        FlashLightTurnOnOff();
        #endregion

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
        }
        Velocity = velocity;
        MoveAndSlide();
    }



    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion mouseMotion = (@event as InputEventMouseMotion);
            /*
            ////modify accumulated mouse rotation
            //head_node.RotateY(-mouseMotion.Relative.X * MouseSensitivity / 1000);
            //camera_node.RotateX(-mouseMotion.Relative.Y * MouseSensitivity / 1000);

            //Vector3 cameraRot = camera_node.Rotation;
            //cameraRot.X = Mathf.Clamp(cameraRot.X, Mathf.DegToRad(-80f), Mathf.DegToRad(85f));
            //camera_node.Rotation = cameraRot;
            */
            #region from https://www.youtube.com/watch?v=9du4wUOGqnc - neccessary finished!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            player_y_axis += (@event as InputEventMouseMotion).Relative.X * MouseSensitivity / 10;
            head_node_x_axis += (@event as InputEventMouseMotion).Relative.Y * MouseSensitivity / 10;
            mouse = (@event as InputEventMouseMotion).Relative.X * MouseSensitivity/450;
            #endregion

            //Body
            #region this is from https://www.youtube.com/watch?v=yKV37k1CdMY
            Rotation = new Vector3(
                Rotation.X,
                Rotation.Y - mouseMotion.Relative.X / 1000 * MouseSensitivity,
                Rotation.Z
                );


            //for eyes / original
            head_node.Rotation = new Vector3(
                Mathf.Clamp(head_node.Rotation.X - mouseMotion.Relative.Y / 1000 * MouseSensitivity, -1.5f, 1.5f),
                head_node.Rotation.Y,
                head_node.Rotation.Z
                );
            #endregion
        }
    }

    //Run method
    void RunMethod(float sprintSpeed, float currentSpeed)
    {
        if (Input.IsActionPressed("Run"))
        {
            if (!_is_sprinting)
            {
                _is_sprinting = true;
            }
            speed = sprintSpeed;
        }
        else
        {
            if (_is_sprinting)
            {
                _is_sprinting = false;
            }
            speed = currentSpeed;
        }
    }

    //Crounch method
    void CrounchMethod(float crounchSpeed, float currentSpeed)
    {
        if (Input.IsActionPressed("Crouch"))
        {
            //GD.Print("Crouch");
            if (!_is_crouch)
            {
                animPl.Play("Crouch");
                _is_crouch = true;
            }
            speed = CrouchSpeed;
        }
        else
        {
            if (_is_crouch)
            {
                PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
                var result = spaceState.IntersectRay(new PhysicsRayQueryParameters3D()
                {
                    From = Position,
                    To = new Vector3(Position.X, Position.Y + 2, Position.Z),
                    Exclude = new Godot.Collections.Array<Rid> { this.GetRid() } //getting unique ID of an object in scene
                });
                GD.Print($"result {result.Values}\n{this.GetRid()}");
                if (result.Count <= 0)
                {
                    animPl.Play("UnCrouch");
                    _is_crouch = false;
                    speed = CurrentSpeed;
                }
            }
        }
    }

    //FlashLight on/off method
    void FlashLightTurnOnOff()
    {
        if (Input.IsActionJustPressed("Flashlight"))
        {
            if (!flashlight_IsOn)
            {
                flash_light.LightEnergy = 3;
                GD.Print("Flashlight turned on");
                //richtext.AddText("\nFlashlight turned on");
                richtext.Text += "\nFlashlight turned on";
                flashlight_IsOn = true;
            }
            else
            {
                flash_light.LightEnergy = 0;
                GD.Print("Flashlight turned off");
                //richtext.AddText("\nFlashlight turned off");
                richtext.Text += "\nFlashlight turned off";
                flashlight_IsOn = false;
            }
        }
    }

    #region Signals
    public void _on_area_signalizer_body_entered(Node3D node)
    {
        //var layer5 = GetTree().GetNodesInGroup("Ice");
        //var layer6 = GetTree().GetNodesInGroup("Ground");
        
        var nodeGroup = node.GetGroups().FirstOrDefault();
        string[] AreasGropups = { "GroundArea", "IceArea", "ObstaclesArea", "MudArea", "PoolWaterArea" };



 
        switch (nodeGroup)
        {
            case "IceArea":
                GD.PrintErr("is in Ice area");
                richtext.AddText("\nin Ice area");
                break;
            case "GroundArea":
                GD.PrintErr("is in Ground area");
                richtext.AddText("\nis in Ground area");
                break;
            case "ObstaclesAreas":
                GD.PrintErr("is in Obstacle area");
                richtext.AddText("\nis in Obstacle area");

                var viewPort = GetViewport();
                if (viewPort != null)
                {
                    GD.Print($"viewPort not null {viewPort.Name}");
                }
                var mainScene = viewPort.GetNode<MainScene>("MainScene2");
                if (mainScene != null)
                {
                    GD.Print($"dfsdfsdf {mainScene.Name}");
                }
                else { GD.Print("Ni huya!"); }

                //var obstacle = mainScene.GetNode<Obstacle>("Obstacle");
                //if (obstacle != null)
                //{
                //    GD.Print("obstacle is found");
                    
                //}
                
                //Transform.Translated()
                break;
            case "MudArea":
                GD.PrintErr("is in Mud area");
                richtext.AddText("\nis in Mud area");
                
                break;
            case "PoolWaterArea":
                GD.PrintErr("is in PoolWater area");
                richtext.AddText("\nis in PoolWater areaPL");
                break;
        }

        GD.Print("Signal, woo-hoo!");
    }
    #endregion
}



