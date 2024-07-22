using Godot;
using System;
using System.Collections;
using Peaky.Coroutines;

public partial class Hitmarker : TextureRect
{   
    public static Hitmarker inst;
    Tween hitTween;
   
    [Export] SoundData hitSFX,critSFX;
    public override void _Ready()
    {
        inst = this;
    }


    public void HitTween(bool wasCrit)
    {
        if(wasCrit)
        {
            AudioManager.inst.Play(hitSFX,AudioType.FLAT);
            Modulate = new Color(255,0,0,0);
        }
        else
        {
            AudioManager.inst.Play(critSFX,AudioType.FLAT);
             Modulate = new Color(255,255,255,0);
        }

        hitTween = CreateTween();
        hitTween.TweenProperty(this,"modulate:a",1,0);
        hitTween.Finished += ()=>
        {
          
            if(!hitTween.IsRunning()){
                hitTween = CreateTween();
                hitTween.TweenProperty(this,"modulate:a",0,.2f);
            }
            
            
          
           
        };
        
    }
}