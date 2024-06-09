using Godot;
using System;

public partial class trebu_sat : Node3D
{

	// Rotation speed kicked to the inspector
	[Export]
	public int rotationSpeed = 2;

	// Boolean to turn rotation on and off
	public bool isRotating = true;

	// Rotating arm
	private Node3D launcher;

 	// Circular rotating part
	private Node3D counterWeights;
	public SpringArm3D springArm;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		launcher = GetNode<Node3D>("Launcher");
		counterWeights = GetNode<Node3D>("CounterWeights");
		springArm = GetNode<SpringArm3D>("Launcher/SpringArm3D");
		GameManager.Instance.trebu_Sat = this;
		if(GameManager.Instance.trebu_Sat != null)
		{
			GD.Print("Trebu-sat reference on Gamemanager");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(isRotating == true)
		{
			RotatingParts(delta);
		}
	}

	private void RotatingParts(double delta)
	{
		launcher.RotateY(rotationSpeed * (float)delta);
		counterWeights.RotateY(-rotationSpeed * (float)delta);
	}
}
