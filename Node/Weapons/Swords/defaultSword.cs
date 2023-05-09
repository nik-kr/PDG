using Godot;
using System;

public class defaultSword : Weapon
{
    public const float defaultDamage = 3f;
    public float damage = defaultDamage;
    public override void _Ready()
    {
        AnimPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        attackAnim = "attack";
    }
    public void _on_defaultSword_body_entered(Node body){
        if (body.IsInGroup("enemy")){
            ((Enemy)body).TakeDamage(damage);
            //Отталкивание при ударе
            if(!body.IsInGroup("GhostMonolith")){
                ((Enemy)body).velocity += this.GlobalPosition.DirectionTo(((Enemy)body).GlobalPosition) * 140;
            }
            GD.Print( ((Enemy)body).HealthPoint );
        }
    }
}
