using Godot;
using System;

public class GhostMonolith : Enemy
{
    [Export]
    public float summoningDeleay = 3;

    private bool beDestroyed = false;
    private Singletone GS;
    private Timer timer;
    private PackedScene pGhost = (PackedScene)ResourceLoader.Load("res://Node/Enemy/Ghost/Ghost.tscn");


    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        timer = GetNode<Timer>("./Timer");
        timer.WaitTime = summoningDeleay;
        animSprite = GetNode<AnimatedSprite>("./AnimatedSprite");
        aPlayerORaSprite = false;
        deathSpriteAnim = "destroy";
        killed = false;
        MaxHealthPoint = 50;
        HealthPoint = 50;

        animSprite.Play("default");
        timer.Start();
    }

    public void _on_Timer_timeout(){
        if(!enemyKilled){
            animSprite.Play("resurrection");
        }else{
            timer.Stop();
        }
    }

    public void _on_AnimatedSprite_animation_finished(){
        if(animSprite.Animation == "resurrection"){
            timer.Start();
            animSprite.Play("default");
            if(GS.ghostCount < GS.maxGhostCount && GS.pauseMode == false){
                Ghost ghost = pGhost.Instance<Ghost>();
                ghost.Position = this.Position;
                AddChild(ghost);
            }
            GS.ghostCount += 1;
        }
    }

    // public override void TakeDamage(float damage){
    //     base.TakeDamage(damage);
    //     if(damage >= HealthPoint){
    //         HealthPoint -= HealthPoint;
    //         animSprite.Play("destroy");
    //         beDestroyed = true;
    //     }
    //     HealthPoint -= damage;
    // }
}
