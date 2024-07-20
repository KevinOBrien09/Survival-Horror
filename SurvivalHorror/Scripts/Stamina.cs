using Godot;
using System;
using System.Collections;
using Peaky.Coroutines;

public partial class Stamina : Node3D
{
    [Export] float current,max,tickRate;
    [Export] TextureProgressBar fill;
    [Export] TextureRect border,red;
	Tween tw;
    IEnumerator q;
    float timeStamp;
    bool runRegen;
    Tween redflash;
    Color OGred,hiddenRed;
    bool canRedFlash;
    public override void  _Ready(){
        OGred  = red.Modulate;
        hiddenRed = OGred;
        hiddenRed.A = 0;
        red.Modulate =  hiddenRed;
        current = max;
        canRedFlash = true;
        RefreshUI()	;
	}

	public void Spend(float cost)
	{
      
        runRegen = false;
         bool fatigue =  isTired(cost);
		for (int i = 0; i < cost; i++)
		{
			if(current > 0)
            { current --; }
        }
        current = Mathf.Clamp(current,0,100);
        
		if(fatigue)
		{RedFlash();}
        RefreshUI();
        if(q == null)
        { 
            q = Regen(fatigue);
            Coro.Run(q,this);
        }
       
    }

    public override void _Process(double delta)
    {
        
        if(current < max)
        {
            if(runRegen)
            {
                if(timeStamp <= Time.GetTicksMsec())
                {
                    current += .2f;
                    current = Mathf.Clamp(current,0,100);
                    RefreshUI();
                    //GD.Print("Timestamp: " + timeStamp + "Time: " + Time.GetTicksMsec() );
                    timeStamp = Time.GetTicksMsec()+ tickRate;
                }
            }
        }
    }

	IEnumerator Regen(bool fatigue)
	{
        RandomNumberGenerator rng = new RandomNumberGenerator();
        int i =  rng.RandiRange(0,1000);
		if(fatigue)
        {
            RedFlash();
			yield return new WaitForSeconds(5f);
		}
		else
        {
            yield return new WaitForSeconds(2f);
		}
        runRegen = true;
        q = null;
    }
    
    public void Refund(float a){
		current += a;
		current = Mathf.Clamp(current,0,max);
		RefreshUI();
	}

	public void RedFlash()
    {
        if(canRedFlash){
            canRedFlash = false;
            red.Modulate = OGred;
        
            Coro.Run(q(),this);
            IEnumerator q(){
                yield return new WaitForSeconds(.5f);
                Tween t = CreateTween();
                t.Finished +=()=>{canRedFlash = true;};
                t.TweenProperty(red,"modulate", hiddenRed,.5f);
            }
        }
      
    }
    
    public bool isTired(float cost)
    {
        return current < cost;
    }
    
    public void RefreshUI()
    {  
        Tween t = CreateTween();
        t.TweenProperty(fill,"value",current,.1f);
    }

}