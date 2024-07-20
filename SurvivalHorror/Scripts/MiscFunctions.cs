using Godot;
using System;
using Peaky.Coroutines;
using System.Collections.Generic;
using System.Collections;

public static class MiscFunctions
{
    public static bool FiftyFifty(){
        RandomNumberGenerator rng = new RandomNumberGenerator();
        int i = rng.RandiRange(0,1);
        return i == 0;
    }


    public static Vector3 RandomSpherePos(float radius,Vector3 point){
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        var u = rng.Randf();
        var v = rng.Randf();
        var theta = 2 * Math.PI * u;
        var phi = Math.Acos(2 * v - 1);
        var x = point.X + (radius * Math.Sin(phi) * Math.Cos(theta));
        var y = point.Y + (radius * Math.Sin(phi) * Math.Sin(theta));
        var z = point.Z + (radius * Math.Cos(phi));
        return new Vector3((float) x,(float)y,(float)z);

    }


  
}