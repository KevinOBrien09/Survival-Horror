using Godot;
using System;
using System.Collections.Generic;
using Peaky.Coroutines;
using System.Collections;
public partial class HomingProjectile : CharacterBody3D
{
    Node3D homingTarget;
    [Export] NavigationAgent3D agent;
    [Export] GpuParticles3D explode,gemBreak;
    [Export] Area3D blastRadius;
    [Export] Node3D model; 
    [Export]SoundData explodeSFX;
    float   speed;
    bool exploded;
    float dist;
    float timeStamp;
    public  void Fire(Node3D target = null){
        homingTarget = target;
        speed = .05f;
        timeStamp = 100 + Time.GetTicksMsec();
    }

    public override void _PhysicsProcess(double delta)
    { 
        if(exploded)
        { return; }

        dist = GlobalPosition.DistanceTo(Player.inst.GlobalPosition);
        if(dist < .5f)
        {
            
            exploded = true;
         //   AudioManager.inst.Play(shatter,AudioType.WORLD,GlobalPosition);
            AudioManager.inst.Play(explodeSFX,AudioType.WORLD,GlobalPosition);
            agent.Velocity = Vector3.Zero;
            explode.Emitting = true;
            model.Visible = false;
            foreach (var item in blastRadius.GetOverlappingBodies())
            {
                Player p = item as Player;
                Enemy e = item as Enemy;
                if(p != null)
                {
                    Player.inst.Hit(1);
                    Player.inst.Invul();
                }
            }  
            Coro.Run(q(),this);
            IEnumerator q(){
                yield return new WaitForSeconds(1);
                QueueFree();
            }
        }
        else{
            //if(timeStamp < Time.GetTicksMsec()){
                timeStamp = 2000 + Time.GetTicksMsec();
                speed += .01f;
                speed = Mathf.Clamp(speed,0,1);
           // }
                // agent.TargetPosition = homingTarget.GlobalPosition;
                // var currentLoc = GlobalPosition;
                // var nextLoc = agent.GetNextPathPosition();
                // var yPos = GlobalPosition.MoveToward(homingTarget.GlobalPosition,speed);
                // var targetPos = new Vector3( nextLoc.X,yPos.Y,nextLoc.Z);
                // if(GlobalPosition != homingTarget.GlobalPosition){
             LookAt(homingTarget.GlobalPosition);
                //         agent.Velocity = targetPos;

                agent.TargetPosition = homingTarget.GlobalPosition;
       
                var currentLoc = GlobalTransform.Origin;
                var nextLoc = agent.GetNextPathPosition();
            
                //YClampLookAt(nextLoc);
                var newVel = (nextLoc - currentLoc).Normalized() * speed;
                agent.Velocity = newVel;
            

        
        
          
        }
       

       
    }

    public virtual void VelocityComputed(Vector3 safeVel)
    { 
        // // if(dead)
        // // { return; }
        // GD.Print("XD");
      Velocity =    Velocity.MoveToward(safeVel,999);
        MoveAndSlide();
    }

    public  void Shatter(){
        if(exploded){
            return;
        }
        exploded = true;
        Hitmarker.inst.HitTween(true);
        gemBreak.Emitting = true;
        agent.Velocity = Vector3.Zero;
        //AudioManager.inst.Play(shatter,AudioType.WORLD,GlobalPosition);
        model.Visible = false;
        Coro.Run(q(),this);
        IEnumerator q(){
            yield return new WaitForSeconds(1);
            QueueFree();
        }
    }

  

  
}