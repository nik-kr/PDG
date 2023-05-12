using Godot;
using System;

public class BoostHP : Area2D
{
    private Singletone GS;
    [Export]
    public float HPBoost = 5;
    private AnimationPlayer ap;

    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        ap = GetNode<AnimationPlayer>("AnimationPlayer");
        ap.Play("Default");
        HPBoost = HPBoost + HPBoost * GS.level/2;
    }

    public void keeped(){
        if (GS.HealthPoint + HPBoost > GS.MaxHealthPoint){
            GS.HealthPoint = GS.MaxHealthPoint;
        }else{
            GS.HealthPoint += HPBoost;
        }
        this.QueueFree();
    }

    public void _on_BoostHP_body_entered(Node body){
        if(body.IsInGroup("player")){
            keeped();
        }
    }
}
