using Godot;
using System;

public class GhostMonolith : Enemy
{

    private AnimatedSprite animationSprite;
    private string deathAnimName;


    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("./AnimationPlayer");
        animationSprite = GetNode<AnimatedSprite>("./AnimatedSprite");
        deathAnimName = "death";
        MaxHealthPoint = 50;
        HealthPoint = 50;

        animPlayer.Play("death");
    }

    public void Death(AnimationPlayer aPlayer = null, String aName = null){
        
    }
}
