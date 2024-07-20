using Godot;
using System;

public partial class InteractRay : RayCast3D
{
    [Export] SoundData grunt;
  
    Clickable currentHover;
 
    public  override void _Input(InputEvent @event)
	{
        if(Input.IsActionJustPressed("Interact"))
        {
            Interact();
        }
        if(IsColliding()){
            var n = GetCollider() as Node;
            if(n != null){
                var c = n.GetParent() as Clickable;
                //GD.Print(n.Name);
                if(c!=null){
                currentHover = c;
                currentHover.outLine.Visible = true;
                }
            }
            
        }
        else
        {
            if(currentHover != null){
            currentHover.outLine.Visible = false;
            currentHover = null;
            }
            
        }
    }

    public  void Interact()
    {
        if(IsColliding()){
        
            var n = GetCollider() as Node;

            var c = n.GetParent() as Clickable;
            //GD.Print(n.Name);
            if(c!=null){
                c.Go();
            }
            else{
              AudioManager.inst.Play(grunt,AudioType.FLAT);
                //GD.Print("Collision, but not interactable");
            }
        }
        else{
          //GD.Print("No Collision");
        }
    }
}