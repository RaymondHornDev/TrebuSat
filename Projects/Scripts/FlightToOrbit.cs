using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class FlightToOrbit : Node3D
{
	// Boolean value indicating the start of the process method
	private bool levelStarting = true;

 	// Boolean value indicating the possesion of a flight path
	private bool hasPath = false;

	// Reference to the OTV spawn point
	private Marker3D spawnPoint;

 	// Reference to a reloaded OTV scene
	private PackedScene OTV;

 	// Reference to the OTV object active in the Tree
	private Node3D otv;

 	// Reference to the RayCast Object
	private RayCast3D ray;

 	// Integer value of the currently targeted flight marker
	private int targetIndex = 0;
	private int targetIndexLimit = 3;
	private Vector3 targetPos;
	
	private List<Marker3D> markerGroup;
	//private List<Vector3> moveChunks = new List<Vector3>();
	private Vector3 moveChunk;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnPoint = GetNode<Marker3D>("LaunchPath/SpawnPoint");
		ray = GetNode<RayCast3D>("LaunchPath/Marker5/RayCast3D");
		OTV = ResourceLoader.Load<PackedScene>("res://Scenes/otv.tscn");
		markerGroup = GetTree().GetNodesInGroup("LaunchMarkers").Select(saar => saar as Marker3D).ToList();
		//GD.Print(markerGroup.Count);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RayCollisionTrigger();

		if(levelStarting == true)
		{
			SpawnOTV();
			levelStarting = false;
		}
		
		if(hasPath == true)
		{
			MoveOTV();
		}
	}

	private void SpawnOTV()
	{
		otv = OTV.Instantiate() as Node3D;
		this.AddChild(otv);
		otv.GlobalPosition = spawnPoint.GlobalPosition;
		OTVFlight();
	}

	private void OTVFlight()
	{
		Vector3 selfPos = otv.GlobalPosition;
		//GD.Print(targetIndex);

		targetPos = markerGroup[targetIndex].GlobalPosition;

		Vector3 direction = targetPos - selfPos;
		moveChunk = direction/30;

		hasPath = true;
	}

	private void MoveOTV()
	{
		Vector3 moveDistance = targetPos - otv.GlobalPosition;
		if(moveDistance > moveChunk)
		{
			otv.GlobalPosition += moveChunk;
			//GD.Print(targetPos);
			//GD.Print(otv.GlobalPosition);
		}
		else
		{
			if(targetIndex < targetIndexLimit)
			{
				targetIndex++;
				OTVFlight();
			}
			else
			{
				hasPath = false;
			}
		}
	}

	private void RayCollisionTrigger()
	{
		if(ray.IsColliding())
		{
			PackedScene scene = ResourceLoader.Load("res://Levels/sol_body_orbit.tscn") as PackedScene;
			GameManager.Instance.ChangeScene(scene);
		}
	}
}
