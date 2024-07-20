using Godot;
using System;
using System.Collections.Generic;
using Peaky.Coroutines;
using System.Collections;
public partial class Projectile : Node3D
{
    [Export] RigidBody3D rb;
    [Export] public float speed = 2f;
    [Export]public float lifeSpan = 5;
    [Export] public Node3D mesh;
    [Export] public SoundData shatter;
    public virtual void Fire(Node3D target = null){
    
        rb.LinearVelocity = -GlobalTransform.Basis.Z * speed;
        Coro.Run(q(),this);
        IEnumerator q()
        {
           
            yield return new WaitForSeconds(lifeSpan);
            QueueFree();
        }
    }

    public override  void _Process(double delta){
  
        mesh.RotateY((float)delta);
        mesh.RotateX((float)delta);
    }

    public virtual void Shatter(){
        AudioManager.inst.Play(shatter,AudioType.WORLD,GlobalPosition);
        QueueFree();
    }
}