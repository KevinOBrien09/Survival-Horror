using Godot;
using System;
using Peaky.Coroutines;
using System.Collections;
public partial class CameraAim : Node3D
{
    // [Export] Node3D playerTrans,cam;
    // [Export] float sens = .01f;
    // public override void _Process(double delta)
    // { 
    //     if(InputManager.inst.raiseWeapon)
    //     {
    //         float vertical = Input.GetAxis( "Forward", "Backward");
    //         float horizontal = Input.GetAxis( "Left", "Right");
    //          Vector2 wasd  = new Vector2(horizontal,vertical);
         
    //         Vector2 vel = new Vector2();
    //         vel.X = Mathf.MoveToward(wasd.X, 0,.1f);
    //         vel.Y = Mathf.MoveToward(wasd.Y, 0,.1f);
    //         playerTrans.RotateY(-vel.X * sens );
    //         RotateX(-vel.Y * sens );
    //         var cr = Rotation;
    //         cr.X = Mathf.Clamp(cr.X,Mathf.DegToRad(-80),Mathf.DegToRad(80));
    //         Rotation = cr;
    //     //}
    //     }
    // }

    
}