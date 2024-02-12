using System;
using Godot;

public static class GameEvents
{
    public static GodotEvent<int> OnCoinCollected = new GodotEvent<int>();
}