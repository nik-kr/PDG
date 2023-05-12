using Godot;
using System;
using System.Collections.Generic;

public class RoomFiller : Node
{
    
    private Singletone GS = new Singletone();
    private NFunc NF = new NFunc();
    private Random random = new Random();

    public String []RoomType   = {"GHOST_SPAWNER", "DEFAULT_ROOM", "BAT_ROOM", "BOOST_ROOM", "EMPTY_ROOM", "END_ROOM"};
    public float  []Wights     = {      10f,           60f,            15f,        500f,          10f,        0.001f};

    public override void _Ready(){
        GS = GetNode<Singletone>("/root/GlobalSingletone");
    }
    public String GetRoomType(){
        return(NF.Choise(RoomType, Wights));
    }
    public void FillRoom(Room _Room, Vector2 Position1, Vector2 Position2, String _RoomType){
        if(_RoomType == ""){
            _RoomType = GetRoomType();
        }
        if(_RoomType == "END_ROOM"){
            PackedScene _Ladder = (PackedScene)ResourceLoader.Load("res://Node/Ladder/Ladder.tscn");
            StaticBody2D Ladder = (StaticBody2D)_Ladder.Instance();
            _Room.AddChild(Ladder);
            // Ladder.Position = new Vector2((Position1.x + Position2.x)/2,
            //                                 Position1.y + Position2.y)/2;
            // Ladder.GlobalPosition = _Room.Position;
            Ladder.ToLocal(new Vector2((Position1.x + Position2.x)/2,
                                            Position1.y + Position2.y)/2);
        }else if(_RoomType == "GHOST_SPAWNER"){
            GhostMonolith ghostMonolith  = GS.pGhostMonolith.Instance<GhostMonolith>();
            _Room.AddChild(ghostMonolith);
            ghostMonolith.GlobalPosition = new Vector2((Position1.x + Position2.x)/2, (Position1.y + Position2.y)/2);
        }else if(_RoomType == "BAT_ROOM"){
            String []batRoomDict = {"bat", "empty"};
            float []_Weights     = { 20f,    200f};
            for(int i = (int)Position1.x; i < Position2.x; i+= GS.TileSize){
                for(int j = (int)Position1.y; j < Position2.y; j+= GS.TileSize){
                    if(new NFunc().Choise(batRoomDict, _Weights) == "bat"){
                        Bat bat = GS.pBat.Instance<Bat>();
                        _Room.AddChild(bat);
                        bat.GlobalPosition = new Vector2(i, j);
                    }
                }
            }
        }else if(_RoomType == "BOOST_ROOM"){ //BOOST ROOM
            PackedScene []boosts = {GS.pHPBoost, GS.pStrenghtBoost};
            PackedScene pBoost = boosts[random.Next(0, boosts.Length)];
            Area2D boost = pBoost.Instance<Area2D>();
            _Room.AddChild(boost);
            boost.GlobalPosition = new Vector2((Position1.x + Position2.x)/2, (Position1.y + Position2.y)/2);
        }else if(_RoomType == "DEFAULT_ROOM"){
            String []_Items = {"", "Chest", "Skeleton", "DebugItem", "Slime"};
            float []_Weights = {1.5f, 0.000f, 0.06f, 0.00f, 0.05f};
            if(GS.DebugMode){
                _Weights[3] = 0.05f;
            }

            Dictionary<String, PackedScene> Items = new Dictionary<String, PackedScene>(){
                ["Skeleton"]    = (PackedScene)ResourceLoader.Load("res://Node/Enemy/Skeleton/Skeleton_lvl1.tscn"),
                ["Slime"]       = (PackedScene)ResourceLoader.Load("res://Node/Enemy/Slime/Slime.tscn")
            };
            
            if(_RoomType == "DEFAULT_ROOM"){
                for(int i = (int)Position1.x; i < Position2.x; i+= GS.TileSize){
                    for(int j = (int)Position1.y; j < Position2.y; j+= GS.TileSize){
                        String _Item = new NFunc().Choise(_Items, _Weights);
                        if(_Item != ""){
                            var ToPlace = (Node2D)Items[_Item].Instance();
                            _Room.AddChild(ToPlace);
                            ToPlace.GlobalPosition = new Vector2(i, j);
                        }
                    }
                }
            }
        }
    }
}
