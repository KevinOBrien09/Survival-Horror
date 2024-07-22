using Godot;
using System;
using System.Collections;
using Peaky.Coroutines;

public partial class WeaponManager : Node3D
{
    public static WeaponManager inst;
    [Export] Breathing breathing;
    [Export] Weapon[] weapons;

    [Export] public Node3D recoil;
    [Export] Camera3D firstPerson,thirdPerson;
    [Export] ThirdPersonCamera thirdPersonCamera;
    public Weapon currentWeapon;
    [Export] float zoomFOV;
    [Export] Node3D cameraAim;
    [Export] Node3D model;
    [Export] AnimationTree animationTree;
    [Export] SoundData[] aimSFX;
    public bool weaponIsRaised;
    [Export] float aimSens;
    bool inTrans;
    float ogFOV;
    Vector2 cameraInput;
    public override void _Ready()
    {
        inst = this;
        ogFOV = firstPerson.Fov;
        foreach (var item in weapons)
        {
            item.Disable();
        }
        thirdPerson.Current = true;
        firstPerson.Current = false;
        Visible = false;
        SetWeapon(weapons[0]);
        SetSensitivity(aimSens);
        cameraAim.Set("firstPerson",false);
        Coro.Run(q(),this);
        IEnumerator q()
        {
            yield return new WaitOneFrame();
            CameraShake.inst.SwapCamera(thirdPerson.GetParentNode3D());
        }
    }

    public void SetWeapon(Weapon w){
        currentWeapon = w;
        currentWeapon.Lower();
        currentWeapon.Activate();
    }

    public void SetSensitivity(float sens){
        var s = sens/1000;
        if(InputManager.inst.currentController == InputManager.ControlType.PAD)
        {
           
            cameraAim.Set("sens",s*50);
        }
        else
        {
           
            cameraAim.Set("sens",s);
        }
        
 
    }

  

    public override void _Process(double delta)
    {
       
        if(inTrans)
        {return;}  
        SetSensitivity(aimSens);
        if(InputManager.inst.raiseWeapon && !weaponIsRaised)
        {
            inTrans = true;
            Player.inst.SetPhysicsProcess(false);
            animationTree.Set("parameters/conditions/aim",true);
            animationTree.Set("parameters/conditions/noAim",false);
            BlackFade.inst.BlindsIn(
                ()=>
                {
                    //AudioManager.inst.Play(aimSFX[0],AudioType.FLAT);
                    currentWeapon.Rise();   
                    breathing.SetProcess(true);
                    Tween c = CreateTween();
                    
                    cameraAim.Rotation = new Vector3(Player.inst.thirdPersonCamera.Rotation.X,0,0);
                    CursorManager.inst.Visible = true;
                    Visible = true;
                    c.Finished += ()=>{   };
                    c.TweenProperty(firstPerson,"fov",zoomFOV,.1f);
                    Player.inst.GlobalRotationDegrees= new Vector3(0,Player.inst. thirdPersonCamera.GlobalRotationDegrees.Y+180,0);
                    cameraAim.Set("firstPerson",true);
                    thirdPerson.Current = false;
                    firstPerson.Current = true;
                    CameraShake.inst.SwapCamera(firstPerson.GetParentNode3D());
                    model.Visible = false;
                    BlackFade.inst.BlindsOut(
                    ()=>
                    {
                        inTrans  = false;
                        weaponIsRaised = true;
                    },.1f);
                },.1F
            );
           
        }
        else if(!InputManager.inst.raiseWeaponHold && weaponIsRaised)
        {  inTrans = true;
            BlackFade.inst.BlindsIn(
                ()=>
                {
                    //AudioManager.inst.Play(aimSFX[1],AudioType.FLAT);
                    animationTree.Set("parameters/conditions/aim",false);
                    animationTree.Set("parameters/conditions/noAim",true);
                    Player.inst.SetPhysicsProcess(true);
                    currentWeapon.Lower();
                    Tween c = CreateTween();
                    Visible = false;
                    CursorManager.inst.Visible = false;
                    cameraAim.Set("firstPerson",false);
                    c.Finished+=()=>{};
                    c.TweenProperty(firstPerson,"fov",ogFOV,.1f);
                    //var cr = thirdPersonCamera.target.Rotation;
                    thirdPersonCamera.target.Rotation = Vector3.Zero;
            
                    breathing.SetProcess  (false);
                    breathing.Kill();
                    breathing.RotationDegrees = Vector3.Zero;
                    thirdPerson.Current = true;
                    firstPerson.Current = false;
                    CameraShake.inst.SwapCamera(thirdPerson.GetParentNode3D());
                    model.Visible = true;

                       BlackFade.inst.BlindsOut(
                    ()=>
                    { 
                     
                        inTrans  = false;
                      weaponIsRaised = false;
                    },.15F);
                },.15F
            );
            
        }

        if(weaponIsRaised)
        {
            if(InputManager.inst.strikeDown)
            { currentWeapon.Strike(); }
        }
    }

    public void Rise(){

    }

    public void Lower(){
        
    }

    public void Swap(){

    }
    
}