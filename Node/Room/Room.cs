using Godot;
using System;

public class Room : RigidBody2D
{
    private RoomFiller RF = new RoomFiller();

    public Vector2 Size;
    public String RoomType = "";

    public Vector2 RoomPosition1 = new Vector2();
    public Vector2 RoomPosition2 = new Vector2();

    private static Singletone GS = new Singletone();
    private CollisionShape2D collision;
    public RectangleShape2D s = new RectangleShape2D();

    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        // GD.Print(GS.TileSize);
    }

    public void MakeRoom(Vector2 pos, Vector2 size){
        Position = pos;
        Size = size;
        s.CustomSolverBias = 0.75f;
        s.Extents = Size;
        collision = GetNode<CollisionShape2D>("CollisionShape2D");
        collision.Shape = s;
    }

    public void FillRoom(String roomType = ""){
        // RoomPosition1 = new Vector2(
        //                 (int)Math.Floor((Position.x - s.Extents.x) / GS.TileSize) * GS.TileSize + GS.TileSize * 2,
        //                 (int)Math.Floor((Position.y - s.Extents.y) / GS.TileSize) * GS.TileSize + GS.TileSize * 2);
        // RoomPosition2 = new Vector2(
        //                 (int)Math.Floor((Position.x + s.Extents.x) / GS.TileSize) * GS.TileSize - GS.TileSize * 2,
        //                 (int)Math.Floor((Position.y + s.Extents.y) / GS.TileSize) * GS.TileSize - GS.TileSize * 2);
        
        // RoomPosition1 = new Vector2(
        //                 (int)Math.Floor((Position.x - s.Extents.x) / GS.TileSize) * GS.TileSize - GS.TileSize * 3,
        //                 (int)Math.Floor((Position.y + s.Extents.y) / GS.TileSize) * GS.TileSize + GS.TileSize * 3);
        // RoomPosition2 = new Vector2(
        //                 (int)Math.Floor((Position.x + s.Extents.x) / GS.TileSize) * GS.TileSize + GS.TileSize * 3,
        //                 (int)Math.Floor((Position.y - s.Extents.y) / GS.TileSize) * GS.TileSize - GS.TileSize * 3);

		RoomPosition1 = new Vector2(
                        (int)Math.Floor((Position.x - s.Extents.x)/GS.TileSize) * GS.TileSize + GS.TileSize*4,
						(int)Math.Floor((Position.y - s.Extents.y)/GS.TileSize) * GS.TileSize + GS.TileSize*4);
		RoomPosition2 = new Vector2(
                        (int)Math.Floor((Position.x + s.Extents.x)/GS.TileSize) * GS.TileSize - GS.TileSize*4,
						(int)Math.Floor((Position.y + s.Extents.y)/GS.TileSize) * GS.TileSize - GS.TileSize*4);

        // RoomPosition1 = new Vector2(

        // );
        // RoomPosition2 = new Vector2();
        
        if (roomType == ""){
            roomType = RF.GetRoomType();
        }
        RoomType = roomType;
        // GD.Print(RoomPosition1);
        // GD.Print(RoomPosition2);
        // collision.QueueFree();
        RF.FillRoom(this, RoomPosition1, RoomPosition2, roomType);
    }
}
