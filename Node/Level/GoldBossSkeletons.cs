using Godot;
using System;

public class GoldBossSkeletons : Node2D
{
    private StaticBody2D InvWall;
    private Singletone GS;
    Godot.Collections.Array skeletonCount;

    public override void _Ready()
    {
        InvWall = GetNode<StaticBody2D>("../InvisibleWall");
        GS = GetNode<Singletone>("/root/GlobalSingletone");
    }
    public override void _PhysicsProcess(float delta)
    {
        skeletonCount = this.GetChildren();
        if(skeletonCount.Count == 0){
            InvWall.QueueFree();
            GS.task.Visible = false;
        }
    }
}
