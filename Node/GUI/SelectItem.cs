using Godot;
using System;

public class SelectItem : Sprite
{

    public Item item;
    private SingInventory SI;

    private Texture _ItemTexture;
    public Texture ItemTexture{
        get{
            return _ItemTexture;
        }
        set{
            _ItemTexture = value;
            GetNode<Sprite>(".").Texture = _ItemTexture;
        }
    }
    private int _ItemCount;
    public int ItemCount{
        get{
            return _ItemCount;
        }
        set{
            _ItemCount = value;
            GetNode<Label>("ItemCount").Text = _ItemCount.ToString();
            if(_ItemCount <= 0){
                SI.isTaken = false;
                this.QueueFree();
            }
        }
    }
    public override void _Ready()
    {
        SI = GetNode<SingInventory>("/root/SingInventory");
    }

    public override void _PhysicsProcess(float delta)
    {
        Position = GetGlobalMousePosition();
    }
}
