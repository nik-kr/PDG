using Godot;
using System;

public class ItemCell : CenterContainer
{

    [Export]
    public Texture CellBackground;

    [Export(PropertyHint.EnumSuggestion, "*,Helmet,Weapon,Boots,Chestplate,Gem,Artefact")]
    public String CurrentItem = "*";

    [Export(PropertyHint.EnumSuggestion, "Default,Chest")]
    public String InvType = "Default";


    private Item __Item = null;
    public Item _Item{
        get{
            return __Item;
        }
        set{
            __Item = value;
            if(value == null){
                ChangeCount(0);
                ChangeTexture(null);
            }else{
                ChangeCount(__Item.ItemCount);
                ChangeTexture(__Item.ItemTexture);
            }
        }
    }

    private Singletone GI;
    private SingInventory SI;

    private void ChangeTexture(Texture texture){
        if(_Item != null){
            _Item.ItemTexture = texture;
        }
        GetNode<Sprite>("Item/Sprite").Texture = texture;
    }
    private void ChangeCount(int count){
        if(_Item != null){
            _Item.ItemCount = count;
        }
        if(count > 1){
            GetNode<Label>("Item/ItemCount").Text = count.ToString();
        }
        else{
            GetNode<Label>("Item/ItemCount").Text = "";
        }
    }

    public override void _Ready(){
        GD.Randomize();
        GI = GetNode<Singletone>("/root/GlobalSingletone");
        SI = GetNode<SingInventory>("/root/SingInventory");
        if(GI.DebugMode && CurrentItem == "*"){
            var di = GD.Load<PackedScene>("res://Node/Items/DebugItem.tscn").Instance<DebugItem>();
            di.ItemCount = (int)GD.RandRange(0, 99);
            AddItem(di);
        }
        switch(CurrentItem){
            // "*,Helmet,Weapon,Boots,Chestplate,Gem,"
            case "*":
                if(InvType == "Default"){
                    GetNode<TextureButton>("TextureButton").TextureNormal = GD.Load<Texture>("res://Resources/GUI/Invenroty/EmptyCell.png");
                }else if(InvType == "Chest"){
                    GetNode<TextureButton>("TextureButton").TextureNormal = GD.Load<Texture>("res://Resources/GUI/Invenroty/EmptyChestCell.png");
                }
                break;
            case "Helmet":
                GetNode<TextureButton>("TextureButton").TextureNormal = GD.Load<Texture>("res://Resources/GUI/Invenroty/HelmetCell.png");
                break;
            case "Weapon":
                GetNode<TextureButton>("TextureButton").TextureNormal = GD.Load<Texture>("res://Resources/GUI/Invenroty/WeaponCell.png");
                break;
            case "Boots":
                GetNode<TextureButton>("TextureButton").TextureNormal = GD.Load<Texture>("res://Resources/GUI/Invenroty/BootsCell.png");
                break;
            case "Chestplate":
                GetNode<TextureButton>("TextureButton").TextureNormal = GD.Load<Texture>("res://Resources/GUI/Invenroty/ChestplateCell.png");
                break;
            case "Gem":
                GetNode<TextureButton>("TextureButton").TextureNormal = GD.Load<Texture>("res://Resources/GUI/Invenroty/GemCell.png");
                break;
            case "Artefact":
                GetNode<TextureButton>("TextureButton").TextureNormal = GD.Load<Texture>("res://Resources/GUI/Invenroty/ArtefactCell.png");
                break;
        }
    }
    public int AddItem(Item item){
        int count = item.ItemCount;
        if ((_Item == null && CurrentItem == item.ItemType) || (_Item == null && CurrentItem == "*")){
            _Item = item;
            ChangeTexture(item.ItemTexture);
            ChangeCount(item.ItemCount);
            return 0;
        }
        else if (_Item != null){
            if(_Item.ItemType == item.ItemType && _Item.ItemTexture == item.ItemTexture && _Item.ItemName == item.ItemName){
                int maxI = _Item.maxItemStack;
                int c = (_Item.ItemCount + item.ItemCount);
                if(c <= maxI){
                    ChangeCount(_Item.ItemCount + item.ItemCount);
                    return 0;
                }else{
                    ChangeCount(_Item.maxItemStack);
                    return c - maxI;
                }
            }else{
                return count;
            }
        }

        return count;
    }
    public Item TakeItem(String TakeMode = "ALL", int count = 1){
        Item item = _Item;
        if(TakeMode == "ALL"){
            SI.TakeItem(this, TakeMode);
        }

        return item;
    }

    public void _on_TextureButton_pressed(){
        if(!SI.isTaken && _Item != null){
            TakeItem();
        }
        else if (SI.isTaken){
            if(_Item != null){
                //If have selected item
                if(SI.si != null && SI.si.item.ItemType == _Item.ItemType &&
                    SI.si.item.ItemTexture == _Item.ItemTexture &&
                    SI.si.item.ItemName == _Item.ItemName){
                    int c = 0;
                    int _c = 0;
                    if(CurrentItem == "*" || CurrentItem == SI.si.item.ItemType || _Item.ItemType == SI.si.item.ItemType){
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
            //Else if have't selected item
            else if(_Item == null && SI.si != null){
                if(CurrentItem == "*" || CurrentItem == SI.si.item.ItemType){
                    GD.Print(123);
                    AddItem(SI.si.item);
                    // _Item = SI.si.item;
                    SI.si.QueueFree();
                    SI.si = null;
                    SI.isTaken = false;
                }
            }
        }
    }
}
