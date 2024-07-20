using Godot;
using System;

[GlobalClass] 
public partial class SoundData :Resource
{
    [Export] public bool isWorldSource;
    [Export] public AudioStreamMP3 file;
    [Export] public Vector2 pitchRange = new Vector2(.9f,1.1f);
    [Export] public float volume = 1;

    public float GetRandomPitch(){
        RandomNumberGenerator rng = new RandomNumberGenerator();
        return rng.RandfRange(pitchRange.X,pitchRange.Y);
    }
       
}