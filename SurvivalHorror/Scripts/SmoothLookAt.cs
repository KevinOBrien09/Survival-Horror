using Godot;
using System;

public partial class SmoothLookAt : Node3D
{
    public static SmoothLookAt inst;
 
    public override void _Ready()
    {
        if(inst != null)
        {GD.PrintErr("TWO LOOKATMANAGERS");}
        inst = this;
    }

    public Vector3 LerpedGlobalRotation(Node3D looker, Node3D target,bool clampY = false,float speed = .1f)
    {
        Node3D lookAtDummy = new Node3D();
        GetTree().Root.AddChild(lookAtDummy) ;
        lookAtDummy.GlobalPosition = looker.GlobalPosition;
        lookAtDummy.Basis = looker.Basis;
        lookAtDummy.LookAt(target.GlobalPosition,Vector3.Up);
        var v = lookAtDummy.GlobalRotation;
        float x = Mathf.LerpAngle(looker.GlobalRotation.X, v.X,speed);
        float y = Mathf.LerpAngle(looker.GlobalRotation.Y, v.Y,speed);
        float z = Mathf.LerpAngle(looker.GlobalRotation.Z, v.Z,speed);
        Vector3 a = new Vector3();
        if(clampY)
        {a = new Vector3(0,y,0);}
        else
        {a = new Vector3(x,y,z);}
        lookAtDummy.QueueFree();
        return a;
    }

    public Vector3 LerpToAngle(Node3D looker, Vector3 target,float speed = .1f){
        var v = target;
     
        float x = Mathf.LerpAngle(looker.GlobalRotation.X, v.X,speed);
        float y = Mathf.LerpAngle(looker.GlobalRotation.Y, v.Y,speed);
        float z = Mathf.LerpAngle(looker.GlobalRotation.Z, v.Z,speed);

        return new Vector3(x,y,z);;
    }

    public Vector3 LerpedLocalRotation(Node3D looker, Node3D target,bool clampY = false,float speed = .1f)
    {
        Node3D lookAtDummy = new Node3D();
        GetTree().Root.AddChild(lookAtDummy) ;
        lookAtDummy.GlobalPosition = looker.GlobalPosition;
        lookAtDummy.Basis = looker.Basis;
        lookAtDummy.LookAt(target.GlobalPosition,Vector3.Up);
        var v = lookAtDummy.GlobalRotation;
        float x = Mathf.LerpAngle(looker.Rotation.X, v.X,speed);
        float y = Mathf.LerpAngle(looker.Rotation.Y, v.Y,speed);
        float z = Mathf.LerpAngle(looker.Rotation.Z, v.Z,speed);
        Vector3 a = new Vector3();
        if(clampY)
        {a = new Vector3(0,y,0);}
        else
        {a = new Vector3(x,y,z);}
        lookAtDummy.QueueFree();
        return a;
    }
}