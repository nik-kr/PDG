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

    private bool doCollect = false;

    public PackedScene NodeScene;

    private Player player;
    public Singletone GS;

    public override void _PhysicsProcess(float delta){
        if(doCollect && player != null){
            GlobalPosition = GlobalPosition.MoveToward(player.GlobalPosition, 150 * delta);
            if(Scale.x >= 0.5){
                Scale -= new Vector2(delta, delta);
            }
        }
    }

    public void Collect(Player p){
        if(!GS.Inv.FullInventory){
            player = p;
            doCollect = true;
        }
    }
}
