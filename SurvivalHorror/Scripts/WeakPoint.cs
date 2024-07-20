using Godot;
using System;
using System.Collections.Generic;
using Peaky.Coroutines;
using System.Collections;
public partial class WeakPoint : StaticBody3D
{
    [Export] public Enemy owner;
    [Export]  public CollisionShape3D collisionShape3D;
    public virtual void Hit(float damage){
        owner.WeakPointHit(damage,this);
          
        
    }

    public void Activate(){
        Visible = true;
        collisionShape3D.Disabled = false;
    }

    public void Deactivate(){
        Visible = false;
        collisionShape3D.Disabled = true;
    }
}