using Godot;
using System;
using Peaky.Coroutines;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public partial class Runner : Enemy
{

    [Export] AnimationPlayer player,eyeA;
    [Export] float explosionRadius;
    [Export] Node3D eye;
    [Export] CollisionShape3D collider;
    [Export] Skeleton3D skeleton;
    [Export] WeakPoint weakPoint;
    [Export] Node3D model;
    [Export] GpuParticles3D gibs,spikes;
    [Export] SoundData goreExplode,explode,spikeExplode;
    [Export] AudioStreamPlayer3D pant;
    [Export] Area3D blastRadius;
    List<Hitbox> hitboxes = new List<Hitbox>();
    float dist;
    bool exploded;
    public override void _Ready()
    {
        if(!AudioManager.inst.mute){
            pant.Play();
        }
        DelayProcessStart();
        player.Play("Walk/Walk");
        eyeA.Play("look");
         foreach (var item in skeleton.GetChildren())
        {
            var b = item as BoneAttachment3D;
            if(b != null)
            {
                
                var hb = b.GetChild(0) as Hitbox;
                if(hb != null){
                    hb.collisionShape3D = hb.GetChild(0) as CollisionShape3D;
                    hb.owner = this;
                    hitboxes.Add(hb);
                }
            }
        }
    }

    public override void WeakPointHit(float dmg, WeakPoint weakPoint)
    {
        Hitmarker.inst.HitTween(true);
        CleanExplode();
    }

    public override void Die(){
      
        dead = true;
        SpikeExplode();
   
    }

    public override void _PhysicsProcess(double delta)
    {
        dist = GlobalPosition.DistanceTo(Player.inst.GlobalPosition);
        MoveToTarget(Player.inst.GlobalTransform.Origin);
       eye.GlobalRotation = SmoothLookAt.inst.LerpedGlobalRotation(eye,Player.inst);
        if(dist <= explosionRadius){
          SpikeExplode();
        }
    }

    void CleanExplode(){
        dead = true;
        exploded = true;
        AudioManager.inst.Play(goreExplode,AudioType.WORLD,GlobalPosition);
        pant.QueueFree();
        foreach (var item in hitboxes)
        {
            item.collisionShape3D.Disabled = true;
        }
        weakPoint.collisionShape3D.Disabled = true;
        model.Visible = false;
        SetPhysicsProcess(false);
        agent.Velocity = Vector3.Zero;
        Velocity = Vector3.Zero;
        gibs.Emitting = true;
        collider.Disabled = true;
        Coro.Run(q(),this );
        IEnumerator q(){
            yield return new WaitForSeconds(1);
            QueueFree();
        }
    }

    public override void EnterPainState()
    {
        SpikeExplode();
    }

    public void SpikeExplode()
    {
        if(exploded){
            return;
        }
        exploded = true;
        AudioManager.inst.Play(spikeExplode,AudioType.WORLD,GlobalPosition);
        spikes.Emitting = true;
        foreach (var item in blastRadius.GetOverlappingBodies())
        {
            Player p = item as Player;
            Enemy e = item as Enemy;
            if(p != null){
                Player.inst.EnterPainState();
                Player.inst.Hit(1);
                Player.inst.Invul();
            }

            if(e != null){
                GD.Print("HIT Z");
                if(e != this){
                    e.EnterPainState();
                }
               
            }
        }  

        CleanExplode();
            
    }
}