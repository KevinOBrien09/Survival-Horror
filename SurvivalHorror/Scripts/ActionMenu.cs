using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class ActionMenu : Control
{
    [Export] ActionTab[] tabs;
    Dictionary<State,ActionTab> tabDict = new Dictionary<State, ActionTab>();
    public enum State{FIRE,SKILL,ITEMS}
    public enum AimState{NONE,CHARACTERS,WEAKPOINTS}
    public enum AimDirection{VERT,HORI};
    State currentState;
    AimState currentAimState;
    string positive,negative;

    public override void _Ready(){
        foreach (var item in tabs)
        {
            tabDict.Add(item.state,item);
            item.UnToggle();
        }
        EnterState(State.FIRE);
    }

    public void EnterState(State state)
    {
        foreach (var item in tabs)
        {item.UnToggle();}
        tabDict[state].Toggle();
        currentState = state;
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("Backward"))
        {
            switch (currentState)
            {
                
                case State.FIRE:
                EnterState(State.SKILL);
                break;
                case State.SKILL:
                EnterState(State.ITEMS);
                break;
                case State.ITEMS:
                EnterState(State.FIRE);
                break;
            }
        }


        if(Input.IsActionJustPressed("Forward"))
        {
            switch (currentState)
            {
                
                case State.FIRE:
                EnterState(State.ITEMS);
                break;
                case State.ITEMS:
                EnterState(State.SKILL);
                break;
                case State.SKILL:
                EnterState(State.FIRE);
                break;
            }
        }

        if(InputManager.inst.strikeDown){
            if(currentAimState == AimState.CHARACTERS){
                CameraTargetLookAt.inst.ChangeFOV(30);
                   GD.Print(CameraTargetLookAt.inst.currentTarget() .Name);
                Zombie z = CameraTargetLookAt.inst.currentTarget() as Zombie;
                CameraTargetLookAt.inst.EnterLookAt<WeakPoint>(z.weakPoints,false  );
                
            }
            else{
                ConfirmState();
            }
            
        }
    }


    public void ConfirmState()
    {
        switch (currentState)
            {
                
                case State.FIRE:
                CameraTargetLookAt.inst.EnterLookAt<Enemy>(BattleManager.inst.enemies.ToList(),true);
                currentAimState = AimState.CHARACTERS;
                GD.Print("Fire");
                break;
                case State.ITEMS:
                GD.Print("Items");
                break;
                case State.SKILL:
                GD.Print("Skill");
                break;
            }
    }
}
