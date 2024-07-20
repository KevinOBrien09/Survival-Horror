// using Godot;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Peaky.Coroutines;
// public partial class SlashEffect : Control
// {
//     public static SlashEffect inst;
//     public Knife currentSlashWeapon;
//     [Export] SoundData swipe;
//     [Export] Trail trail;
//     [Export] float staminaCost;
//     [Export] float coolDown;
//     [Export] SlashRay ray;
//     [Export] float knifeRange;
//     Vector2 previousPosition;
//     float deltaTime;
//     public bool cutting,onCooldown,needToLiftKey,drawTrail,landedHit;
//     public List<float> velocities = new List<float>();

//     public override void  _Ready(){
//         inst = this;
//     }

   
//     public override void _UnhandledInput(InputEvent @event)
//     {
//         if(!Player.inst.stamina.isTired(staminaCost))
//         {
//             cutting = InputManager.inst.strikeHold;
//             if(InputManager.inst.strikeDown)
//             {
//                 InputManager.inst.ChangePadMouseSens(70);
//                 StartCut();
//             }
//         }
//         else
//         {
//             InputManager.inst.ResetPadMouseSens();
//             if(InputManager.inst.strikeDown)
//             {Player.inst.stamina.RedFlash();
//             trail.Wipe();}
//         }

//         if(InputManager.inst.strikeUp)
//         {
//             InputManager.inst.ResetPadMouseSens();
//             EndCut();
    
//         }
//     }

//     public void CoolDown()
//     {
//         currentSlashWeapon.SlashGraphic(coolDown);
//         Coro.Run(q(),this);
//         IEnumerator q(){
//             yield return new WaitForSeconds(.1f);
//             drawTrail = false;
//         }   
//     }

//     public void StartCut()
//     {   
//         cutting = true;
//         drawTrail = true;
//         trail.Wipe();
//         ray.Enabled = false;
//         previousPosition = Position;
//         velocities.Clear();
//     }

//     void UpdateCut()
//     {  
//         if(!onCooldown ) // && !needToLiftKey
//         {
//             var mousePos = GetViewport().GetMousePosition() ;
//             Position = mousePos;
           
//             if(validCut())
//             {
//                 AudioManager.inst.Play(swipe,AudioType.FLAT);
//                 Player.inst.stamina.Spend(staminaCost);
//                 ray.Enabled = true;
//                 CoolDown();
//                 velocities.Clear();
                
//             }
//             else
//             {
//                 ray.Enabled = false;
//             }
//             previousPosition = Position;
//         }
//     }


//     public bool validCut()
//     {
//         var mousePos = GetViewport().GetMousePosition() ;
//         float velocity =  (mousePos - previousPosition).Length() * deltaTime;
//         var cutThreshold = 1f;
//         velocities.Add(velocity);
//         return velocity >= cutThreshold && velocities.Count >= 3;
//     }
    
//     void EndCut()
//     {
//         needToLiftKey = false;
//         drawTrail = false;
//         //trail.Wipe();
//         velocities.Clear();
//         landedHit = false;

//         if(Player.inst.stamina.isTired(staminaCost))
//         {
//             trail.Wipe();
//         }
        
//        // cutting = false;
//     }

//     public override void _Process(double delta)
//     {
//         deltaTime = (float)delta;

//         if(cutting && drawTrail)
//         {
//             trail.Collect();
//         }
//          else
//         {
//            trail.Wipe();
//         }
        
//         if(!Player.inst.stamina.isTired(staminaCost))
//         {
//             if(cutting && !onCooldown)
//             {
//                 UpdateCut();
//             }
//         }
//         else
//         {
//            trail.Wipe();
//         }
        
//     }
// }