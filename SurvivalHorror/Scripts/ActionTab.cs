using Godot;
using System;

public partial class ActionTab : Control
{
    [Export] public ActionMenu.State state;
    [Export] Control dot;
    public void Toggle(){
        dot.Visible = true;
    }

    public void UnToggle(){
        dot.Visible = false;
    }
}