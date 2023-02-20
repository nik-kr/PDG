using Godot;
using System;

public class Item : Area2D
{
    [Export]
    public Texture ItemTexture;

    [Export]
    public int ItemCount = 0;   

    [Export]
    public String ItemType;

    [Export]
    public String ItemName;

    [Export]
    public String ItemDescription = "";
    
    [Export]
    public int maxItemStack;
}
