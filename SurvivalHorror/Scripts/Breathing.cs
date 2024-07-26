using Godot;
using System;

public partial class Breathing : Node3D
{
    [Export] Vector2 range;
    [Export] float swayScale,speed;
    float swayTime;
    public override void _Ready()
    {
        
    }
    public override void _Process(double delta)
    {
        var target = Curve(swayTime)/swayScale;
        swayTime += (float)delta;
        if(swayTime > 999
        ){
            swayTime = 0;
        }
        RotationDegrees = RotationDegrees.Lerp(target,speed);
    }

    Vector3 Curve(float t){

        return new Vector3(range.X * Mathf.Sin(range.X*t*Mathf.Pi),range.Y * Mathf.Sin(range.Y*t*Mathf.Pi),0);
    }

    public void Kill(){
      Position = Vector3.Zero;
    }
}