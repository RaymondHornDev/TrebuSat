using Godot;
using System;

public partial class GameManager : Node
{
	#Static instance of GameManager that's universally accessable
	public static GameManager Instance;

 	#Reference to the ScenBase node
	public Node SceneBase;

 	#Reference to the PlayerBase Node
	public Node PlayerBase;

 	#Refernce to the MenuBase node
	public Node MenuBase;

 	#reference to the Orbital Transfer Vehicle current in the Tree
	public StaticBody3D otv;

 	#Reference to the Lunar Transfer Vehicle currently in the Tree
	public StaticBody3D ltv;

 	#Reference to the Particles3D used a rocket thrust currently in the Tree
	public GpuParticles3D RocketThrusts;

 	#Reference to the OTV crew capsule currently in the Tree
	public Node3D capsule;

 	#Reference to the current active scene
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
