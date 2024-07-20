using Godot;
using System;

public partial class Aimer : Node3D
{
	public static Aimer inst;
    [Export]   public Breathing child;
    [Export] float startingZDist = 10;
   
    float zDist;

    public override void _Ready()
    {
		if(inst!=null){
			GD.PrintErr("TWO AIMERS!!!");
		}
        inst = this;
        zDist = startingZDist;
    }
    public void ChangeZDist(float newZDist){
        zDist = newZDist;
    }

    public void ResetZDist(){
        zDist = startingZDist;
    }

    public override void _Process(double delta)
    {
    	Position = MouseToScreen();
        GlobalRotation = SmoothLookAt.inst.LerpedGlobalRotation(this,Player.inst,false);
	
    }

    public Vector3 MouseToScreen()
	{
        var spaceState = GetWorld3D().DirectSpaceState;
        var mousePos = GetViewport().GetMousePosition();
        var cam = GetTree().Root.GetCamera3D();
        var rayOrigin = cam.ProjectRayOrigin(mousePos) ;
        var rayEnd = rayOrigin + cam.ProjectRayNormal(mousePos) * zDist;
        return rayEnd;
	}
}
