using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class launch_level : Node3D
{
	/* I'm the greatest */
 	/* I'm the greatest */
  	/* I'm the greatest */
 	
	private bool launching = false;
	private bool starting = true;
	private List<float> moveChunks = new List<float>();

	private int targetIndex = 0;
	private int targetIndexLimit = 8;
	private List<Marker3D> markerGroup;
	private Node3D rocket;

	private RayCast3D ray;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rocket = GetNode<Node3D>("RocketBase");
		ray = GetNode<RayCast3D>("WayPoints/Marker9/RayCast3D");
		markerGroup = GetTree().GetNodesInGroup("FlightMarkers").Select(saar => saar as Marker3D).ToList();
		GameManager.Instance.RocketThrusts.Emitting = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RayCollision();
		if(launching == true)
		{
			MoveRocket();
		}
		if(starting == true)
		{
			LaunchRocket();
			starting = false;
		}
		
	}

    private void MoveRocket()
    {
        if(moveChunks.Count > 0)
		{
			//GD.Print(moveChunks.Count);
			float yVal = moveChunks[moveChunks.Count - 1];
			//GD.Print(yVal);
			moveChunks.RemoveAt(moveChunks.Count - 1);
			Vector3 newPos = new Vector3(0, yVal, 0);
			rocket.GlobalPosition += newPos;
		}
		else
		{
			//GD.Print(targetIndex < targetIndexLimit);
			if(targetIndex < targetIndexLimit)
			{
				targetIndex++;
				//GD.Print(targetIndex);
				LaunchRocket();
			}
			else
			{
				launching = false;
			}
		}
    }

    private void LaunchRocket()
	{
		Vector3 targetPos = markerGroup[targetIndex].GlobalPosition;
		Vector3 rocketPos = rocket.GlobalPosition;
		Vector3 totalDist = targetPos - rocketPos;
		Vector3 normDist = totalDist;
		float dictCut = normDist.Y/30;
		for(int x = 0; x < 30; x++)
		{
			moveChunks.Add(dictCut);
		}
		launching = true;
	}

	private void RayCollision()
	{
		if(ray.IsColliding())
		{
			//GD.Print(ray.GetCollider());
			PackedScene scene = ResourceLoader.Load<PackedScene>("res://Levels/flight_to_orbit.tscn");
			GameManager.Instance.ChangeScene(scene);
		}
	}
}
