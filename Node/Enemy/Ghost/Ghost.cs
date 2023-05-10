using Godot;

public class Ghost : Enemy
{

    [Export]
    public float speed = 60;
    [Export]
    public float hitTime = 3.5f;
    [Export]
    public float damage = 0.5f;

    private Timer HitTimer;
    private KinematicBody2D Player = null;
    private Singletone GS;
    private AnimatedSprite animationSprite;

    private Vector2 direction = Vector2.Zero;

    private bool toHit = true;

    public override void _ExitTree()
    {
        GS.ghostCount -= 1;
    }

    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");

        MaxHealthPoint = 3 + (3/10)*GS.level; HealthPoint = MaxHealthPoint;
        animationSprite = GetNode<AnimatedSprite>("AnimatedSprite");

        HitTimer = GetNode<Timer>("HitTimer");
        HitTimer.WaitTime = hitTime;
        foreach(KinematicBody2D p in GetTree().GetNodesInGroup("player")){
            Player = p;
        }
        SetCollisionLayerBit(0, false);
        SetCollisionLayerBit(1, false);
        SetCollisionMaskBit(1, false);
        SetCollisionMaskBit(0, false);

    }

    public override void _PhysicsProcess(float delta){
        if(GS.player != null){
            direction = GlobalPosition.DirectionTo(GS.player.GlobalPosition);
            Vector2 desiredVelocity = direction * speed;
            Vector2 steering = (desiredVelocity - velocity) * delta * 4f;

            velocity += steering;
            velocity = MoveAndSlide(velocity);

            float deg2fp = Godot.Mathf.Rad2Deg(GlobalPosition.AngleToPoint(GS.player.GlobalPosition));
            //LEFT UP
            if(deg2fp >-7.5 && deg2fp < 52.5){
                animationSprite.Play("backword");
            }
            //RIGHT UP
            else if(deg2fp < 52.5 && deg2fp>7.5){
                animationSprite.Play("backword");
                animationSprite.FlipH = true;
            }
            //LEFT DOWN
            else if(deg2fp>-97.5 && deg2fp < 7.5){
                animationSprite.Play("forword");
            }
            //RIGHT DOWN
            else if(deg2fp>97.5 && deg2fp < 7.5){
                animationSprite.Play("forword");
                animationSprite.FlipH = true;
            }
        }
    }


    public void _on_HitArea_body_entered(Node body){
        if(body.IsInGroup("player")){
            Player = (KinematicBody2D)body;
            if(toHit){
                toHit = false;
                GS.takeDamage(damage);
                HitTimer.Start();
            }
        }
    }
    
    public void _on_HitArea_body_exited(Node body){
        if(body == Player){
            Player = null;
        }
    }

}
