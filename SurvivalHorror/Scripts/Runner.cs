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
    [Export] Skeleton3D skeleton;
    [Export] WeakPoint weakPoint;
    List<Hitbox> hitboxes = new List<Hitbox>();
    float dist;
    public override void _Ready()
    {
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
        GD.Print("HIT EYE");
        base.WeakPointHit(dmg, weakPoint);
    }

    public override void _PhysicsProcess(double delta)
    {
        dist = GlobalPosition.DistanceTo(Player.inst.GlobalPosition);
        MoveToTarget(Player.inst.GlobalTransform.Origin);
       eye.GlobalRotation = SmoothLookAt.inst.LerpedGlobalRotation(eye,Player.inst);
        if(dist <= explosionRadius){
            GD.Print("BOOM");
        }
    }
}