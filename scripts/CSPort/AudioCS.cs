using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public partial class AudioCS : Node
{
	private int _numPlayers = 12;
	private static AudioCS _instance;
	
	public static AudioCS Instance => _instance;

	private Godot.Collections.Array<AudioStreamPlayer> _available;
	private Godot.Collections.Array<string> _queue;
	public override void _EnterTree()
	{
		base._EnterTree();
		_instance = this;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_available = new Array<AudioStreamPlayer>();
		_queue = new Array<string>();
		for (int i = 0; i < _numPlayers; i++)
		{
			AudioStreamPlayer p = new AudioStreamPlayer();
			AddChild(p);

			p.VolumeDb = -10;
			p.Bus = GameConstants.AUDIO_BUS;
			p.Finished += () =>
			{
				_available.Add(p);
			};

			
			_available.Add(p);

			
		}
	}

	public void Play(string path)
	{
		_queue.Add(path);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (_queue.Count > 0 && _available.Count > 0)
		{
			_available[0].Stream = ResourceLoader.Load<AudioStream>(_queue[0]);
			_available[0].Play();
			_available[0].PitchScale = (float)GD.RandRange(0.9f, 1.1f);
			_queue.RemoveAt(0);
			_available.RemoveAt(0);
		}
	}
}
