using Godot;
using System;
using Peaky.Coroutines;
using System.Collections;
public partial class CameraAim : Node3D
{
    [Export] Node3D playerTrans,cam;
    [Export] float sens = .01f;
    [Export] float clamp;
    public bool firstPerson;
    public override void _Input(InputEvent @event)
    { 
        if(firstPerson)
        {
         	playerTrans.RotateY(-InputManager.inst.cameraInput.X * sens);
            cam.RotateX(-InputManager.inst.cameraInput.Y * sens);
            Vector3 x = cam.Rotation;
            x.X = Mathf.Clamp(cam.Rotation.X,Mathf.DegToRad(-clamp),Mathf.DegToRad(clamp));
            cam.Rotation = x;
			
       
        }
    }

    
}