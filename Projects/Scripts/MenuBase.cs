using Godot;
using System;

public partial class MenuBase : Node
{
	private Control startMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Instance.MenuBase = this;
		startMenu = GameManager.Instance.MenuBase.GetNode<Control>("StartMenu");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _ExitTree()
    {
        base._ExitTree();
		GameManager.Instance.MenuBase = null;
    }
}