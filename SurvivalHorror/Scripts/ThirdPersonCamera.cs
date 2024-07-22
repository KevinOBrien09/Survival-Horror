using Godot;
using System;

public partial class ThirdPersonCamera : Node3D
{
    [Export]public Node3D target;
    [Export] float mouseSens;

   [Export] public Node3D followTarget;
    [Export] public Vector3  offset;
    [Export] float smoothSpeed;
    Vector2 padInput;
    float sens;
    public override void _Process(double delta)
    {
        if(followTarget!= null){
        Vector3 desiredposition = followTarget.Position + offset;
        Vector3 smoothedposition = Position.Lerp(desiredposition, smoothSpeed*(float)delta);
        Position = smoothedposition;
        }
    
        if(InputManager.inst.currentController == InputManager.ControlType.PAD){
            RotateCam(InputManager.inst.cameraInput);
            sens = InputManager.inst.PadSensChange(mouseSens);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if(InputManager.inst.currentController == InputManager.ControlType.MKB)
        {
            RotateCam(InputManager.inst.cameraInput);
            sens = mouseSens;
        }
    }


    void RotateCam(Vector2 input)
    {
        if(!WeaponManager.inst.weaponIsRaised){
            RotateY(Mathf.DegToRad(-input.X *  sens));
            target.RotateX(Mathf.DegToRad(-input.Y * sens));
            Vector3 v =target.Rotation;
            v.X = Mathf.Clamp(target.Rotation.X,Mathf.DegToRad(-90),Mathf.DegToRad(45));
            target.Rotation = v;
        }
    }

   
    
}