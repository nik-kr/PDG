using Godot;
using System;

public class Warrior : KinematicBody2D
{
    [Export]
    public String NodeType = "Player";
    [Export]
    public float speed = 200;
    public Vector2 velocity = new Vector2();
    public float HealthPoint = 5;

    [Signal]
    delegate void giveDamageSignal(float damage);

    public Control DebugMenu;

    private PackedScene footprint = (PackedScene)GD.Load("res://Node/Character/Footprint.tscn");

    public override void _Ready()
    {
        Connect("giveDamageSignal", this, "giveDamageCallback");
        foreach(Control c in GetTree().GetNodesInGroup("DebugMenu")){
            DebugMenu = c;
        }
    }
    public void giveDamageCallback(float damage){
        HealthPoint -= damage;
        if (HealthPoint <= 0){
            QueueFree();
        }
    }
    public void GetInput()
    {
        velocity = new Vector2();

        if (Input.IsActionPressed("ui_right"))
            velocity.x += 1;

        if (Input.IsActionPressed("ui_left"))
            velocity.x -= 1;

        if (Input.IsActionPressed("ui_down"))
            velocity.y += 1;

        if (Input.IsActionPressed("ui_up"))
            velocity.y -= 1;
        if (Input.IsActionPressed("ui_attack"))
        {
            this.GetNode<Node2D>("Weapon").LookAt(GetGlobalMousePosition());
            this.GetNode<Weapon>("Weapon/Weapon").Attack();
        }

        velocity = velocity.Normalized() * speed;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        velocity = MoveAndSlide(velocity);
        DebugMenu.GetNode<Label>("PlayerPosition").Text = (
            "PLAYER POSITION\nX: " + GlobalPosition.x.ToString() +
            "\nY: " + GlobalPosition.y.ToString());
    }
    public void _on_footstepTimer_timeout(){
        Footprint f = (Footprint)footprint.Instance();
        f.Position = Position;
        GetParent().AddChild(f);
    }
}
