using Godot;
using System;
using System.Collections.Generic;

public class Inventory : Control
{
    private List<ItemCell> InvCell      = new List<ItemCell>();
    private List<ItemCell> ChestCell    = new List<ItemCell>();

    private Panel inventoryPanel;
    private Panel chestInventory;

    public bool FullInventory = false;

    public override void _Ready()
    {
        inventoryPanel = GetNode<Panel>("Panels/InventoryPanel");
        chestInventory = GetNode<Panel>("Panels/ChestIncentory");

        foreach(ItemCell i in inventoryPanel.GetChildren()){
            if(i.IsInGroup("InvCell")){
                InvCell.Add(i);
            }
        }
        GD.Print("invCells " + InvCell.Count.ToString());
        foreach(ItemCell i in chestInventory.GetChildren()){
            if(i.IsInGroup("InvCell")){
                ChestCell.Add(i);
            }
        }

    }
    public bool CollectItem(Item item){
        var remainingQuantity = item.ItemCount;
        foreach(ItemCell ic in InvCell){
            if(remainingQuantity > 0){
                int rq = ic.AddItem(item);
                remainingQuantity = rq;
            }
        }
        var full = (remainingQuantity == 0);
        if(!full){
            FullInventory = true;
        }else{
            FullInventory = false;
        }
        return full;
    }

    public void OpenChest(){
        chestInventory.Visible = true;
    }
    public void CloseChest(){
        chestInventory.Visible = false;
    }

    public void CloseInventory(){
        this.Visible = false;
    }
    public void OpenInventory(){
        this.Visible = true;
    }
}
