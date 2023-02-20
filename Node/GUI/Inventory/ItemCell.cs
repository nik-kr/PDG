using Godot;
using System;

public class ItemCell : CenterContainer
{

    private Item __Item;
    public Item _Item{
        get{
            return __Item;
        }
        set{
            __Item = value;
            if(value == null){
                ChangeCount(0);
                ChangeTexture(null);
            }
        }
    }

    private Singletone GI;
    private SingInventory SI;

    private void ChangeTexture(Texture texture){
        GetNode<Sprite>("Item/Sprite").Texture = texture;
    }
    private void ChangeCount(int count){
        GD.Print(count);
        GetNode<Label>("Item/ItemCount").Text = count.ToString();
        if(count <= 0){
            GetNode<Label>("Item/ItemCount").Text = "";
        }
    }

    public override void _Ready(){
        GI = GetNode<Singletone>("/root/GlobalSingletone");
        SI = GetNode<SingInventory>("/root/SingInventory");
        if(GI.DebugMode){
            var di = GD.Load<PackedScene>("res://Node/Items/DebugItem.tscn").Instance<DebugItem>();
            di.ItemCount = (int)GD.RandRange(0, 99);
            AddItem(di);
        }
    }
    public void AddItem(Item item){
        _Item = item;
        ChangeTexture(item.ItemTexture);
        ChangeCount(item.ItemCount);

    }
    public Item TakeItem(String TakeMode = "ALL", int count = 1){
        Item item = _Item;
        if(TakeMode == "ALL"){
            SI.TakeItem(this, TakeMode);
        }

        return item;
    }

    public void _on_TextureButton_pressed(){
        if(!SI.isTaken){
            TakeItem();
        }else{
            if(SI.si != null){
                if(SI.si.item.ItemType == _Item.ItemType &&
                    SI.si.item.ItemTexture == _Item.ItemTexture &&
                    SI.si.item.ItemName == _Item.ItemName){
                    int c = 0;
                    int _c = 0;
                        if(_Item.ItemCount + SI.si.item.ItemCount <= _Item.maxItemStack){
                            c = _Item.ItemCount + SI.si.item.ItemCount;
                            _c = SI.si.ItemCount - (c - _Item.ItemCount);
                            ChangeCount(c);
                        }else{
                            c = _Item.maxItemStack;
                            _c = SI.si.ItemCount - (c - _Item.ItemCount);
                            ChangeCount(c);
                        }
                        
                    SI.si.ItemCount = _c;
                }
            }
        }
    }
}
