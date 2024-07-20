using Godot;
using System;
using System.Collections;
using Peaky.Coroutines;

public partial class BlackFade : ColorRect
{
    public static BlackFade inst;

    [Export] ColorRect b_top,b_bot;

    public override void _Ready()
    {
        inst = this;
        Color = new Color(0,0,0,0);
        b_top.Visible = false;
        b_bot.Visible = false;
    }
    

    public void FadeIn(Action a,float time = .25f){
        Tween t = CreateTween();
        t.TweenProperty(this,"color:a",1,time);
        t.Finished += a.Invoke;
    }   

    public void FadeOut(Action a,float time = .25f){
        Tween t = CreateTween();
        t.TweenProperty(this,"color:a",0,time);
        t.Finished += a.Invoke;
    }

    public void BlindsIn(Action act,float time = .25f){
        Tween a = CreateTween();
        a.Finished+=()=>
        {
            act.Invoke();
        };
        Tween b = CreateTween();
        Vector2 screen = GetViewport().GetVisibleRect().Size;
        b_top.Visible = true;
        b_bot.Visible = true;
        b_top.Position = new Vector2(0,-screen.Y/2);
        b_bot.Position = new Vector2(0,screen.Y);
        a.TweenProperty(b_top,"position:y",0,time);
        b.TweenProperty(b_bot,"position:y",screen.Y/2,time);
    }

    public void BlindsOut(Action act,float time = .25f){
        Tween a = CreateTween();
        a.Finished+=()=>
        {
            act.Invoke();
            b_top.Visible = false;
            b_bot.Visible = false;
        };
        Tween b = CreateTween();
        Vector2 screen = GetViewport().GetVisibleRect().Size;
        b_top.Visible = true;
        b_bot.Visible = true;
        b_top.Position = new Vector2(0,0);
        b_bot.Position = new Vector2(0,screen.Y/2);
        a.TweenProperty(b_top,"position:y",-screen.Y/2,time);
        b.TweenProperty(b_bot,"position:y",screen.Y,time);
    }
}