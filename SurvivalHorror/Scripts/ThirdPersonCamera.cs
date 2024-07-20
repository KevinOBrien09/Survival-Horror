using Godot;
using System;

public partial class ThirdPersonCamera : Node3D
{
    [Export]public Node3D target;
    [Export] float sens;

   [Export] public Node3D followTarget;
    [Export] public Vector3  offset;
    [Export] float smoothSpeed;

    public override void _Process(double delta)
    {
        if(followTarget!= null){
        Vector3 desiredposition = followTarget.Position + offset;
        Vector3 smoothedposition = Position.Lerp(desiredposition, smoothSpeed*(float)delta);
        Position = smoothedposition;
        }
     
    }

    public override void _UnhandledInput(InputEvent @event){
        if(!WeaponManager.inst.weaponIsRaised){
            RotateY(Mathf.DegToRad(-InputManager.inst.cameraInput.X *  sens));
            target.RotateX(Mathf.DegToRad(-InputManager.inst.cameraInput.Y * sens));
            Vector3 v =target.Rotation;
            v.X = Mathf.Clamp(target.Rotation.X,Mathf.DegToRad(-90),Mathf.DegToRad(45));
            target.Rotation = v;
        }
        // else{
        //     GlobalRotationDegrees = -followTarget.GlobalRotationDegrees;
        //     target.GlobalRotationDegrees  = Vector3.Zero;
        // }
      
    }

   
    
}