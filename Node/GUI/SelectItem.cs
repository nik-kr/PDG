using Godot;
using System;

public class SelectItem : Sprite
{

    private Item _item;
    public Item item{
        get{
            return _item;
        }
        set{
            _item = value;
            if(_item == null){
                SI.isTaken = false;
                this.QueueFree();
            }
        }
    }

    private SingInventory SI;

    public Texture ItemTexture{
        get{
            return item.ItemTexture;
        }
        set{
            item.ItemTexture = value;
            GetNode<Sprite>(".").Texture = item.ItemTexture;
        }
    }

    public int ItemCount{
        get{
            return item.ItemCount;
        }
        set{
            item.ItemCount = value;
            GetNode<Label>("ItemCount").Text = item.ItemCount.ToString();
            if(item.ItemCount <= 0){
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
