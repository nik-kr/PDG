using Godot;
using System;

public class Inventory : Control
{
    public override void _Ready()
    {
        
    }
    public void CloseInventory(){
        this.Visible = false;
    }
    public void OpenInventory(){
        this.Visible = true;
    }
}
