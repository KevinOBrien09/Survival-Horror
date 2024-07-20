using Godot;
using System;

public partial class Breathing : Node3D
{
    [Export] Vector2 range;
    [Export] float speed;
    bool inTween;
    Tween t;
    bool up;
    public override void _Ready()
    {
        SetProcess(false);
    }
    public override void _Process(double delta)
    {
        if(!inTween){
            Move();
        }
    }

    public void Move(){
        t = CreateTween().SetEase(Tween.EaseType.Out);
        Vector3 target = new Vector3();
        if(up )
        { target = new Vector3(range.Y,0,0); }
        else
        { target = new Vector3(range.X,0,0); }
        inTween = true;
        t.TweenProperty(this, "rotation_degrees",target,speed);
        t.Finished +=()=>{inTween = false;
        up = !up;};
    }

    public void Kill(){
        t.Kill();
        inTween  =false;
    }
}