using Godot;
using System;

public partial class HeadBob : Node3D
{

    [Export] public float timer,bobbingSpeed,bobbingAmount;
    float xInit, yInit,xOffset, yOffset,bobSpeed,bobAmp;
    float deltaTime;
    public override void _Ready()
    {
        base._Ready();
        xInit = Position.X;
        yInit = Position.Y;
        // xOffset = xInit;
        // yOffset = yInit;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        deltaTime = (float)delta;
    }

    public void Bob(Vector2 input)
    { 
       
        float xMovement = 0.0f;
        float yMovement = 0.0f;
        float horizontal = input.X;
        float vertical = input.Y;

        Vector3 calcPosition = Position;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            xMovement = Mathf.Sin(timer) / 2;
            yMovement = -Mathf.Sin(timer);
            timer += bobbingSpeed;
            if (timer > Mathf.Pi * 2)
            {
                timer = timer - (Mathf.Pi * 2);
            }
        }

        if (xMovement != 0)
        {
            float translateChange = xMovement * bobbingAmount ;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;

            calcPosition.X = xOffset + translateChange ;
        }
        else
        {
            calcPosition.X = xOffset;
        }

        if (yMovement != 0)
        {
            float translateChange = yMovement * bobbingAmount ;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange ;

            calcPosition.Y = yOffset + translateChange ;
        }
        else
        {
            calcPosition.Y = yOffset;
        }

       Position = Position.Lerp(calcPosition,deltaTime);
    }
}