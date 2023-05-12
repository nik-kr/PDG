using Godot;
using System;

public class BoosLevel1 : TileMap
{
    private Singletone GS;
    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        GS.player = GetNode<Player>("Warrior");
    }
}
