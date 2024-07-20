
using Godot;
using System;
using Peaky.Coroutines;
using System.Collections;
public partial class Clickable : Node3D
{
    [Export] public Node3D outLine;
    [Export] float interactRadius = 5;
    public float dist;
    public void Go()
    {
        if(canBeClicked())
        {
            GD.Print("Go");
        }
        
    }
    public override void _Process(double delta)
    {
        dist =  GlobalTransform.Origin.DistanceTo(Player.inst.GlobalTransform.Origin);
        OutlineToggle();
    }

    public virtual void OutlineToggle()
    {
        if(outLine == null){
            return;
        }
        if(!canBeClicked()  && InteractManager.inst.currentClickableHover == this)
        { outLine.Visible = false; }
        else if(canBeClicked()  && InteractManager.inst.currentClickableHover == this && outLine.Visible == false)
        { outLine.Visible = true; }
    }

    public void MouseEnter()
    {
        InteractManager.inst.currentClickableHover = this;
      //  GD.Print("Mouse Enter");
    }

    public void MouseExit()
    {  
        if(outLine != null){
        outLine.Visible = false;
        }
        InteractManager.inst.currentClickableHover = null;
     //   GD.Print("Mouse Exits");
    }

    public bool canBeClicked(){
        return interactRadius > dist;
    }

    

    public virtual void LeftClick(){

    }

    public virtual void RightClick(){

    }

  
}