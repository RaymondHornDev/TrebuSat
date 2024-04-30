using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;

	public Node SceneBase;
	public Node PlayerBase;
	public Node MenuBase;
	public StaticBody3D otv;
	public StaticBody3D ltv;
	public GpuParticles3D RocketThrusts;
	public Node3D capsule;

	public Node3D currentScene;
	public trebu_sat trebu_Sat;

	public Camera3D movementCam;

    public override void _Ready()
    {
        base._Ready();
		if(Instance == null)
		{
			Instance = this;
			GD.Print("GameManager.Instance assigned");
		}
		else
		{
			QueueFree();
		}
    }

	public void ChangeScene(PackedScene passeScene)
	{
		if(SceneBase != null)
		{
			foreach(var item in Instance.SceneBase.GetChildren())
			{
				item.QueueFree();
			}
		}
		SceneBase.AddChild(passeScene.Instantiate());
	}
}
