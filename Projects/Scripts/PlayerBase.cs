using Godot;
using System;

public partial class PlayerBase : Node
{
	private Node gameManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		if(GameManager.Instance.PlayerBase == null)
		{
			GD.Print("GameManager.PlayerBase found");
			GameManager.Instance.PlayerBase = this;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _ExitTree()
    {
        base._ExitTree();
		GameManager.Instance.PlayerBase = null;
    }
}
