using Godot;
using System;
using static System.Formats.Asn1.AsnWriter;

public partial class Menu : Control
{
    [Export] public Button PlayBttn;
    [Export] public Button ExitBttn;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsMouseButtonPressed(MouseButton.Left)) { }
	}

    public void OnStartBttnButtonDown()
    {
        //var simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/MainScene.tscn").Instantiate();
        //var currentScene = ResourceLoader.Load<PackedScene>("res://Scenes/MainMenu.tscn");
        //GetTree().Root.AddChild(simultaneousScene);
        GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
        //currentScene.Free();
    }

    public void _on_exit_bttn_button_up()
    {
        GetTree().Quit();
    }
}
