using Godot;
using System;
using System.Collections;
using Peaky.Coroutines;
public partial class Rifle : Weapon
{
    [Export] Vector3 lowRot,lowPos,riseRot,risePos;
    [Export] AnimationPlayer anim;
    [Export] SoundData fire;
     bool canShoot;
    public override void Rise()
    {
     
        Tween r = CreateTween();
        Tween p = CreateTween();
        r.TweenProperty(this,"rotation_degrees",riseRot,.1f);
        p.TweenProperty(this,"position",risePos,.1f);
        p.Finished += (()=>{
            Position = risePos;
            RotationDegrees = riseRot;
            canShoot = true;
        });

    }

    public override void Strike(){
        if(canShoot){
            canShoot = false;
            anim.Play("Fire");
       
           
        }
        else{
            GD.Print("cool down");
        }
       
    }

    public void ExitShoot(){
    anim.Play("Idle");
                canShoot = true;
    }

    public void Fire(){
        AudioManager.inst.Play(fire,AudioType.FLAT);
        CameraShake.inst.Shake(.3f,.1f);
        WeaponManager.inst.recoil.Call("recoilFire");
        Scan();
    }
   
    public override bool isLowered()
    { return Position == lowPos; }

    public override void Lower()
    {
        Tween r = CreateTween();
        Tween p = CreateTween();
        r.TweenProperty(this,"rotation_degrees",lowRot,.1f);
        p.TweenProperty(this,"position",lowPos,.1f);
        p.Finished += (()=>{
            canShoot = false;
            Position = lowPos;
            RotationDegrees = lowRot;
       });
    }
}