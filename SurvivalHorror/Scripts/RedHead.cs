using Godot;
using System;
using System.Collections.Generic;
using Peaky.Coroutines;
using System.Collections;
public partial class RedHead : Enemy
{
    [Export] MeshInstance3D face;
    [Export] Vector2 shootFreq;
    [Export] WeakPoint[] eyes;
    [Export] Node3D projSpawn;
    [Export] AnimationPlayer player;
    [Export] Hitbox hitbox;
    [Export] AudioStreamPlayer3D drone;
    [Export] PackedScene projectilePrefab;
    Vector3 targetPos;
    float timeStamp;
    public bool shooting;
    bool leftEyeOpen;

        float q = 0;
    public override void _Ready()
    {
        DelayProcessStart();
        face.Set("blend_shapes/Fcl_EYE_Surprised",0);
        face.Set("blend_shapes/Fcl_EYE_Close",1);
        timeStamp = GetCD()*2;
        eyes[0].Deactivate();
        eyes[1].Deactivate();
        RandomNumberGenerator rng = new RandomNumberGenerator();
        leftEyeOpen = rng.RandiRange(0,1) == 0;
        player.Play("Hover");
        if(!AudioManager.inst.mute){
            drone.Play();
        }
      
          //GetNewTarget();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(!shooting)
        {
            if(timeStamp <= Time.GetTicksMsec())
            { Shoot(); }
            else
            { 
                
                
                MoveToTarget(targetPos); 
                if(GlobalPosition.DistanceTo(targetPos) < 2 || Velocity == Vector3.Zero){
                    GetNewTarget();
                }
                else{
                 
                }
            }
         
        }
        else  
        {
            YClampLookAt(Player.inst.GlobalPosition);
            Velocity = Vector3.Zero;
            agent.Velocity = Vector3.Zero;
        }
    }

    public override void MoveToTarget(Vector3 targetPos){
    agent.TargetPosition = targetPos;
    
    var currentLoc = GlobalTransform.Origin;
    var nextLoc = agent.GetNextPathPosition();
    if(!shooting){
    YClampLookAt(Player.inst.GlobalPosition);
    }

    var newVel = (nextLoc - currentLoc).Normalized() * speed;
    agent.Velocity = newVel;
    }


   


    public void GetNewTarget()
    {
        Vector3 point = MiscFunctions.RandomSpherePos(3,Player.inst.GlobalPosition);
        targetPos = point;
        // if(IsPositionInMap(point))
        // { }
        // else
        // {GetNewTarget();}
    }

    public override     void Die(){
       QueueFree();
    }

    public override void Hit(float dmg,Hitbox hitbox = null)
    {
       // AudioManager.inst.Play(hitConnected,AudioType.FLAT);
        health.Damage(dmg);
        AudioManager.inst.Play(gore,AudioType.WORLD,GlobalPosition);
    }

    public override void WeakPointHit(float dmg,WeakPoint weakPoint)
    {
        GD.Print("WP HIT");
        AudioManager.inst.Play(hitConnected,AudioType.FLAT);
        health.Damage(dmg);
        AudioManager.inst.Play(gore,AudioType.WORLD,GlobalPosition);
    }

    public void SwapOpenEye(){
       
        if(leftEyeOpen){
            leftEyeOpen = false;
      
            eyes[1].Activate();
            eyes[0].Deactivate();
            
            Tween a = CreateTween();
            Tween b = CreateTween();
            a.TweenProperty(face, "blend_shapes/Fcl_EYE_Close_L",0,.1f);
            b.TweenProperty(face, "blend_shapes/Fcl_EYE_Close_R",1,.1f);
            b.Finished+=()=>{
                //eyes[1].SetCollisionLayerValue(3,true);
            };
         
        }
        else{
            leftEyeOpen = true;
            eyes[1].Deactivate();
            eyes[0].Activate();
            Tween a = CreateTween();
            Tween b = CreateTween();
            a.TweenProperty(face, "blend_shapes/Fcl_EYE_Close_L",1,.1f);
            b.TweenProperty(face, "blend_shapes/Fcl_EYE_Close_R",0,.1f);
            b.Finished+=()=>{
              //  eyes[0].SetCollisionLayerValue(3,true);
            };
        }
    }

    public float GetCD()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        return rng.RandfRange(shootFreq.X,shootFreq.Y) * 1000;
    }
    
 

    public void Shoot()
    {
        shooting = true;
        Coro.Run(q(),this);
        IEnumerator q(){
            Velocity = Vector3.Zero;
            agent.Velocity = Vector3.Zero;
            face.Set("blend_shapes/Fcl_MTH_A",1);
            face.Set("blend_shapes/Fcl_EYE_Surprised",1);
            face.Set("blend_shapes/Fcl_EYE_Close",0);
            eyes[1].Activate();
            eyes[0].Activate();
            Tween tw1 = CreateTween();
            tw1.TweenProperty(drone,"volume_db",-80,.5f);
            Projectile proj =   projectilePrefab.Instantiate() as HomingProjectile;
            GetTree().Root.AddChild(proj);
            proj.GlobalPosition = projSpawn.GlobalPosition;
            proj.GlobalRotation = projSpawn.GlobalRotation;
          proj.Fire(Player.inst);
            yield return new WaitForSeconds(1);
            Tween tw2 = CreateTween();
            tw2.TweenProperty(drone,"volume_db",0,.5f);
            eyes[1].Deactivate();
            eyes[0].Deactivate();
            face.Set("blend_shapes/Fcl_EYE_Surprised",0);
            face.Set("blend_shapes/Fcl_EYE_Close",1);
            Tween b = CreateTween();
            b.TweenProperty(face,"blend_shapes/Fcl_MTH_A",0,.2f);
            b.Finished += ()=>
            {
                shooting = false;
                timeStamp = GetCD() + Time.GetTicksMsec();
            };
        }
    
    }

}