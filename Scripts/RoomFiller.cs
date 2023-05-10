using Godot;
using System;
using System.Collections.Generic;

public class RoomFiller : Node
{
    
    private Singletone GS = new Singletone();
    private NFunc NF = new NFunc();

    public String []RoomType   = {"GHOST_SPAWNER", "DEFAULT_ROOM", "SPAUNER_ROOM", "BOSS_ROOM"};
    public float  []Wights      = {0.2f, 0.80f, 0.0f, 0.0f};

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
            GD.Print("Ghost Monolith created!");
        }else if(GS.level%5 < 5){
            String []_Items = {"", "Chest", "Skeleton", "DebugItem", "Slime"};
            float []_Weights = {0.98f, 0.000f, 0.06f, 0.00f, 0.05f};

            Dictionary<String, PackedScene> Items = new Dictionary<String, PackedScene>(){
                ["Chest"]       = (PackedScene)ResourceLoader.Load("res://Node/Chests/Wood_Chest/Wood_Chest.tscn"),
                ["Skeleton"]    = (PackedScene)ResourceLoader.Load("res://Node/Enemy/Skeleton/Skeleton_lvl1.tscn"),
                ["DebugItem"]   = (PackedScene)ResourceLoader.Load("res://Node/Items/DebugItem.tscn"),
                ["Slime"]       = (PackedScene)ResourceLoader.Load("res://Node/Enemy/Slime/Slime.tscn")
            };
            
            if(_RoomType == RoomType[1]){
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
                // foreach(int i in GD.Range((int)Position1.x, (int)Position2.x, GS.TileSize)){
                //     foreach(int j in GD.Range((int)Position1.y, (int)Position2.y, GS.TileSize)){
                //         String _Item = new NFunc().Choise(_Items, _Weights);
                //         if(_Item != ""){
                //             var ToPlace = (Node2D)Items[_Item].Instance();
                //             _Room.AddChild(ToPlace);
                //             ToPlace.Position = new Vector2((int)i, (int)j);
                //             GD.Print(ToPlace.Position);
                //         }
                //     }
                // }
            }
        }
    }
}
