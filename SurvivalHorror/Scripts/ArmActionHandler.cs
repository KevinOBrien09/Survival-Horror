// using Godot;
// using System;

// public partial class ArmActionHandler : Node3D
// {
//     public enum ArmState{IDLE,SWAY,SWAP,JUMP}

//     [Export]public AnimationTree tree;
//     [Export] public AnimationPlayer player;
//     [Export] float blendSpeed = 15;
//     public ArmState currentState;
//     float sway,swap,jump;
//     public float swayTarget;
//     float deltaTime;



//     public void SwapState(ArmState newState){
//         currentState = newState; 
//         HandleAnims();

    
//     }
//     public override void _Process(double delta)
//     {
//         deltaTime = (float)delta;
//     }
//     void HandleAnims()
//     {  float fastBlend = blendSpeed *3;
//         switch(currentState)
//         {
//             case ArmState.IDLE:
          
//             sway = Mathf.Lerp(sway,0,blendSpeed  * deltaTime);
//             swap = 0;
//             jump = Mathf.Lerp(jump,0,blendSpeed  * deltaTime);
//             break;
//             case ArmState.SWAY:
          
//             sway = Mathf.Lerp(sway,swayTarget,blendSpeed * deltaTime);
//             swap = 0;
//             jump = Mathf.Lerp(jump,0,blendSpeed * deltaTime);

//             break;
//             case ArmState.JUMP:
          
//             sway = Mathf.Lerp(sway,0,fastBlend  * deltaTime);
//             swap = 0;
//             jump = Mathf.Lerp(jump,1,fastBlend * deltaTime);
//             break;
//             case ArmState.SWAP:
//             tree.Set("parameters/SwapAnim/time",0);
             
//             sway = 0;
//             swap = 1;
//             jump = 0;

//             break;
//             default:
     
//             sway = Mathf.Lerp(sway,0,blendSpeed * deltaTime);
//             swap = 0;
//             jump = Mathf.Lerp(jump,0,blendSpeed * deltaTime);
//             break;
//         }

//         UpdateTree();
//     }

//     void UpdateTree(){
  
//         tree.Set("parameters/Sway/blend_amount",sway);
//         // tree.Set("parameters/Jump/blend_amount",jump);
//         // tree.Set("parameters/Swap/blend_amount",swap);

        
//     }




// }