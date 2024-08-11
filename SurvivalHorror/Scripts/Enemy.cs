using Godot;
using System;
using System.Collections.Generic;
using Peaky.Coroutines;
using System.Collections;
public partial class Enemy : CharacterBody3D
{
   
    [Export] public SoundData gore;
    [Export] SoundData footstep;
    [Export] public NavigationAgent3D agent;
    [Export] public float speed;
    [Export] public Health health;
    [Export] Node3D lookAt;
    public bool dead;
    public override void _Ready(){
        DelayProcessStart();
    }

    public virtual List<WeakPoint> GetTargets(){
        return null;
    }
    public Node3D GetInterfaceNode(){
        return this;
    }
    public Node3D LookAtNode(){
        return lookAt;
    }


    public virtual void Hit(float dmg,Hitbox hitbox = null)
    {
        Hitmarker.inst.HitTween(false);
        health.Damage(dmg);
        AudioManager.inst.Play(gore,AudioType.WORLD,GlobalPosition);
    }

    public virtual void WeakPointHit(float dmg,WeakPoint weakPoint){
        Hitmarker.inst.HitTween(true);
        health.Damage(dmg*3);
        AudioManager.inst.Play(gore,AudioType.WORLD,GlobalPosition);
        
    }

    public virtual void EnterPainState(){

    }

    public void Footstep(){
        if(!dead){
    AudioManager.inst.Play(footstep,AudioType.WORLD,GlobalPosition);
        }
    
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
        if(target.IsEqualApprox(Vector3.Zero)){
            return;
        }
        LookAt(target,new Vector3(0,-1,0));
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