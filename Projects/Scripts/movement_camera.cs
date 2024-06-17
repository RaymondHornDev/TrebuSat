using Godot;
using System;

public partial class movement_camera : CharacterBody3D
{

	// Creates a Vec3 at (0, 0, 0)
	public Vector3 targetLocation = new Vector3(0, 0, 0);
	public Camera3D cam;

	[Export]
	public float Speed = 5.0f;
	public bool tracking = false;
	public bool canMove = false;
	//public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	//public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	public override void _Ready()
	{
		cam = GetNode<Camera3D>("Camera3D");
		GameManager.Instance.movementCam = cam;
		tracking = true;	
	}

	public override void _PhysicsProcess(double delta)
	{
		if(tracking == true)
		{
			FocusOnObject();
		}
		if(canMove == true)
		{
			Movement();
		}
	}

	private void Movement()
	{
		Vector3 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void FocusOnObject()
	{
		targetLocation = TargetFinder(GameManager.Instance.otv);
		this.LookAt(targetLocation, Vector3.Up);
	}

	private Vector3 TargetFinder(StaticBody3D passedTarget)
	{
		Vector3 returnVec = new Vector3(0, 0, 0);
		returnVec = passedTarget.GlobalPosition;
		return returnVec;
	}
}
