using Godot;
using System;

public class Ladder : StaticBody2D
{

    private Singletone GS;
    private Game game;

    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        game = GetNode<Game>("/root/Game");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void _on_Area2D_body_entered(Node body){
        if(body.IsInGroup("player")){
            GS.level += 1;
            game.UpdateLevel();
        }
    }
}
