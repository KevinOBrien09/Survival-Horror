using Godot;
using System;
using GodotPlugins;
using Peaky.Coroutines;

using System.Collections;
public static class Coro
{
    #pragma warning disable 4014
    public static void Run(IEnumerator coroutine, Node runner){
	    Coroutine.Run(coroutine,runner);
    }
    #pragma warning disable 4014
}