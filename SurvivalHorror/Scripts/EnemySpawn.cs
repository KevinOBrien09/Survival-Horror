using Godot;
using System;
using System.Collections.Generic;
using Peaky.Coroutines;
using System.Collections;
public partial class EnemySpawn : Node3D
{
    public Enemy enemy;
    public bool filled;
    public Enemy Spawn(PackedScene scene)
    {
        enemy = scene.Instantiate() as Enemy;
        
        Coro.Run(q(),this);
        IEnumerator q(){
            yield return new WaitOneFrame(); 
            GetTree().Root.AddChild(enemy);
         
            // yield return new WaitOneFrame(); 
            // yield return new WaitOneFrame();
            //  yield return new WaitOneFrame(); 
            // yield return new WaitOneFrame();
            //  yield return new WaitOneFrame(); 
            // yield return new WaitOneFrame();
            enemy.GlobalPosition = GlobalPosition;
            enemy.GlobalRotationDegrees =GlobalRotationDegrees;
            GD.Print(enemy.GlobalPosition);
             GD.Print(GlobalPosition);
        }
       
        filled = true;
        return enemy;
    }
}