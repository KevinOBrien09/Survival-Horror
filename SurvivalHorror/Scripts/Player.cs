using Godot;
using System;
using Peaky.Coroutines;
using System.Collections.Generic;
using System.Linq;

public partial class Player : CharacterBody3D
{
    public enum State{IDLE,RUN,AIM,PAIN,DIE}
    public static Player inst;
    public bool inHit,hitInvul;
    [Export] SoundData footStep;
    [Export] float blendSpeed;
    [Export] float speed = 5;
    [Export] float gravity = 14;
    [Export] public ThirdPersonCamera thirdPersonCamera;
    [Export] AnimationPlayer player;
    [Export] AnimationTree tree;
    [Export] Health health;
    [Export] SoundData[] footsteps;
    [Export] SoundData heartBeatSingle;
    [Export] SoundData[] screams;
    Dictionary<State,float> dict = new Dictionary<State, float>();
    Vector3 velocity;
    State currentState;
    bool footstepA = true;
    float deltaT;
    float hitTimeStamp,invulTimeStamp;
   

    public override void _Ready()
    {
        foreach (State item in Enum.GetValues(typeof(State)))
        { dict.Add(item,0); }

        if(inst != null)
        {GD.PrintErr("TWO PLAYERS!!!");}
        inst = this;
    }

    public void EnterPainState()
    {
        if(hitInvul)
        {return;}
        inHit = true;
        tree.Set("parameters/PainSeek/seek_request",0);
        Velocity = Vector3.Zero;
        velocity = Vector3.Zero;
        hitTimeStamp = 1.2f*1000 +Time.GetTicksMsec();
        SetState(State.PAIN);
        if(WeaponManager.inst.weaponIsRaised){
            WeaponManager.inst.ExitAim();
        }
        AudioManager.inst.Play(screams[0],AudioType.FLAT);
    }

    public void SetState(State state){
        
        currentState = state;
       
    }
    
    public override void _PhysicsProcess(double delta)
    {
        if(InputManager.inst.pause)
        {
            if(Input.MouseMode == Input.MouseModeEnum.ConfinedHidden)
            {Input.MouseMode = Input.MouseModeEnum.Visible;}
        }

        deltaT = (float)delta;
        if(!WeaponManager.inst.weaponIsRaised && !inHit)
        {
            Move(InputManager.inst.move);
        }
      

       
    }

    // public void Scream(){
    //     RandomNumberGenerator rng = new RandomNumberGenerator();
    //     int i = rng.RandiRange(0,screams.Length-1);
    //     AudioManager.inst.Play(screams[i],AudioType.FLAT);
    // }
    

    public override void _Process(double delta)
    {
        if(Time.GetTicksMsec() >invulTimeStamp && hitInvul){
            hitInvul = false;
        }
        if(Time.GetTicksMsec()> hitTimeStamp && inHit){
            inHit = false;
        }
        HandleAnims();
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
            SetState(State.RUN);
            float targetAngle = MathF.Atan2(inputDir.X,inputDir.Z) + thirdPersonCamera.GlobalRotation.Y;
            float smoothAngle = Mathf.LerpAngle(GlobalRotation.Y,targetAngle,.2f);
          
            GlobalRotation = new Vector3(0,smoothAngle,0);
            Vector3 f = ( GlobalTransform.Basis * new Vector3(0, 0, 1)).Normalized();
            velocity.X = f.X * speed;
            velocity.Z = f.Z * speed;
           
        }
        else
        {
            SetState(State.IDLE);
            velocity.X = 0;
            velocity.Z = 0;
        }
       
      
    }

    public void Hit(float damage){
        if(hitInvul){
            return;
        }
        AudioManager.inst.Play(heartBeatSingle,AudioType.FLAT);
        health.Damage(damage);
        AudioManager.inst.Play(screams[1],AudioType.FLAT);
        CameraShake.inst.Shake(.3f,.1f);
       
    }

    public void Invul(){
        if(!hitInvul){
            hitInvul = true;
            invulTimeStamp = 1f*1000 + Time.GetTicksMsec();
        }
       
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


    void HandleAnims()
    {   
        //if(currentState != State.HIT){
            foreach (var item in dict)
            {
                var key = item.Key;
                if(currentState == key)
                { dict[key] = Mathf.Lerp(dict[key], 1,blendSpeed); }
                else
                { dict[key] = Mathf.Lerp(dict[key],0,blendSpeed); }
            }
       // }
        
        UpdateTree();
    }

    void UpdateTree(){
        foreach (var item in dict)
        {
            var key = item.Key;
            tree.Set("parameters/" + key.ToString() + "/blend_amount",dict[key]);
        }
    }
}