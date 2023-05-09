using Godot;
using System;

public class Enemy : KinematicBody2D
{
    public float HealthPoint;
    public float MaxHealthPoint;

    public AnimationPlayer animPlayer = null;
    public String deathAnimName = null;
    public Vector2 velocity = Vector2.Zero;
    public AnimatedSprite spriteAnim = null;
    public String deathSpriteAnim = null;
    
    public async void Death(AnimationPlayer aPlayer = null, String aName = null){
        if(aPlayer != null && aName != null){
            await ToSignal(aPlayer, "animation_finished");
        }
        QueueFree();
    }

    public void TakeDamage(float damage){
        if(damage >= HealthPoint){
            HealthPoint -= HealthPoint;
            if(animPlayer != null){
                animPlayer.Play("death");
            }
            Death(animPlayer, deathAnimName);
        }
        HealthPoint -= damage;
        
    }
}
