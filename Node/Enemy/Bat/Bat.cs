using Godot;
using System;

public class Bat : Enemy
{
    [Export]
    public float speed = 120;
    [Export]
    public float detectionRadius = 50;
    [Export]
    public float hitTime = 2.5f;
    [Export]
    public float damage;


    private Footprint _Footprint; // Footprint
    private Timer HitTimer;
    private KinematicBody2D Player;
    private Singletone GS;
    private AnimatedSprite animationSprite;

    private Vector2 direction = Vector2.Zero;

    private bool toHit = true;


    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        animationSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        MaxHealthPoint = 1 + (1/10)*GS.level; HealthPoint = MaxHealthPoint;
        damage = 0.1f + (0.1f)*GS.level;
        aPlayerORaSprite = false;
        deathAnimName = "death";
        HitTimer = GetNode<Timer>("HitTimer");
        HitTimer.WaitTime = hitTime;
        foreach(KinematicBody2D p in GetTree().GetNodesInGroup("player")){
            Player = p;
        }
    }
    public override void _PhysicsProcess(float delta){
        if(_Footprint != null){
            direction = GlobalPosition.DirectionTo(_Footprint.GlobalPosition);
            Vector2 desiredVelocity = direction * speed;
            Vector2 steering = (desiredVelocity - velocity) * delta * 4f;
            velocity += steering;
            velocity = MoveAndSlide(velocity);
            float deg2fp = Godot.Mathf.Rad2Deg(GlobalPosition.AngleToPoint(_Footprint.GlobalPosition));
            //LEFTUP
            if(deg2fp>=45 && deg2fp <= 150){
                animationSprite.Play("backword");
            }else if(deg2fp>=150 && deg2fp <= -135){
                animationSprite.Play("sideway");
                animationSprite.FlipH = true;
            }else if(deg2fp>=-135 && deg2fp <= -60){
                animationSprite.Play("forword");
            }else if(deg2fp>=-60 && deg2fp <= 45){
                animationSprite.Play("sideway");
            }
        }else{
            animationSprite.Play("forword");
        }
    }
    public void _on_DetectionZone_area_entered(Area2D area){
        if (area.GetType() == new Footprint().GetType()){
            _Footprint = (Footprint)area;
        }
    }
    public void _on_DetectionZone_area_exited(Area2D area){
        if(area == _Footprint){
            _Footprint = null;
        }
    }
    public void _on_HitTimer_timeout(){
        toHit = true;
    }
    public void _on_HitArea_body_entered(Node body){
        if(body.IsInGroup("player")){
            Player = (Player)body;
            if(toHit){
                toHit = false;
                HitTimer.Start();
                GS.takeDamage(damage);
            }
        }
    }
    public void _on_HitArea_body_exited(Node body){
        if(body == Player){
            Player = null;
        }
    }
}
