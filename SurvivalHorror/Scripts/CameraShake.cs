using Godot;
using System;
using Peaky.Coroutines;
using System.Collections;
public partial class CameraShake : Node3D
{
    public static CameraShake inst;
    public Node3D currentCamHolder;
    float mag;
    bool shaking;
    public override void _Ready()
    {
        if(inst != null){
            GD.PrintErr("TWO CAMERA SHAKES!!!");
        }
        inst = this;
    }
   
    public override void _Process(double delta){
        if(shaking)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            float x = rng.RandfRange(-1f,1f) * mag;
            float y = rng.RandfRange(-1f,1f) * mag;
            currentCamHolder.Position = new Vector3(x,y, currentCamHolder.Position.Z);
        }
       
    }

    public void SwapCamera(Node3D newHolder){
        currentCamHolder = newHolder;
    }

    
    public void Shake(float duration,float magn)
    {
        this.mag = magn;
        if(!shaking)
        {
            Coro.Run(w(),this);
        }
        IEnumerator w()
        {
            shaking = true;
            yield return new WaitForSeconds(duration);
            shaking = false;
            currentCamHolder.Position = Vector3.Zero;
        }
    }
}