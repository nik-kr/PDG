using Godot;
using System;
using System.Collections.Generic;

public class Inventory : Control
{
    private List<ItemCell> InvCell      = new List<ItemCell>();
    private List<ItemCell> ChestCell    = new List<ItemCell>();

    private Panel inventoryPanel;
    private Panel chestInventory;

    public override void _Ready()
    {
        inventoryPanel = GetNode<Panel>("Panels/InventoryPanel");
        chestInventory = GetNode<Panel>("Panels/ChestIncentory");

        foreach(ItemCell i in inventoryPanel.GetChildren()){
            if(i.IsInGroup("InvCell")){
                InvCell.Add(i);
            }
        }
        foreach(ItemCell i in chestInventory.GetChildren()){
            if(i.IsInGroup("InvCell")){
                ChestCell.Add(i);
            }
        }

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
