using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class sol_body_orbit : Node3D
{

	public Node3D otvNode;
	public Node3D ltvNode;
	private List<Marker> markers;
	private Marker launchPoint;
	private int currentMarker = 0;
	private int markerLimit ;
	private int flightBrake = 30;
	private Vector3 moveChunk;
	private Vector3 currentTarget;
	private bool hasFlightPath = false;
	private bool hasLauncherArmPath = false;
	private bool isFlying = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		markers = GetTree().GetNodesInGroup("EntryMarkers").Select(saar => saar as Marker).ToList();
		launchPoint = GetTree().GetNodesInGroup("LaunchPoint").Select(saar => saar as Marker).ToList()[0];
		markerLimit = markers.Count;
		GameManager.Instance.trebu_Sat.isRotating = false;
		SpawnOTV();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(isFlying)
		{
			if(hasFlightPath == true)
			{
				OTVFlight();
			}
			else
			{
				OTVFlightPath();
			}
		}
		if(hasLauncherArmPath == true)
		{
			LauncherArmFlight();
		}
	}

	private void SpawnOTV()
	{
		PackedScene otv = ResourceLoader.Load("res://Scenes/otv.tscn") as PackedScene;
		otvNode = (Node3D)otv.Instantiate();
		this.AddChild(otvNode);
		otvNode.GlobalPosition = markers[currentMarker].GlobalPosition;
		otvNode.GlobalRotation = markers[currentMarker].GlobalRotation;
		GameManager.Instance.RocketThrusts.Emitting = true;
		currentMarker++;
	}

	private void SpawnLTV()
	{
		PackedScene ltv = ResourceLoader.Load("res://Scenes/ltv.tscn") as PackedScene;
		ltvNode = (Node3D)ltv.Instantiate();
		GameManager.Instance.trebu_Sat.springArm.AddChild(ltvNode);
		ltvNode.GlobalRotation -= new Vector3(0, 90, 0);
	}

	private void OTVFlightPath()
	{
		//GD.Print("Flight path called");
		currentTarget = markers[currentMarker].GlobalPosition;
		Vector3 currentPosition = otvNode.GlobalPosition;
		Vector3 direction = currentTarget - currentPosition;

		if(currentMarker < 2)
		{
			flightBrake = 30;
		}
		else if(currentMarker == 2)
		{
			flightBrake = 45;
		}
		else{
			flightBrake = 60;
		}


        moveChunk = new Vector3(direction.X / flightBrake, 0, direction.Z / flightBrake);
		hasFlightPath = true;
	}

	private void OTVFlight()
	{
		if(currentTarget - otvNode.GlobalPosition < moveChunk)
		{
			//GD.Print("Moving OTV");
			otvNode.GlobalPosition += moveChunk;
		}
		else if(currentMarker < markerLimit -1)
		{
			//GD.Print("Resetting flight path");
			currentMarker++;
			hasFlightPath = false;
		}
		else
		{
			//GD.Print("Flight finished");
			GameManager.Instance.RocketThrusts.Emitting = false;
			isFlying = false;
			MoveToLauncherArmPath();
		}
	}

	private void MoveToLauncherArmPath()
	{
		//Sets divisor for movement distance
		flightBrake = 550;

		//Stores OTV capsule global positon as a Vec3
		Vector3 currentPosition = GameManager.Instance.capsule.GlobalPosition;

		//Stores the difference between the target's global position and the OTV capsule's global position
		Vector3 direction = launchPoint.GlobalPosition - currentPosition;

		//Set move chunk distance by running the divisor through the difference vector
		moveChunk = new Vector3(direction.X / flightBrake, direction.Y / flightBrake, direction.Z / flightBrake);

		//Activates the LauncherArmFlight flow in process
		hasLauncherArmPath = true;
	}

	private void LauncherArmFlight()
	{
		if(launchPoint.GlobalPosition - GameManager.Instance.capsule.GlobalPosition < moveChunk)
		{
			GD.Print(launchPoint.GlobalPosition - GameManager.Instance.capsule.GlobalPosition > moveChunk);
			GameManager.Instance.capsule.GlobalPosition += moveChunk;
		}
		else
		{
			//Removes OTV capsule
			GameManager.Instance.capsule.QueueFree();

			//Spawns LTV
			SpawnLTV();
			GD.Print("Capsule in position");

			GameManager.Instance.movementCam.Current = false;
			GameManager.Instance.movementCam.Hide();

			//Activates camera

			Camera3D cam = this.GetNode<Camera3D>("Camera3D");
			cam.Show();
			cam.Current = true;

			//Activates trebuSat rotation
			GameManager.Instance.trebu_Sat.isRotating = true;
			GD.Print(GameManager.Instance.trebu_Sat.isRotating);

			//Decativates OTV capsule flight path to springarm
			hasLauncherArmPath = false;
		}
	}
}
