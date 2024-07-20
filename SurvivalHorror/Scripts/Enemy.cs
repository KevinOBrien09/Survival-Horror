using Godot;
using System;
using System.Collections.Generic;
using Peaky.Coroutines;
using System.Collections;
public partial class Enemy : CharacterBody3D
{
    [Export] public SoundData hitConnected;
    [Export] public SoundData gore;
    [Export] SoundData footstep;
    [Export] public NavigationAgent3D agent;
    [Export] public float speed;
    [Export] public Health health;
    public bool dead;
    public override void _Ready(){
        DelayProcessStart();
    }


    public virtual void Hit(float dmg,Hitbox hitbox = null)
    {
        AudioManager.inst.Play(hitConnected,AudioType.FLAT);
        health.Damage(dmg);
        AudioManager.inst.Play(gore,AudioType.WORLD,GlobalPosition);
    }

    public virtual void WeakPointHit(float dmg,WeakPoint weakPoint){
        
        Hit(dmg*2); 
        
    }

    public void Footstep(){
        AudioManager.inst.Play(footstep,AudioType.WORLD,GlobalPosition);
    }

    public virtual void Die(){
        dead = true;
        QueueFree();
    }

    public bool IsPositionInMap(Vector3 pos)
    {
        var rid = agent.GetNavigationMap();
        return NavigationServer3D.MapGetClosestPoint(rid, pos).IsEqualApprox(pos);
    }
     
    
    public void DelayProcessStart()
    {
        SetProcess(false);
        SetPhysicsProcess(false);
        Coro.Run(q(),this);
        IEnumerator q()
        {
            yield return new WaitOneFrame();
            SetPhysicsProcess(true);
            SetProcess(true);
        }
    }

    public override void _PhysicsProcess(double delta)
    {

        MoveToTarget(Player.inst.GlobalTransform.Origin);
    
    }

    public virtual void MoveToTarget(Vector3 targetPos){
        agent.TargetPosition = targetPos;
       
        var currentLoc = GlobalTransform.Origin;
        var nextLoc = agent.GetNextPathPosition();
     
        YClampLookAt(nextLoc);
        var newVel = (nextLoc - currentLoc).Normalized() * speed;
        agent.Velocity = newVel;
    }

    public virtual void YClampLookAt(Vector3 target){
        LookAt(target);
        Rotation = new Vector3(0,Rotation.Y,0);
    }

    public virtual void VelocityComputed(Vector3 safeVel)
    { 
        // // if(dead)
        // // { return; }
        // GD.Print("XD");
        Velocity = Velocity.MoveToward(safeVel,999);
        MoveAndSlide();
    }
}