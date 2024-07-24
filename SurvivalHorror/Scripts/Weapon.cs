using Godot;
using System;

public partial class Weapon : Node3D
{
    [Export] bool clamp;
    [Export] RayCast3D ray;
    [Export] Vector2 xClamp = new Vector2(-90,90);
    [Export] Vector2 yClamp = new Vector2(-180,90);
    [Export] Vector2 zClamp = new Vector2(-90,90);
    [Export] Node3D characterModel;
    [Export] float damage;
    public virtual void LookAtCursor(){
        GlobalRotation = SmoothLookAt.inst.LerpedGlobalRotation(this,Aimer.inst,false);
    
        if(clamp){
           RotationDegrees = ClampAxis();
        }
    }

    public virtual void Activate(){
            characterModel.Visible = true;
    }   

    public virtual void Disable(){
          characterModel.Visible = false;
    }

    public virtual void Rise(){
        
    }

    public virtual void Lower(){
        
    }
    
    public virtual void Strike(){
        
    }

    public void Scan()
    {
        var e = ray.GetCollider() as Enemy;
        var wp = ray.GetCollider() as WeakPoint;
        var hb = ray.GetCollider() as Hitbox;
        var p = ray.GetCollider() as Projectile;
        var hp = ray.GetCollider() as HomingProjectile;
        if(wp != null)
        { wp.Hit(damage); }
        else if(hb != null)
        { hb.Hit(damage); }
        else if(e != null)
        { e.Hit(damage); }
        else if(p != null)
        { p.Shatter();}
        else if(hp != null)
        { p.Shatter();}
    }

    public virtual bool isLowered(){
        return false;
    }

    public Vector3 ClampAxis(){
        return  new Vector3(
        Mathf.Clamp(RotationDegrees.X,xClamp.X,xClamp.Y),
        Mathf.Clamp(RotationDegrees.Y,yClamp.X,yClamp.Y),
        Mathf.Clamp(RotationDegrees.Z,zClamp.X,zClamp.Y));
    }  
}