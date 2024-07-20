using Godot;
using System;
using System.Collections.Generic;
public enum AudioType{WORLD,FLAT}
public partial class AudioManager : Node3D
{
    public static AudioManager inst;
    [Export]public bool mute;
    [Export]public PackedScene flatAudioPrefab;
    [Export] public PackedScene worldAudioPrefab;
    [Export] float howManyInstances;
    List<WorldSoundEffect> worldList =  new List<WorldSoundEffect>();
    List<FlatSoundEffect> flatList =  new List< FlatSoundEffect>();
    Queue<WorldSoundEffect> worldQ = new Queue<WorldSoundEffect>();
    Queue<FlatSoundEffect> flatQ = new Queue<FlatSoundEffect>();

    public override void _Ready()
    {
        if(inst != null){
            GD.PrintErr("TWO AUDIO MANAGERS!!!!");
        }
        inst = this;
        for (int i = 0; i < howManyInstances; i++)
        {
            var f = flatAudioPrefab.Instantiate() as FlatSoundEffect;
            flatList.Add(f);
            AddChild(f);
        }
        for (int i = 0; i < howManyInstances; i++)
        {
            var w = worldAudioPrefab.Instantiate() as WorldSoundEffect;
            worldList.Add(w);
            AddChild(w);
        }
    }

    public FlatSoundEffect GetFlat()
    {
        if(flatQ.Count > 0)
        {
            return flatQ.Dequeue();
        }
        else
        {
            

            foreach (var item in flatList)
            {
                flatQ.Enqueue(item);
             
            }
            return GetFlat();
        }
    }
    
    public WorldSoundEffect GetWorld()
    {
        if(worldQ.Count > 0)
        {
           return worldQ.Dequeue();
        }
        else
        {
            foreach (var item in worldList)
            {
                worldQ.Enqueue(item);
                item.Reset();
            }
            return GetWorld();
        }
    }
    public void Play(SoundData soundData,AudioType type,Vector3 point = default){
        if(mute){
            return;
        }
        switch (type)
        {
            case AudioType.WORLD:
            GetWorld().Go(soundData,point);
            return;

            case AudioType.FLAT:
            GetFlat().Go(soundData);
            return;
            
            default:
            GD.PrintErr("DEFAULT CASE CALLED IN AUDIOMANAGER!!!");
            GetWorld().Go(soundData,point);
            return;
        }
    }
}