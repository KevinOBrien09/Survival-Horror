using Godot;
using System;
using Peaky.Coroutines;
using System.Collections;
public partial class FlatSoundEffect : AudioStreamPlayer
{ 
    public virtual void Go(SoundData soundData,Vector3 worldPoint = default){
        SetAndPlay(soundData);
        
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