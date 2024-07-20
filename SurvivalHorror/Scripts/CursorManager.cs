using Godot;
using System;

public partial class CursorManager : TextureRect
{
    public static CursorManager inst;
    public bool spin;
    public override void _Ready(){
        if(inst != null){
            GD.PrintErr("TWO CURSOR MANAGERS!!!");
        }
        inst = this;
        Visible = false;
        ChangeCursorState(Input.MouseModeEnum.Captured);
    }

    public override void _Process(double delta)
	{
        // Vector2 v = GetViewport().GetMousePosition();
        // v -=  PivotOffset;
        // Position =v ;
        Rotation += (float) delta;
    }

    public void ChangeCursorState(Input.MouseModeEnum newState){
            Input.MouseMode = newState;
    }

    public void ChangeCursor(Texture2D text,bool _spin){
        Texture = text;
        spin = _spin;
    }
}