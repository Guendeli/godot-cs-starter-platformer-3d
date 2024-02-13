using Godot;
using System;

public partial class CoinCS : Area3D
{
	private double _time;
	private bool _grabbed;

	private MeshInstance3D _mesh;
	private CpuParticles3D _particleEmitter;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_mesh = GetNode<MeshInstance3D>("Mesh");
		_particleEmitter = GetNode<CpuParticles3D>("Particles");
	}

	public void OnBodyEntered(Node3D body)
	{
		
		if (!_grabbed && body is ICollectUser collectorUser)
		{
			// body.CollectCoin
			collectorUser.Collect();
			AudioCS.Instance.Play(GameConstants.SFX_COIN);

			_mesh.QueueFree();
			_particleEmitter.Emitting = false;

			_grabbed = true;
		}
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		RotateY(2 * (float)delta);
		
		Vector3 pos = Position;
		pos.Y += (float)(Mathf.Cos(_time * 5) * 1 * delta);
		Position = pos;
		_time += delta;
	}
}
