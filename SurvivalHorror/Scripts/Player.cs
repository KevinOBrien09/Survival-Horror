using Godot;
using System;
using Peaky.Coroutines;
using System.Collections.Generic;
using System.Linq;

public partial class Player : CharacterBody3D
{
    public static Player inst;
    [Export] SoundData footStep;
    [Export] float speed = 5;
    [Export] float gravity = 14;
    [Export] public ThirdPersonCamera thirdPersonCamera;
    [Export] AnimationPlayer player;
    [Export] AnimationTree tree;
    [Export] Health health;
    [Export] SoundData[] footsteps;
    [Export] SoundData heartBeatSingle;
    
    bool footstepA = true;
    Vector3 velocity;
    float deltaT;
    float moveLerp;

    public override void _Ready()
    {
        if(inst != null)
        {GD.PrintErr("TWO PLAYERS!!!");}
        inst = this;
    }
    
    public override void _PhysicsProcess(double delta)
    {
        if(InputManager.inst.pause)
        {
            if(Input.MouseMode == Input.MouseModeEnum.ConfinedHidden)
            {Input.MouseMode = Input.MouseModeEnum.Visible;}
        }

        deltaT = (float)delta;
        if(!WeaponManager.inst.weaponIsRaised)
        {
            Move(InputManager.inst.move);
        }
        else
        {
            tree.Set("parameters/Move/blend_position",0);
            
        }
    }

    public override void _Process(double delta)
    {
        if(!IsOnFloor()) 
        {
            velocity.Y -= gravity * deltaT;
          
        }
        if(WeaponManager.inst.weaponIsRaised){
            velocity.X = 0;
            velocity.Z = 0;
        }
        Velocity = velocity;
        MoveAndSlide();
    }


    public void Move(Vector2 move)
    {
       
        Vector3 XD = new Vector3(move.X, 0, move.Y);
        Vector3 inputDir = XD.Normalized();
        if(inputDir.Length() >= .1f)
        {
            float targetAngle = MathF.Atan2(inputDir.X,inputDir.Z) + thirdPersonCamera.GlobalRotation.Y;
            float smoothAngle = Mathf.LerpAngle(GlobalRotation.Y,targetAngle,.2f);
            moveLerp = Mathf.Lerp(moveLerp,1,.1f);
            GlobalRotation = new Vector3(0,smoothAngle,0);
            Vector3 f = ( GlobalTransform.Basis * new Vector3(0, 0, 1)).Normalized();
            velocity.X = f.X * speed;
            velocity.Z = f.Z * speed;
           
        }
        else
        {
            moveLerp = Mathf.Lerp(moveLerp,0,.1f);
            velocity.X = 0;
            velocity.Z = 0;
        }
       
        tree.Set("parameters/Move/blend_position",moveLerp);
    }

    public void Hit(float damage){
        AudioManager.inst.Play(heartBeatSingle,AudioType.FLAT);
        health.Damage(damage);
        CameraShake.inst.Shake(.3f,.1f);
    }
    
  
    public void Footstep()
    {
        if(InputManager.inst.move != Vector2.Zero)
        {
            Vector3 v = new Vector3(GlobalPosition.X,0,GlobalPosition.Z);
            if(footstepA)
            { AudioManager.inst.Play(footsteps[0],AudioType.WORLD,v); }
            else
            { AudioManager.inst.Play(footsteps[1],AudioType.WORLD,v); }
            footstepA = !footstepA;
        }
    }
}