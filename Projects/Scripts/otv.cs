using Godot;
using System;

public partial class otv : StaticBody3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Instance.otv = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
