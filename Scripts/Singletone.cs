using Godot;
using System;

public class Singletone : Node
{
    public ProgressBar HPBar;
    public Inventory Inv;
    public Level LevelNode;
    public int level = 1;
    
    public PackedScene _PacWarrior = (PackedScene)ResourceLoader.Load("res://Node/Character/Warrior/Warrior.tscn");
    public String Character;
    public Player player = new Player();

    public CanvasLayer GUI;

    public int TileSize = 16;

    //!Player Charateristic
    public float Strenght;
    public float MaxHealthPoint;
    public float HealthPoint;
    public float MaxMana;
    public float Manna;
    public float MaxStamina;
    public float Stamina;
    public float Lucky;
    public float Charisma;
    public float Intelect;
    public float Damage;
    public float Secrecy;

    //!GameSettings
    public bool DebugMode = false;

    //?Debug
    public Texture debugItemTexture = (Texture)GD.Load("res://Sprite/GameResources/Debug/DebugItem.png");
    public PackedScene debugItem_p = (PackedScene)GD.Load("res://Node/Items/DebugItem.tscn");

    public void ChangeCharacter(String character){
        Character = character;
        if (Character == "Warrior"){
            Strenght = 5;
            MaxHealthPoint = 10;
            HealthPoint = 10;
            MaxMana = 0;
            Manna = 0;
            MaxStamina = 5;
            Stamina = 5;
            Lucky = 1;
            Charisma = 2;
            Intelect = 1;
            Damage = 2;
            Secrecy = 0;
        }
    }

    public override void _Ready()
    {
        
    }
    public override void _PhysicsProcess(float delta)
    {

    }
    public bool takeDamage(float damage){
        if(damage > HealthPoint){
            HealthPoint -= HealthPoint;
        }else{
            HealthPoint -= damage;
        }
        return true;
    }
}
