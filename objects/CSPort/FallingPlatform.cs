using Godot;
using System;

public partial class FallingPlatform : Node3D
{
	private Area3D _area3D;

	private bool _falling = false;
	private float _gravity = 0.0f;
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		
		_area3D = GetNode<Area3D>("Area3D");
		_area3D.BodyEntered += OnByEntered;
	}

	private void OnByEntered(Node3D body)
	{
		if (!_falling)
		{
			AudioCS.Instance.Play(GameConstants.SFX_FALL);
			Scale = new Vector3(1.25f, 1, 1.25f);
		}

		_falling = true;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		Vector3 pos = Position;
		Scale = Scale.Lerp(Vector3.One, (float)delta * 10);
		pos.Y -= _gravity * (float)delta;
		
		if(Position.Y < -10) QueueFree();

		if (_falling)
		{
			_gravity += 0.25f;
		}

		Position = pos;
	}
	
	
}
