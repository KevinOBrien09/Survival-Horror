using Godot;
using System;
using Peaky.Coroutines;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public partial class Zombie : Enemy
{
    enum State{IDLE,WALK,HIT,DIE,ATTACK1,ATTACK2}
    [Export] Skeleton3D skeleton;
    [Export] AnimationTree tree;
    [Export] AnimationPlayer player;
    [Export] SoundData[] gemBreak;
    [Export] CollisionShape3D collider;
    [Export] float blendSpeed;
    [Export] float attackDist;
    [Export] RayCast3D attackRay;
    [Export] float attackDamage;
    [Export] SoundData leftSwipeSFX,rightSwipeSFX,smackedPlayer,hitGroan,randomNoise1,randomNoise2,thud,die;
    Dictionary<State,float> dict = new Dictionary<State, float>();
    List<WeakPoint> weakPoints = new List<WeakPoint>();
    List<Hitbox> hitboxes = new List<Hitbox>();
    State currentState;
    bool canMove,moving,inHit,inAttack,inSound,leftSwip,randomWalkFrame;
    float returnToIdle,soundTimeStamp;
    float dist;
    
    public override void _Ready()
    {
        foreach (State item in Enum.GetValues(typeof(State)))
        { dict.Add(item,0); }
        moving = true;
        canMove = true;
        DelayProcessStart();
        foreach (var item in skeleton.GetChildren())
        {
            var b = item as BoneAttachment3D;
            if(b != null)
            {
                string str = (string) b.Name; 
                if(str.Contains("GEM")){
                    WeakPoint wp =  b.GetChild(0).GetChild(0) as WeakPoint;
                    if(wp != null){
                        weakPoints.Add(wp);
                        wp.owner = this;
                    }
                }
                else{
                    var hb = b.GetChild(0) as Hitbox;
                    if(hb != null){
                        hitboxes.Add(hb);
                    }
                }
            }
        }
        leftSwip = MiscFunctions.FiftyFifty();

        foreach (var item in weakPoints)
        { item.Deactivate(); }
        List<int> l = new List<int>();
        for (int i = 0; i < weakPoints.Count; i++)
        { l.Add(i); }
        Random rnd = new Random();
        l  =  l.OrderBy((item) => rnd.Next()).ToList();

        for (int i = 0; i < 3; i++)
        {
            weakPoints[l[i]].Activate();
        }
    }

    public void PlaySound(SoundData sd)
    {
        if(!dead)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            soundTimeStamp =  (float)sd.file.GetLength() *1000 + Time.GetTicksMsec();
            //+ rng.RandfRange(5,10)
            AudioManager.inst.Play(sd,AudioType.WORLD,GlobalPosition);
        }
       
    }

    public override void _PhysicsProcess(double delta)
    {
        dist = GlobalPosition.DistanceTo(Player.inst.GlobalPosition);
        inSound = soundTimeStamp >= Time.GetTicksMsec();
   
        if(dead){
            return;
        }
        HandleAnims();
       
        if(canMove)
        {
            inAttack = dist <= attackDist && !inHit && canMove;
            moving = !inAttack && !inHit && canMove;
            if(moving)
            {
                MoveToTarget(Player.inst.GlobalTransform.Origin);
                if(!randomWalkFrame){
                    RandomNumberGenerator rng = new RandomNumberGenerator();
                    var q = rng.RandfRange(0,1f);
                    tree.Set("parameters/walkSeek/seek_request",q);
                    randomWalkFrame = true;
                }
                ChangeState(State.WALK);
            }
            if(inAttack)
            {
              
                if(currentState != State.ATTACK1 && currentState != State.ATTACK2)
                {
                    canMove = false;
                    var t = 1.6333f * 1000;
                    returnToIdle = t + Time.GetTicksMsec();
                    if(leftSwip)
                    { 
                        ChangeState(State.ATTACK1); 
                        tree.Set("parameters/leftSeek/seek_request",0);
                        PlaySound(leftSwipeSFX);
                    }
                    else
                    {
                        ChangeState(State.ATTACK2);
                        tree.Set("parameters/rightSeek/seek_request",0);
                        PlaySound(rightSwipeSFX);
                    }
                }
            }
        }
        else
        {
            Velocity = Vector3.Zero;
            agent.Velocity = Vector3.Zero;
            
            if(inHit)
            { 
                if(returnToIdle <= Time.GetTicksMsec() && !dead)
                {
                    canMove = true;
                    ChangeState(State.IDLE);
                    tree.Active = true;
                    inHit = false;
                }
            }
            else if(inAttack)
            {
                YClampLookAt(Player.inst.GlobalPosition);
                if(returnToIdle <= Time.GetTicksMsec() && !dead)
                {
                    leftSwip = !leftSwip;
                    ChangeState(State.IDLE);
                    tree.Active = true;
                    canMove = true;
                }

            }
        }
    }

    public void Attack()
    {
        if(attackRay.IsColliding())
        {
            var p = attackRay.GetCollider() as Player;
            if(p != null)
            {
                AudioManager.inst.Play(smackedPlayer,AudioType.WORLD,GlobalPosition);
                p.Hit(attackDamage);
            }
        }
    }
    
    void ChangeState(State newState){
        
        currentState = newState;
    }

    public override void Hit(float dmg, Hitbox hitbox = null)
    {
     
        base.Hit(dmg, hitbox);
    
    }

    public override void Die(){
        dead = true;
        tree.Active = false;
        collider.Disabled = true;
        agent.QueueFree();
        Velocity = Vector3.Zero;
        AudioManager.inst.Play(die,AudioType.WORLD,GlobalPosition);
        player.Play("Die/die");
        foreach (var item in hitboxes)
        {
            item.QueueFree();
        }


        Coro.Run(q(),this);
        IEnumerator q(){
            yield return new WaitForSeconds(5);
           // QueueFree();
        }
       
    }

    public void ThudSFX(){
        AudioManager.inst.Play(thud,AudioType.WORLD,GlobalPosition);
    }
    

  
    public override void WeakPointHit(float dmg,WeakPoint weakPoint)
    {
        SoundData sd = null;
        if(MiscFunctions.FiftyFifty())
        { sd = gemBreak[0]; }
        else
        { sd = gemBreak[1]; }
        AudioManager.inst.Play(sd,AudioType.WORLD,GlobalPosition);
        weakPoint.Deactivate();
        if(dead)
        {return;}
       var ps = weakPoint.GetParent().GetChild(1) as GpuParticles3D;
       ps.Emitting = true;
       
        inHit = true;
        tree.Active = false;
        health.Damage(4);
        AudioManager.inst.Play(gore,AudioType.WORLD,GlobalPosition);
        Hitmarker.inst.HitTween(true);
        if(!dead){
            foreach (var item in dict)
            {       
                var key = item.Key;
                if(currentState != key)
                { 
                    dict[key] = 0;
                }
            }
            var t = 1.2f * 1000;
            returnToIdle = t + Time.GetTicksMsec();
            dict[State.HIT] = 1;
            player.Play("Hit/hit");
            player.Seek(0);
            UpdateTree();
            canMove = false;
            ChangeState(State.HIT);
        }
      
    }

    public override void VelocityComputed(Vector3 safeVel)
    { if(dead){
            return;
        }
        if(canMove){
            Velocity = Velocity.MoveToward(safeVel,999);
            MoveAndSlide();
        }
        else
        {
            Velocity = Vector3.Zero;
            agent.Velocity = Vector3.Zero;
        }
     
    }
    void HandleAnims()
    {   
        if(currentState != State.HIT){
           
     
            float fastBlend = blendSpeed *3;
            foreach (var item in dict)
            {
                var key = item.Key;
                if(currentState == key)
                { dict[key] = Mathf.Lerp(dict[key], 1,blendSpeed); }
                else
                { dict[key] = Mathf.Lerp(dict[key],0,blendSpeed); }
            }
        }
        
        UpdateTree();
    }

    void UpdateTree(){
        //names in blend tree must be same as enum names
        foreach (var item in dict)
        {
            var key = item.Key;
            tree.Set("parameters/" + key.ToString() + "/blend_amount",dict[key]);
        }
    }
}