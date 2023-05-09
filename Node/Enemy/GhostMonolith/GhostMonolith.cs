using Godot;
using System;

public class GhostMonolith : Enemy
{
    [Export]
    public float summoningDeleay = 3;


    private AnimatedSprite animSprite;
    private bool beDestroyed = false;
    private Singletone GS;
    private Timer timer;
    private PackedScene pGhost = (PackedScene)ResourceLoader.Load("res://Node/Enemy/Ghost/Ghost.tscn");


    public override void _Ready()
    {
        timer = GetNode<Timer>("./Timer");
        timer.WaitTime = summoningDeleay;
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        MaxHealthPoint = 50;
        HealthPoint = 50;

        animSprite.Play("default");
        timer.Start();
    }

    public void _on_Timer_timeout(){
        animSprite.Play("resurrection");
    }

    public void _on_AnimatedSprite_animation_finished(){
        if(animSprite.Animation == "resurrection"){
            timer.Start();
            animSprite.Play("default");
            if(GS.ghostCount < GS.maxGhostCount){
                Ghost ghost = pGhost.Instance<Ghost>();
                ghost.Position = this.Position;
                this.AddChild(ghost);
            }
            GS.ghostCount += 1;
        }
    }

    public void Death(){
        beDestroyed = true;
    }

    public void TakeDamage(float damage){
        if(damage >= HealthPoint){
            HealthPoint -= HealthPoint;
            animPlayer.Play("death");
            Death();
        }
        HealthPoint -= damage;
    }
}
