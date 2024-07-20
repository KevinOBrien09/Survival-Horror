using Godot;
using System;

public partial class InteractManager : Node3D
{
    public static InteractManager inst;
    public Clickable currentClickableHover;
    public override void _Ready()
    {
        if(inst != null)
        {GD.PrintErr("TWO INTERACT MANAGERS!!!");}
        inst = this;
    }

    public override void _Process(double delta){
    
        if(currentClickableHover != null){
            if(InputManager.inst.interact){
                currentClickableHover.Go();
            }
        
        
        }
      

    }

}