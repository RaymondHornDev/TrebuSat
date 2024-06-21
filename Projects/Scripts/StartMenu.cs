using Godot;
using System;

public partial class StartMenu : Control
{
	// Reference to the startMenu
	public static PanelContainer startMenu;

 	// Reference to the Info Menu
	public static PanelContainer infoMenu;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startMenu = GetNode<PanelContainer>("CanvasLayer/SMPanelContainer");
		infoMenu = GetNode<PanelContainer>("CanvasLayer/IMPanelContainer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Signal triggered when the start button is pressed
	public static void OnButtonPressed()
	{
		/* Start menu button */
		if(GameManager.Instance.PlayerBase != null)
		{
			GD.Print("Button pressed");
			startMenu.Hide();
			PackedScene scene = GD.Load<PackedScene>("res://Levels/launch_level.tscn");
			GameManager.Instance.ChangeScene(scene);
			//infoMenu.Show();
		}
	}

	public static void OnInfoButtonPressed()
	{
		GD.Print("Info menu button pressed");
		infoMenu.Hide();
		startMenu.Show();
	}
}
