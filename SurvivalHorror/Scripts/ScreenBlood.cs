using Godot;
using System;
using System.Collections;
using Peaky.Coroutines;

public partial class ScreenBlood : TextureRect
{
 
    Tween tw;

    public override void _Ready()
    {
        tw = CreateTween();
        tw.TweenProperty(this,"modulate:a",0,0);
    }
    public void Refresh()
    {
        
        TweenAlpha(1);
    }

    void TweenAlpha(float target){
        
    
        tw = CreateTween();
        tw.TweenProperty(this,"modulate:a",target,.1f);
        tw.Finished+=()=>{
            Coro.Run(q(),this);
            IEnumerator q()
            {
                if(tw!=null)
                {
                    if(tw.IsRunning())
                    {tw.Kill();}
                }
       
                yield return new WaitForSeconds(.2f);
                tw = CreateTween();
                tw.TweenProperty(this,"modulate:a",0,1);
            }

        };
    }
}