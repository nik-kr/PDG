using Godot;

public class Game : Node
{
    private PackedScene pLevelNode = GD.Load<PackedScene>("res://Node/Level/Level.tscn");
    private PackedScene MainMenu = GD.Load<PackedScene>("res://Scene/MainMenu/MainMenu.tscn");
    private Node levelScene = null;
    private int level;

    private Label FPS;
    private ProgressBar HPbar;
    private Inventory Inv;
    private Singletone GS;

    private bool isInvOpen = false;

    [Export]
    public AudioStreamSample ALVLUp;

    public void _on_AudioStreamPlayer_finished(){
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }

    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        FPS = GetNode<Label>("GUI/Control/Debug/FPS");
        HPbar = GetNode<ProgressBar>("GUI/Control/Bar/HPBar");
        GS.task = GetNode<RichTextLabel>("GUI/Control/task");
        Inv = GetNode<Inventory>("GUI/Control/Inventory");

        level = GS.level;

        GS.game = this;
        GS.Inv = Inv;
        GS.HPBar = HPbar;
        GS.GUI = GetNode<CanvasLayer>("GUI");
        GS.PauseScreen = GetNode<Control>("GUI/PauseScreen");
        GS.DeathScreen = GetNode<Control>("GUI/DeathScreen");
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
        if(Input.IsActionJustPressed("ui_cancel")){
            if(!GS.endGame){
                if(GS.PauseScreen.Visible == false){
                    GS.pauseMode = true;
                    var newPauseState = !levelScene.GetTree().Paused;
                    levelScene.GetTree().Paused = newPauseState;
                    GS.PauseScreen.Visible = true;
                }else{
                    GS.pauseMode = false;
                    var newPauseState = !levelScene.GetTree().Paused;
                    levelScene.GetTree().Paused = newPauseState;
                    GS.PauseScreen.Visible = false;
                }
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
        if(GS.gameRecord < GS.level){
            GS.gameRecord = level;
            GS.SaveCfg();
        }
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Load");
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
        if(level % 5 != 0){
            AddChild(levelScene);
            GS.task.Visible = false;
        }else{
            PackedScene pBoosRoom = (PackedScene)ResourceLoader.Load("res://Node/Level/BoosLevel1.tscn");
            levelScene = pBoosRoom.Instance<Node>();
            AddChild(levelScene);
            GS.task.Visible = true;
        }
        GS.ChangePauseMode(levelScene.GetChildren(), PauseModeEnum.Inherit);
        GS.LevelNode = levelScene;
        GetNode<Label>("GUI/Control/Label").Text = "LEVEL: " + GS.level.ToString();
    }
    public void rmLevel(){
        levelScene.QueueFree();
        GS.ChangeCharacter("Warrior");
        GS.HealthPoint = GS.MaxHealthPoint;
        GS.level = 1;
        GS.DeathScreen.Visible = false;
        UpdateLevel();
        GS.pauseMode = false;
        var newPauseState = !levelScene.GetTree().Paused;
        levelScene.GetTree().Paused = newPauseState;
    }


    public void _on_NewGame_pressed(){
        rmLevel();
        GS.pauseMode = false;
        var newPauseState = !levelScene.GetTree().Paused;
        levelScene.GetTree().Paused = newPauseState;
        GS.PauseScreen.Visible = false;
    }

    public void _on_MainMenu_pressed(){
        GetTree().ChangeSceneTo(MainMenu);
        GS.pauseMode = false;
        var newPauseState = !levelScene.GetTree().Paused;
        levelScene.GetTree().Paused = newPauseState;
        GS.PauseScreen.Visible = false;
    }

    public void _on_Unpaused_pressed(){
        GS.pauseMode = false;
        var newPauseState = !levelScene.GetTree().Paused;
        levelScene.GetTree().Paused = newPauseState;
        GS.PauseScreen.Visible = false;
    }

}
