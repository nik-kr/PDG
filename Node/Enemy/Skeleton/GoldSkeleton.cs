using Godot;
using System;

public class GoldSkeleton : Enemy
{
    [Export]
    public float speed = 100;
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
        MaxHealthPoint = 10 + (10/10)*GS.level; HealthPoint = MaxHealthPoint;
        damage = 2f + (2f/2)*GS.level;
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
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
            if(deg2fp>=45 && deg2fp <= 150){
                animationSprite.Play("Up");
            }else if(deg2fp>=150 && deg2fp <= -135){
                animationSprite.Play("Right");
            }else if(deg2fp>=-135 && deg2fp <= -60){
                animationSprite.Play("Down");
            }else if(deg2fp>=-60 && deg2fp <= 45){
                animationSprite.Play("Left");
            }
        }else{
            animationSprite.Play("Stay");
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
    public void _on_AnimationPlayer_animation_finished(String AnimName){
        if(AnimName == "takeDamage"){
            GS.takeDamage(damage);
        }
    }
}
