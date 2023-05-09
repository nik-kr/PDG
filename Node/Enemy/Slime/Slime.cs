using Godot;
using System;

public class Slime : Enemy
{
    [Export]
    public float speed = 120;
    [Export]
    public float detectionRadius = 50;
    [Export]
    public float hitTime = 2.5f;
    [Export]
    public float damage = 1.5f;



    private Footprint _Footprint; // Footprint
    private Timer HitTimer;
    private Timer JumpTimer;
    private KinematicBody2D Player;
    private Singletone GS;
    private AnimatedSprite animationSprite;

    private Vector2 direction = Vector2.Zero;

    private bool toHit = true;


    public override void _Ready()
    {
        animationSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        MaxHealthPoint = 5 + (5/10) * GS.level; HealthPoint = MaxHealthPoint;
        deathAnimName = "death";
        GS = GetNode<Singletone>("/root/GlobalSingletone");
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
            if(deg2fp > -7.5 && deg2fp < 7.5){
                animationSprite.Play("backword");
            }else{
                animationSprite.Play("forword");
            }
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
        if (Player != null){
            takeDamage(damage);
        }
    }
    public void _on_JumpTimer_timeout(){

    }
    public void _on_HitArea_body_entered(Node body){
        if(body.IsInGroup("player")){
            Player = (KinematicBody2D)body;
            if(toHit){
                takeDamage(damage);
                toHit = false;
                HitTimer.Start();
            }
        }
    }
    public void _on_HitArea_body_exited(Node body){
        if(body == Player){
            Player = null;
        }
    }
    public void takeDamage(float damage){
        animPlayer.Play("takeDamage");
    }
}
