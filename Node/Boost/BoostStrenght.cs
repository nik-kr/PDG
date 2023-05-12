using Godot;
using System;

public class BoostStrenght : Area2D
{
    private Singletone GS;
    private float StrenghtBoost = 1.5f;
    private AnimationPlayer ap;

    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        ap = GetNode<AnimationPlayer>("AnimationPlayer");
        ap.Play("Default");
        
        StrenghtBoost = StrenghtBoost + GS.level/5;
    }

    public void keeped(){
        GS.Strenght += StrenghtBoost;
        this.QueueFree();
    }
    
    public void _on_BoostStrenght_body_entered(Node body){
        if(body.IsInGroup("player")){
            keeped();
        }
    }
}
