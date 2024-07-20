using Godot;
using System;

public partial class AimCrossHair : TextureRect
{
    public static AimCrossHair inst;
    [Export] Breathing breathing;
    public override void _Ready(){
    if(inst != null){
        GD.PrintErr("TWO Crosshair MANAGERS!!!");
    }
    inst = this;
    }
    public void EnterAimMode(){
        CursorManager.inst.Visible = true;
        Visible = false;
        breathing.SetProcess   (true);

    }

    public void EnterInvestigationMode(){
        CursorManager.inst.Visible = false;
        Visible = false;
        breathing.SetProcess   (false);
        breathing.RotationDegrees = Vector3.Zero;
        CursorManager.inst.ChangeCursorState(Input.MouseModeEnum.Captured);
    }

}