using Godot;
using System;


public interface ICollectUser
{
	public void Collect();
}
public partial class PlayerCS : CharacterBody3D, ICollectUser
{
	[ExportSubgroup("Components")]
	[Export] public Node3D View;

	[ExportSubgroup("Properties")]
	[Export] public float MoveSpeed = 200f;
	[Export] public float JumpStr = 6f;
	[Export] public int MaxJumps = 2;

	private Vector3 _movementVelocity;
	private float _direction;
	private float _gravity;

	private int _currentJumps;
	private bool _previouslyFloored = false;
	private int _coins = 0;

	private Node3D _visual;
	private AnimationPlayer _animator;
	private AudioStreamPlayer _audioSource;
	private CpuParticles3D _trail;
	
	private const string ANIM_WALK = "walk";
	private const string ANIM_IDLE = "idle";
	private const string ANIM_JUMP = "jump";

	public override void _Ready()
	{
		base._Ready();
		_currentJumps = MaxJumps;
		
		_visual = GetNode<Node3D>("Character");
		_animator = GetNode<AnimationPlayer>("Character/AnimationPlayer");
		_trail = GetNode<CpuParticles3D>("ParticlesTrail");
		_audioSource = GetNode<AudioStreamPlayer>("SoundFootsteps");
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		HandleInput(deltaF);
		HandleGravity(deltaF);
		HandleAnimations();
		
		 // Apply Movement
		 Vector3 appliedVelocity = Vector3.Zero;
		 appliedVelocity = Velocity.Lerp(_movementVelocity, deltaF * 10f);
		 appliedVelocity.Y = -_gravity;

		 Velocity = appliedVelocity;

		 MoveAndSlide();
		 
		 // apply rotation

		 if (Velocity.Length() > 0)
		 {
			 _direction = new Vector2(Velocity.Z, Velocity.X).Angle();
		 }

		 Vector3 rotation = Rotation;
		 rotation.Y = Mathf.LerpAngle(Rotation.Y, _direction, deltaF * 10f);
		 Rotation = rotation;

		 if (Position.Y < -10)
			 GetTree().ReloadCurrentScene();
		 
		 // apply Scale
		 _visual.Scale = _visual.Scale.Lerp(Vector3.One, deltaF * 10f);
		 
		 // Aniation landing
		 if (IsOnFloor() && _gravity > 2f && _previouslyFloored == false)
		 {
			 _visual.Scale = new Vector3(1.25f, 0.75f, 1.25f);
		 }

		 _previouslyFloored = IsOnFloor();
	}

	private void HandleInput(float delta)
	{
		// Movement
		var input = Vector3.Zero;
		input.X = Input.GetAxis(GameConstants.INP_AXIS_MOVE_LEFT, GameConstants.INP_AXIS_MOVE_RIGHT);
		input.Z = Input.GetAxis(GameConstants.INP_AXIS_MOVE_FWRD, GameConstants.INP_AXIS_MOVE_BACK);

		//input = input.Rotated(Vector3.Up, View.Rotation.Y).Normalized();
		
		_movementVelocity = View.Basis * input * MoveSpeed * delta;
		
		// Jumping
		if (Input.IsActionJustPressed(GameConstants.INP_ACTION_JUMP))
		{
			if (_currentJumps <= 0)
				return;

			AudioCS.Instance.Play(GameConstants.SFX_JUMP);

			Jump();
		}
	}

	private void HandleGravity(float delta)
	{
		_gravity += 25f * delta;
		if (_gravity > 0 && IsOnFloor())
		{
			_gravity = 0f;
			_currentJumps = MaxJumps;
		}
	}

	private void HandleAnimations()
	{
		_trail.Emitting = false;
		_audioSource.StreamPaused = true;

		if (IsOnFloor())
		{
			if (Mathf.Abs(Velocity.X) > 1 || Mathf.Abs(Velocity.Z) > 1)
			{
				_animator.Play(ANIM_WALK);
				_trail.Emitting = true;
				_audioSource.StreamPaused = false;
			}
			else
			{
				_animator.Play(ANIM_IDLE);
			}
		}
		else
		{
			_animator.Play(ANIM_JUMP);
		}
	}

	private void Jump()
	{
		_gravity = -JumpStr;
		_visual.Scale = new Vector3(0.5f, 1.5f, 0.5f);
		_currentJumps -= 1;
	}
	
	public void Collect()
	{
		_coins += 1;
		
		GameEvents.OnCoinCollected.Invoke(_coins);
	}
}
