using Godot;
using System;

public partial class HudCS : CanvasLayer
{

	private Label _coinsText;
	public override void _Ready()
	{
		base._Ready();
		_coinsText = GetNode<Label>("Coins");
		GameEvents.OnCoinCollected?.AddListener(OnCoinCollected); // <---- SIGNALS
		
	}

	public void OnCoinCollected(int coins)
	{
		// TODO - Figure out this crash after ReloadCurrentScene() is called
		if(_coinsText != null && IsInstanceValid(_coinsText))
			_coinsText.Text = coins.ToString();
	}

	public override void _ExitTree()
	{
		GameEvents.OnCoinCollected.RemoveListener(OnCoinCollected);
	}
}
