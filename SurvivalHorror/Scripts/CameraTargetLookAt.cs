using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class CameraTargetLookAt : Node3D
{
    public static CameraTargetLookAt inst;
    [Export] Camera3D mainCam;
    List<Node3D> potentialTargets = new List<Node3D>();
    public int target;
    public bool look;
    float ogFOV;
    public string positive,negative;

	public override void _Ready()
	{
		inst = this;
        ogFOV = mainCam.Fov;
    }

    public void EnterLookAt<T>(List<T> targets,bool X){
        potentialTargets.Clear();
        target = 0;
        foreach (var item in targets)
        {
            if(!potentialTargets.Contains(item as Node3D)){
                potentialTargets.Add(item as Node3D);
            }
           
        }
        if(X){
            potentialTargets = potentialTargets.OrderBy(go =>  go.GlobalPosition.X).ToList();
            positive = "Right";
            negative = "Left";
            target = (int)potentialTargets.Count/2;
        }
        else{
            potentialTargets = potentialTargets.OrderBy(go =>  go.GlobalPosition.Y).ToList();
            positive = "Forward";
            negative = "Backward";
            target = (int)0;
        }
      
     
     
        look = true;
    }

    public override void _Process(double delta)
    {
        if(look){
            GlobalRotation = SmoothLookAt.inst.LerpedGlobalRotation(this,(Node3D)currentTarget(),false);
            Swap();
        }
        else{
            GlobalRotationDegrees = Vector3.Zero;
        }
      
    }

    public void ChangeFOV(float fov){
        Tween t = CreateTween();
        t.TweenProperty(mainCam,"fov",fov,.1f);
    }

    public Node3D currentTarget(){
        return potentialTargets[ target];
    }



    public void Swap(){
        
        int previousSelectedWeapon = target;
        if (Input.IsActionJustPressed(positive))
        {
            if (target >= potentialTargets.Count - 1)
                target = 0;
            else
               target++;
                
        }
       if (Input.IsActionJustPressed(negative))
        {
            if (target <= 0)
              target = potentialTargets.Count - 1;
            else
                target--;
        }
       
    }
        
        
       
        
    
}