using Godot;
using System;

public class Item : Area2D
{
    [Export]
    public Texture ItemTexture;

    [Export]
    public int ItemCount = 0;   

    [Export(PropertyHint.EnumSuggestion, "Item,Helmet,Weapon,Boots,Chestplate,Gem,Artefact")]
    public String ItemType = "Item";

    [Export]
    public String ItemName;

    [Export]
    public String ItemDescription = "";

    [Export]
    public int maxItemStack;
}
