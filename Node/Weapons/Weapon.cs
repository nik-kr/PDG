using Godot;
using System;

public class Weapon : Area2D
{
    private AnimationPlayer animPlayer;
    public String attackAnim;

    public bool doAttack = true;

    public AnimationPlayer AnimPlayer{
        get
        {
            return this.animPlayer;
        }
        set
        {
            this.animPlayer = value;
            animPlayer.Connect("animation_finished", this, nameof(animStoped));
        }
    }

    public void Attack(){
        if(doAttack)
        {
            doAttack = false;
            animPlayer.Play(attackAnim);
        }
    }
    public void animStoped(String aName){
        doAttack = true;
    }
}
