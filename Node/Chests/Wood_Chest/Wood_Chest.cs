using Godot;
using Godot.Collections;
using System;

public class Wood_Chest : Area2D
{
    private SingInventory SI;
    private Singletone GS;

    private NFunc nf = new NFunc();

    private AnimatedSprite animatedSprite;
    
    /*
    private Dictionary<String, PackedScene> Items = new Dictionary<String, PackedScene>(){
                ["BigSword"]        = (PackedScene)ResourceLoader.Load("res://Node/Chests/Wood_Chest/Wood_Chest.tscn"),
                ["GatzSword"]       = (PackedScene)ResourceLoader.Load("res://Node/Enemy/Skeleton/Skeleton_lvl1.tscn"),
                ["SpeedBoots"]      = (PackedScene)ResourceLoader.Load("res://Node/Items/DebugItem.tscn"),
                ["LeatherArmor"]    = ,
                ["Spear"]           = 
            };
    */

    public override void _Ready()
    {
        SI = GetNode<SingInventory>("/root/SingInventory");
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        animatedSprite = GetNode<AnimatedSprite>("./AnimatedSprite");
    }
}
