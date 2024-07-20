using Godot;
using System;
using Peaky.Coroutines;
using System.Collections.Generic;
using System.Collections;

public partial class Goblin : Enemy
{
    enum State{IDLE,RUN,ATTACK1,ATTACK2,DIE}
    [Export] AnimationTree tree;
    [Export] AnimationPlayer player;
    [Export] float blendSpeed;
    State currentState;
    float deltaTime;
    Dictionary<State,float> dict = new Dictionary<State, float>();
    bool chase,disableStateChange,attacking;
    public override     void _Ready(){
        currentState = State.IDLE;
        DelayProcessStart();
        foreach (State item in Enum.GetValues(typeof(State)))
        {
            dict.Add(item,0);
        }
        chase = true;
    }   
    
   public override void _PhysicsProcess(double delta)
    {
        if(!dead)
        {
            if(!attacking){
                if(chase){
                    MoveToTarget(Player.inst.GlobalTransform.Origin);
            
                    YClampLookAt(Player.inst.GlobalPosition);
                    ChangeState(State.RUN);
                }
            }
            else{
                Velocity = Vector3.Zero;
                agent.Velocity = Vector3.Zero;
            }
        }
      
        HandleAnims();
        deltaTime = (float)delta;
        if(InputManager.inst.reload){
            ChangeState(State.ATTACK1);
        }
    }

    void ChangeState(State newState){
        
            currentState = newState;
            if(newState ==State.ATTACK1)
            {
                attacking = true;
                player.SpeedScale = 2f;
                Coro.Run(q(),this);
                IEnumerator q(){
                    yield return new WaitForSeconds(.6f);
                    ChangeState(State.ATTACK2);
                }
            }
            else if(newState ==State.ATTACK2){
                Coro.Run(q(),this);
                IEnumerator q(){
                    yield return new WaitForSeconds(.6f);
                    ChangeState(State.IDLE);
                    player.SpeedScale = 1;
                    attacking = false;
                   
                }
            }
        
    }

    public override     void Die(){
        dead = true;
        Velocity = Vector3.Zero;
        agent.Velocity = Vector3.Zero;
        ChangeState(State.DIE);
       //QueueFree();
    }

    void HandleAnims()
    {   float fastBlend = blendSpeed *3;
        foreach (var item in dict)
        {
            var key = item.Key;
            if(currentState == key){
                dict[key] = Mathf.Lerp(dict[key], 1,blendSpeed);
            }
            else{
                dict[key] = Mathf.Lerp(dict[key],0,blendSpeed);
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