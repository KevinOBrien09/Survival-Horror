using Godot;
using System;
using Peaky.Coroutines;
using System.Collections;
using System.Reflection;

public partial class WorldSoundEffect : AudioStreamPlayer3D
{
   
    public virtual void Go(SoundData soundData,Vector3 worldPoint = default){
        SetAndPlay(soundData);
        Position = worldPoint;
    }

    public void Reset(){
        Stream = null;
        Position = Vector3.Zero;
       
    }

    public void SetAndPlay(SoundData soundData){
        PitchScale = soundData.GetRandomPitch();
        Stream = soundData.file;
        VolumeDb = soundData.volume;
        Play(); 

        // Finished += ()=>
        // {
        //     GD.Print("Played: " + Stream.ResourceName);
        // };
    }
}



