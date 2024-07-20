using Godot;
using System;
using System.Collections.Generic;
using Peaky.Coroutines;
using System.Collections;
public partial class HomingProjectile : Projectile
{
    Node3D homingTarget;
    [Export] NavigationAgent3D agent;
    float timeStamp;
        public override void Fire(Node3D target = null){
        homingTarget = target;
        speed = .05f;
        timeStamp = 100 + Time.GetTicksMsec();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(timeStamp < Time.GetTicksMsec()){
            timeStamp = 2000 + Time.GetTicksMsec();
            speed += .0001f;
            speed = Mathf.Clamp(speed,0,1);
        }
        agent.TargetPosition = homingTarget.GlobalPosition;
        var currentLoc = GlobalPosition;
        var nextLoc = agent.GetNextPathPosition();
        var yPos = GlobalPosition.MoveToward(homingTarget.GlobalPosition,speed);
        var targetPos = new Vector3( nextLoc.X,yPos.Y,nextLoc.Z);
        if(GlobalPosition != homingTarget.GlobalPosition){
        LookAt(homingTarget.GlobalPosition);
        }

       
       
        GlobalPosition =   GlobalPosition.MoveToward(targetPos,speed);

       
    }

  

  
}