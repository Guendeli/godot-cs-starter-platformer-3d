using Godot;
using System;

public partial class CloudCS : Node3D
{
	private RandomNumberGenerator _rnd;
	private double _time = 0.0f;
	private float _randomVelocity;
	private float _randomTime;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_rnd = new RandomNumberGenerator();
		_randomVelocity = _rnd.RandfRange(0.1f, 2.0f);
		_randomTime = _rnd.RandfRange(0.1f, 2.0f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		Vector3 pos = Position;
		pos.Y += (float)(Mathf.Cos(_time * _randomTime) * _randomVelocity * delta);
		Position = pos;
		_time += delta;
	}
}
