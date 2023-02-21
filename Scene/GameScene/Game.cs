using Godot;

public class Game : Node
{
    private PackedScene pLevelNode = GD.Load<PackedScene>("res://Node/Level/Level.tscn");
    private Level levelScene = null;
    private int _level;
    private int level
    {
        get
        {
            return level;
        }
        set
        {
            _level = value;
            GS.level = _level;
        }
    }

    private Label FPS;
    private ProgressBar HPbar;
    private Inventory Inv;
    private Singletone GS;

    private bool isInvOpen = false;


    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");

        FPS = GetNode<Label>("GUI/Control/Debug/FPS");
        HPbar = GetNode<ProgressBar>("GUI/Control/Bar/HPBar");
        Inv = GetNode<Inventory>("GUI/Control/Inventory");

        level = GS.level;
        GS.HPBar = HPbar;
        GS.GUI = GetNode<CanvasLayer>("GUI");
        UpdateLevel();
    }

    private void GetInput(){
        //Close
        if(Input.IsActionJustPressed("ui_cancel")){
            if(isInvOpen){
                isInvOpen = false;
                Inv.CloseInventory();
            }
        }
        //Open Invenroty
        if(Input.IsActionJustPressed("ui_inv")){
            switch(isInvOpen){
                case false:
                    isInvOpen = true;
                    Inv.OpenInventory();
                    break;
                case true:
                    isInvOpen = false;
                    Inv.CloseInventory();
                    break;
            }
        }

    }

    public override void _PhysicsProcess(float delta)
    {
        FPS.Text = "FPS" + Engine.GetFramesPerSecond().ToString();
        HPbar.MaxValue = (float)GS.MaxHealthPoint;
        HPbar.Value = (float)GS.HealthPoint;

        GetInput();
    }

    public void UpdateLevel()
    {
        if (levelScene == null)
        {
            levelScene = pLevelNode.Instance<Level>();
        }
        else
        {
            levelScene.QueueFree();
            levelScene = pLevelNode.Instance<Level>();
            level += 1;
        }
        levelScene.LEVEL = GS.level;
        AddChild(levelScene);
    }

}
