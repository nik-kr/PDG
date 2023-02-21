using Godot;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
// using static 

public class Level : Node2D
{
    //Expotr Variable


    //Nodes
    private PackedScene _Room_l = (PackedScene)GD.Load("res://Node/Room/Room.tscn");
    private Room _Room;
    private TileMap Map;
    private KinematicBody2D Player;
    private Camera2D PreloadCamera;
    //DebugParameters
    private bool debugMode = false;
    private bool drawRoomOutline = true;
    private bool drawAstarLine = true;

    //Any Variable
    public int LEVEL;
    private Room startRoom;
    private Room endRoom;
    private bool playMode = false;
    //_Room Generate Params
    private int MinSize = 10;
    private int MaxSize = 16;
    private int TileSize = 16;
    private int NumRooms = 45;
    private int Hspread = 400;
    private float Cull = 0.5f;

    private AStar path;
    private Singletone GS;

    public override void _Draw(){
        // if(drawRoomOutline || debugMode){
        //     foreach(Room room in GetNode("Rooms").GetChildren()){
        //         DrawRect(new Rect2(room.Position - room.Size, room.Size*2),
        //         new Color(0, 1, 0), false);
        //     }
        // }
        // if(drawAstarLine || debugMode){
        //     if(path != null){
        //         foreach(int p in path.GetPoints()){
        //             foreach(int c in path.GetPointConnections(p)){
        //                 Vector3 pp = path.GetPointPosition(p);
        //                 Vector3 cp = path.GetPointPosition(c);
        //                 DrawLine(new Vector2(pp.x, pp.y),
        //                         new Vector2(cp.x, cp.y),
        //                         new Color(1, 1, 0, 1), 15, true);
        //             }
        //         }
        //     }
        // }
    }
    public override void _Ready()
    {
        GS = GetNode<Singletone>("/root/GlobalSingletone");
        Map = GetNode<TileMap>("./TileMap");
        PreloadCamera = GetNode<Camera2D>("./Camera");
        PreloadCamera.Visible = true;

        GS.TileSize = Map.CellQuadrantSize;

        AStar ast = new AStar();
        ast.AddPoint(0, new Vector3(1, 1, 1));
        // GD.Print(ast.GetPoints().GetType().ToString());
        if (GS.Character == "Warrior")
        {
            Player = (KinematicBody2D)GS._PacWarrior.Instance();
            Player.Name = "Player";
        }
        GD.Randomize();
        MakeRooms();
        // _Room = (Node2D)_Room_l.Instance();
    }
    public async void MakeRooms()
    {
        foreach (int i in GD.Range(NumRooms))
        {
            Vector2 Pos = new Vector2((float)GD.RandRange(-Hspread, Hspread), (float)GD.RandRange(-Hspread, Hspread) / 2);
            Room room = (Room)_Room_l.Instance();
            int w = (int)(MinSize + GD.Randi() % (MaxSize - MinSize));
            int h = (int)(MinSize + GD.Randi() % (MaxSize - MinSize));

            room.MakeRoom(Pos, new Vector2(w, h) * GS.TileSize);
            GetNode("Rooms").AddChild(room);
        }
        await Task.Delay(TimeSpan.FromMilliseconds(1400));

        List<Vector3> roomPosition = new List<Vector3>();
        foreach (Room room in GetNode("./Rooms").GetChildren())
        {
            if (GD.Randf() < Cull)
            {
                GetNode("./Rooms").RemoveChild(room);
                room.QueueFree();
            }
            else
            {
                room.Mode = Room.ModeEnum.Static;
                roomPosition.Add(new Vector3(room.Position.x, room.Position.y, 0));
                // GD.Print(roomPosition.Count);
            }
        }
        await ToSignal(GetTree(), "idle_frame");
        
        path = findMst(roomPosition);

        makeMap();

        drawRoomOutline = false;
        drawAstarLine = false;

        findStartRoom();

        PreloadCamera.Visible = false;
        startRoom.AddChild(Player);
        Player.GlobalPosition = startRoom.Position;
        playMode = true;


        findEndRoom();
        fillRooms();

    }

    private void fillRooms()
    {
        foreach (Room room in GetNode("./Rooms").GetChildren())
        {
            if (room.RoomType == "")
            {
                // room.GetType().GetField("RoomType").SetValue(room, );
                room.FillRoom();
                // Type _r = room.GetType();
                // MethodInfo _FillRoom = _r.GetMethod("FillRoom");
                // _FillRoom.Invoke(r, null);
                room.GetNode<CollisionShape2D>("CollisionShape2D").QueueFree();
            }
        }
        
    }

    private void findStartRoom()
    {
        float minX = float.MaxValue;
        // GD.Print("MinValue: " + minX.ToString());
        Room r = new Room();
        foreach (Room room in GetNode("Rooms").GetChildren())
        {
            if (room.Position.x < minX)
            {
                // startRoom = room;
                minX = room.Position.x;
                startRoom = room; 
                r = room;
            }
            // r.RoomType = "START_ROOM";
            // r.FillRoom();
            // r.GetType().GetField("RoomType").SetValue(r, "START_ROOM");

            // Type _r = r.GetType();
            // MethodInfo _FillRoom = _r.GetMethod("FillRoom");
            // _FillRoom.Invoke(r, null);
        }
        r.FillRoom("START_ROOM");
    }

    private void findEndRoom()
    {
        float maxX = float.MinValue;
        // GD.Print("MaxValue: " + maxX.ToString());
        Room r = new Room();
        foreach (Room room in GetNode("Rooms").GetChildren())
        {
            if (room.Position.x > maxX)
            {
                // endRoom = room;
                maxX = room.Position.x;
                r = room;
                endRoom = r;
                
            }
            // r.RoomType = "END_ROOM";
            // r.FillRoom();
            // r.GetType().GetField("RoomType").SetValue(r, "END_ROOM");
            // Type _r = r.GetType();
            // MethodInfo _FillRoom = _r.GetMethod("FillRoom");
            // _FillRoom.Invoke(r, null);
        }
        r.FillRoom("END_ROOM");
    }

    private AStar findMst(List<Vector3> Nodes)
    {
        AStar Path = new AStar();
        Path.AddPoint(Path.GetAvailablePointId(), Nodes[0]);
        Nodes.RemoveAt(0);
        // new AStar().GetPoints();
        while (Nodes.Count > 0)
        {
            float minDist = float.MaxValue;
            Vector3 minP = new Vector3();
            Vector3 p = new Vector3();
            foreach (int p1 in Path.GetPoints())
            {
                var _p1 = Path.GetPointPosition(p1);
                foreach (Vector3 p2 in Nodes)
                {
                    if (_p1.DistanceTo(p2) < minDist)
                    {
                        minDist = _p1.DistanceTo(p2);
                        minP = p2;
                        p = _p1;
                    }
                }
            }
            int n = Path.GetAvailablePointId();
            Path.AddPoint(n, minP);
            if ((int)GD.RandRange(0, 1) == 1)
            {
                int rn = (int)GD.RandRange(0, Nodes.Count);
                Vector3 rp = Nodes[rn];
                Path.AddPoint(Path.GetAvailablePointId(), rp);
            }
            Path.ConnectPoints(Path.GetClosestPoint(p), n);
            Nodes.Remove(minP);
        }

        return Path;
    }

    private void makeMap()
    {
        Map.Clear();
        Rect2 fullRect = new Rect2();
        foreach (Room room in GetNode("./Rooms").GetChildren())
        {
            ;
            // RectangleShape2D shape = (RectangleShape2D)room.GetNode<CollisionShape2D>("CollisionShape2D").Shape;
            Rect2 r = new Rect2(room.Position - room.Size,
                           room.s.Extents * 2);
            // RectangleShape2D shape = (RectangleShape2D)room.GetNode<CollisionShape2D>("CollisionShape2D").Shape;
            // Rect2 r = new Rect2(room.Position - (Vector2)room.GetType().GetField("size").GetValue(this),
            // 				shape.Extents * 2);
            room.GetNode<CollisionShape2D>("CollisionShape2D").QueueFree();

            fullRect = fullRect.Merge(r);
        }
        Vector2 topLeft = Map.WorldToMap(fullRect.Position);
        Vector2 bottomRight = Map.WorldToMap(fullRect.End);
        foreach (var x in GD.Range((int)topLeft.x, (int)bottomRight.x))
        {
            foreach (var y in GD.Range((int)topLeft.y, (int)bottomRight.y))
            {
                Map.SetCell(x, y, 1);
            }
        }

        List<int> Corridors = new List<int>();
        foreach (Room room in GetNode("./Rooms").GetChildren())
        {
            // Vector2 s = ((Vector2)room.GetType().GetField("size").GetValue(this) / GS.TileSize).Floor();
            Vector2 s = (room.Size / GS.TileSize).Floor();
            Vector2 pos = Map.WorldToMap(room.Position);
            Vector2 ul = (room.Position / TileSize).Floor() - s;
            foreach (var x in GD.Range(2, (int)(s.x * 2 - 1)))
            {
                foreach (var y in GD.Range(2, (int)(s.y * 2 - 1)))
                {
                    Map.SetCell((int)ul.x + x, (int)ul.y + y, 0);
                }
            }
            Map.UpdateBitmaskRegion();
            var p = path.GetClosestPoint(new Vector3(room.Position.x,
                                            room.Position.y, 0));
            foreach (var conn in path.GetPointConnections(p))
            {
                if (!(Corridors.Contains(conn)))
                {
                    var start = Map.WorldToMap(new Vector2(path.GetPointPosition(p).x, path.GetPointPosition(p).y));
                    var end = Map.WorldToMap(new Vector2(path.GetPointPosition(conn).x, path.GetPointPosition(conn).y));
                    carvePath(start, end);
                }
            }
            Corridors.Add(p);
        }
    }

    private void carvePath(Vector2 Position1, Vector2 Position2)
    {
        int xDiff = Math.Sign(Position2.x - Position1.x);
        int yDiff = Math.Sign(Position2.y - Position1.y);

        if (xDiff == 0) { xDiff = (int)Math.Pow(-1.0, GD.Randi() % 2); }
        if (yDiff == 0) { yDiff = (int)Math.Pow(-1.0, GD.Randi() % 2); }

        Vector2 x_y = Position1;
        Vector2 y_x = Position2;

        for (int i = 0; i < 4; i++)
        {
            foreach (var x in GD.Range((int)Position1.x, (int)Position2.x, xDiff))
            {
                Map.SetCell(x, (int)x_y.y, 0);
                Map.SetCell(x, (int)x_y.y + yDiff + i - 2, 0);
            }
            foreach (var y in GD.Range((int)Position1.y, (int)Position2.y, yDiff))
            {
                Map.SetCell((int)y_x.x, y, 0);
                Map.SetCell((int)y_x.x + xDiff + i - 2, y, 0);
            }
        }
        Map.UpdateBitmaskRegion();
    }


}
