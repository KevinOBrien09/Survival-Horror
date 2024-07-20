using Godot;
using System;

public partial class InputManager : Node3D
{
    [Export] float defaultpadMouseSens = 10;
    public static InputManager inst;
	public Vector2 cameraInput;
	public Vector2 move;
	public bool pause,reload,inventory,strikeHold,block,interact,strikeUp,strikeDown,raiseWeapon,lowerWeapon,raiseWeaponHold;
	public float scrollWheel;
    public enum ControlType{MKB,PAD}
    public ControlType currentController;
	float padMouseSens;
    public override void _Ready(){
        if(inst != null){
            GD.PrintErr("TWO INPUT MANAGERS!!!");
        }
        inst = this;

    }

	public void ChangePadMouseSens(float newSens){
		padMouseSens = newSens;
	}

	public void ResetPadMouseSens(){
		padMouseSens = defaultpadMouseSens;
	}
	public override void _Input(InputEvent @event)
	{
      
		
		if(@event is InputEventMouseMotion mouseMotion)
		{cameraInput = mouseMotion.Relative;}
		else
		{cameraInput = Vector2.Zero;}
		
	}


	void GetInput(){
		float vertical = Input.GetAxis( "Forward", "Backward");
		float horizontal = Input.GetAxis( "Left", "Right");
		move  = new Vector2(horizontal,vertical);
       
		pause = Input.IsActionJustReleased("Pause");
		raiseWeapon = Input.IsActionJustPressed("Aim");
		lowerWeapon = Input.IsActionJustReleased("Aim");
		raiseWeaponHold = Input.IsActionPressed("Aim");
		interact = Input.IsActionJustPressed("Interact");
        strikeHold = Input.IsActionPressed("Strike");
		strikeDown = Input.IsActionJustPressed("Strike");
		strikeUp = Input.IsActionJustReleased("Strike");
        reload  = Input.IsActionJustPressed("Reload");
    
	}

    public void WarpMouse()
    {
        var direction = new  Vector2();
		var movement = new  Vector2();
        direction.X = Input.GetActionStrength("JoystickRight") - Input.GetActionStrength("JoystickLeft");
		direction.Y = Input.GetActionStrength("JoystickDown") - Input.GetActionStrength("JoystickUp");
        if (Mathf.Abs(direction.X) == 1 && Mathf.Abs(direction.Y) == 1)
        {direction = direction.Normalized();}
        movement = padMouseSens * direction ;
        GetViewport().WarpMouse(GetViewport().GetMousePosition() + movement);
    }


	public override void _Process(double delta)
	{
       
		GetInput();
        WarpMouse();
        
        
		if(Input.IsActionJustReleased("MouseWheelDown"))
		{
			scrollWheel = -1;
		}
		else if(Input.IsActionJustReleased("MouseWheelUp"))
		{
			scrollWheel = 1;
		}
		else
		{
			scrollWheel = 0;
		}
	}


}
