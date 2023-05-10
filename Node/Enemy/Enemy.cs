using Godot;
using System;

public class Enemy : KinematicBody2D
{
    public float HealthPoint;
    public float MaxHealthPoint;

    public AnimationPlayer animPlayer = null;
    public String deathAnimName = "death";
    public bool killed = true;
    public bool enemyKilled = false;
    public bool aPlayerORaSprite = true;
    public Vector2 velocity = Vector2.Zero;
    public AnimatedSprite animSprite = null;
    public String deathSpriteAnim = null;
    
    public virtual async void Death(AnimationPlayer aPlayer = null, String aName = null){
        if(aPlayer != null && aName != null && aPlayerORaSprite == true){
            await ToSignal(aPlayer, "animation_finished");
        }
        if(killed){
            QueueFree();
        }else{
            animSprite.Play(deathSpriteAnim);
            enemyKilled = true;
        }
    }

    public virtual void TakeDamage(float damage){
        if(damage >= HealthPoint){
            HealthPoint -= HealthPoint;
            if(animPlayer != null){
                animPlayer.Play("death");
            }
            Death(animPlayer, deathAnimName);
        }
        HealthPoint -= damage;
        GD.Print("Destroyed!");
    }
}
