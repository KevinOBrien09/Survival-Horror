using Godot;
using System;

public partial class CameraDash : Node3D
{
    public void Left(float t)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this,"rotation_degrees:z",3f,t);
        tween.Finished +=()=>
        {
            Tween tween = CreateTween();
            tween.TweenProperty(this,"rotation_degrees:z",0,t);
        };
    }

    public void Right(float t)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this,"rotation_degrees:z",-3f,t);
        tween.Finished +=()=>
        {
            Tween tween = CreateTween();
            tween.TweenProperty(this,"rotation_degrees:z",0,t);
        };
    }

    public void ForwardRight(float t)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this,"rotation_degrees",new Vector3(-3f,0,-3f) ,t);
        tween.Finished +=()=>
        {
            Tween tween = CreateTween();
            tween.TweenProperty(this,"rotation_degrees",Vector3.Zero,t);
        };
    }

    public void ForwardLeft(float t)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this,"rotation_degrees",new Vector3(-3f,0,3f) ,t);
        tween.Finished +=()=>
        {
            Tween tween = CreateTween();
            tween.TweenProperty(this,"rotation_degrees",Vector3.Zero,t);
        };
    }
    public void BackwardLeft(float t)
    {Back(t);}

    public void BackwardRight(float t)
    {Back(t);}

    public void Back(float t)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this,"position",new Vector3(0,-.2f,0) ,t);
        tween.Finished +=()=>
        {
            Tween tween = CreateTween();
            tween.TweenProperty(this,"position",Vector3.Zero,t);
        };
    }
}