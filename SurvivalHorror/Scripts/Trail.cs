using Godot;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Peaky.Coroutines  ;
public partial class Trail : Line2D
{
    [Export] int maxLength;
    List<Vector2> q = new List<Vector2>();
    
    public void Collect()
    {
        var pos = GetGlobalMousePosition();
        q.Add(pos);
        if(q.Count > maxLength)
        { q.Remove(q.First()); }
           
        ClearPoints();
        foreach (var point in q)
        { AddPoint(point); }
    }

    public void Wipe()
    {
        q.Clear();
        ClearPoints();
    }
}