using Godot;
using System;
using System.Collections.Generic;
using Peaky.Coroutines;
using System.Collections;
public partial class Hitbox : StaticBody3D
{
    [Export] public string area;
    [Export] public Enemy owner;
    [Export] public CollisionShape3D collisionShape3D;
    public override void _Ready()
    {
        area = GetParent().Name;
        owner =  GetParent().Owner as Enemy;
        if(owner == null){
            GD.Print("OWNER IS NULL!!!");
        }
    }
    public virtual void Hit(float damage){
        owner.Hit(damage,this);
    }
}