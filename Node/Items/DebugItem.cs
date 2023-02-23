using Godot;
using System;

public class DebugItem : Item
{

    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        maxItemStack = 99;
        ItemCount = (int)GD.RandRange(1, maxItemStack);
    }
}
