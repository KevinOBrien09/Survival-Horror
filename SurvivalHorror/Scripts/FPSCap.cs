using Godot;
using System;

public partial class FPSCap : RichTextLabel
{
	[Export] RichTextLabel textLabel;
	[Export] int targetFPS = 60;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Engine.MaxFps = targetFPS;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		textLabel.Text = "[right]fps:" +  Engine.GetFramesPerSecond().ToString();
	}
}
