using Godot;
using System;

public class SingInventory : Node
{
    public bool isTaken = false;
    private PackedScene p_si = GD.Load<PackedScene>("res://Node/GUI/SelectItem.tscn");
    public SelectItem si;

    public void TakeItem(ItemCell ic, String TakeMode = "ALL"){
        Item item;
        if(!isTaken){
            item = ic._Item;
            si = p_si.Instance<SelectItem>();
            si.item = item;
            si.ItemCount = item.ItemCount;
            si.ItemTexture = item.ItemTexture;
            isTaken = true;
            if(TakeMode == "ALL"){
                ic._Item = null;
            }
            ic._Item = null;
            ic.GetParent().GetParent().AddChild(si);
        }else{

        }
    }
}
