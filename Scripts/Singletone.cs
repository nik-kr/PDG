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

    //*Player Charateristic
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

    //*GameSettings
    public bool DebugMode = false;
    public ConfigFile config = new ConfigFile();    //Game Data
    public float masterVolume = 50;                 //Громкость
    public int difficult = 2;                       //Сложность игры
    public int gameRecord = 0;                      //Рекордный уровень подземелья

    //KeyBind
    public InputEventKey keyUiUp;
    public InputEventKey keyUiDown;
    public InputEventKey keyUiLeft;
    public InputEventKey keyUiRight;

    //?Debug
    public Texture debugItemTexture = (Texture)GD.Load("res://Sprite/GameResources/Debug/DebugItem.png");
    public PackedScene debugItem_p = (PackedScene)GD.Load("res://Node/Items/DebugItem.tscn");

    //** Save Config Settings and Function
    public string cfgPath = "./gameconfig.cfg";
    //This function saving Game Settings
    public void SaveCfg(){
        // config.SetValue("KeyBind", "ui_up", keyUiUp);
        // config.SetValue("KeyBind", "ui_down", keyUiDown);
        // config.SetValue("KeyBind", "ui_left", keyUiLeft);
        // config.SetValue("KeyBind", "ui_right", keyUiRight);
        config.SetValue("KeyBind", "ui_up", InputMap.GetActionList("ui_up")[0]);
        config.SetValue("KeyBind", "ui_down", InputMap.GetActionList("ui_down")[0]);
        config.SetValue("KeyBind", "ui_left", InputMap.GetActionList("ui_left")[0]);
        config.SetValue("KeyBind", "ui_right", InputMap.GetActionList("ui_right")[0]);

        config.SetValue("Sound", "volume", masterVolume);
        config.SetValue("GameSetting", "difficult", difficult);

        config.SetValue("Other", "gameRecord", gameRecord);

        config.Save(cfgPath);
    }
    //This function load Game Settings
    public void LoadCfg(){
        var error = config.Load(cfgPath);
        if(error != Error.Ok){
            GD.Print("Ошибка! Невозможно загрузить файл настроек!");
            SaveCfg();
        }else{
            keyUiUp      = (InputEventKey)config.GetValue("KeyBind", "ui_up");
            keyUiDown    = (InputEventKey)config.GetValue("KeyBind", "ui_down");
            keyUiLeft    = (InputEventKey)config.GetValue("KeyBind", "ui_left");
            keyUiRight   = (InputEventKey)config.GetValue("KeyBind", "ui_right");
            masterVolume = (float)config.GetValue("Sound", "volume", 50f);
            difficult    = (int)config.GetValue("GameSetting", "difficult", 2);
            gameRecord   = (int)config.GetValue("Other", "gameRecord");
            
            changeKey(keyUiUp, "ui_up");
            changeKey(keyUiDown, "ui_down");
            changeKey(keyUiLeft, "ui_left");
            changeKey(keyUiRight, "ui_right");
        }
    }
    

    public void changeKey(InputEvent _event, string changeKeyName){
        GD.Print(InputMap.GetActionList(changeKeyName));
        if(InputMap.GetActionList(changeKeyName).Count > 0){
            InputMap.ActionEraseEvent(changeKeyName, (InputEventKey)InputMap.GetActionList(changeKeyName)[0]);
        }
        if(InputMap.ActionHasEvent(changeKeyName, _event)){
            InputMap.ActionEraseEvent(changeKeyName, _event);
        }
        InputMap.ActionAddEvent(changeKeyName, _event);
        GD.Print(InputMap.GetActionList(changeKeyName));
    }

    //Any
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
