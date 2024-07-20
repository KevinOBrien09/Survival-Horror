  // IEnumerator BackDash(float _dashTime, float _dashSpeed,Vector3 _dashDir)
    // {   
        
    //     isDashing = true;
    //     dashSpeed = _dashSpeed;
    //     Vector2 move = InputManager.inst.move;
    //     Vector3 inputDir = ( GlobalTransform.Basis * new Vector3(move.X, 0, move.Y)).Normalized();
    //     if(move != Vector2.Zero)
    //     { dashDir = inputDir; }
    //     else
    //     { dashDir = _dashDir;
    //     cameraDash.Back(_dashTime);}
    //     if(move ==  new Vector2(0,1)) //backward
    //     { cameraDash.Back(_dashTime); }
    //     else if(move ==  new Vector2(-1,1)) //back left
    //     { cameraDash.BackwardLeft(_dashTime);}
    //     else if(move ==  new Vector2(1,1)) //back right
    //     {cameraDash.BackwardRight(_dashTime);}
    //     else if(move ==  new Vector2(1,0)) //right
    //     {cameraDash.Right(_dashTime);}
    //     else if(move ==  new Vector2(-1,0)) //left
    //     {cameraDash.Left(_dashTime);} 
    //     else if(move ==  new Vector2(0,-1)) //forward
    //     {
    //         cameraDash.Back(_dashTime);
    //         dashDir = _dashDir;
    //     }
    //     else if(move ==  new Vector2(-1,-1)) //forward left
    //     { 
    //         dashDir = ( GlobalTransform.Basis * new Vector3(-1, 0, -1)).Normalized();
    //         cameraDash.Left(_dashTime);
    //     }
    //     else if(move ==  new Vector2(1,-1)) //forward right
    //     {   
    //         dashDir = ( GlobalTransform.Basis * new Vector3(1, 0, -1)).Normalized();
    //         cameraDash.Right(_dashTime);
    //     }
    //     yield return new WaitForSeconds(_dashTime);
    //     isDashing = false;
    //     dashDir = Vector3.Zero;
    //     dashSpeed = 0;
    // }
