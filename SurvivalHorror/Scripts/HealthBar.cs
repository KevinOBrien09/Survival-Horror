using Godot;
using System;
using System.Collections.Generic;
using Peaky.Coroutines;

public partial class HealthBar : Control
{
    [Export] PackedScene healthPip;
    [Export] Health health;
    List<Control> pips = new List<Control>();
    public void Spawn()
    {
        foreach (var item in pips)
        { item.QueueFree(); }
        pips.Clear();

        for (int i = 0; i < health.maxHealth; i++)
        {
            Control pip = healthPip.Instantiate() as Control;
            GetChild(0).AddChild(pip);
            pips.Add(pip);
        }
        pips.Reverse();
    }

    public void Refresh()
    {
        foreach (var item in pips)
        {
            Control colour = item.GetChild(1) as Control;
            colour.Visible = false;
        }
        for (int i = 0; i < health.currentHealth; i++)
        {
            Control colour = pips[i].GetChild(1) as Control;
            colour.Visible = true;
        }
    }
}