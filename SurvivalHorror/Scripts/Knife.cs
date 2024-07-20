using Godot;
// using System;
// using System.Collections;
// using Peaky.Coroutines;
// public partial class Knife : Weapon
// {   
//     [Export] SlashRay ray;
//     [Export] Vector3 downPos,slashRot;
//     [Export] Node3D parent;
//     Vector3 ogPos,ogRot;
//     public override void  _Ready(){
      
//         ogPos = Position;
//         ogRot = Rotation;
//     }

//     public override void Activate(){
//         SlashEffect.inst.currentSlashWeapon = this;
//         foreach (var item in parent.GetChildren())
//         {
//             item.SetProcess(true);
//             item.SetPhysicsProcess(true);
//         }
//         ray.SetProcess(true);
//         SlashEffect.inst.SetProcessUnhandledInput(true);
//         SlashEffect.inst.SetProcess(true);
//         Visible = true;
//     }

//     public override void Disable(){
//         SlashEffect.inst.currentSlashWeapon = null;
//         foreach (var item in parent.GetChildren())
//         {
            
//             item.SetProcess(false);
//             item.SetPhysicsProcess(false);
//         }
//         ray.SetProcess(false);
//         SlashEffect.inst.SetProcessUnhandledInput(false);
//         SlashEffect.inst.SetProcess(false);
//         Visible = false;
//     }
    
//     public void SlashGraphic(float coolDown)
//     {
//         Visible = false;
//         Position = downPos;
//         RotationDegrees = slashRot;
//         SlashEffect.inst.onCooldown = true; 
//         float wait = coolDown * 80 / 100;
//         float rise = coolDown * 20 / 100;
//         Coro.Run(Q(),this);
//         IEnumerator Q(){
//             yield return new WaitForSeconds(wait);
       
//             Visible = true;
//             Tween t = CreateTween();
//             Tween a = CreateTween();
//             a.TweenProperty(this,"rotation_degrees",ogRot,.3f);
//             t.TweenProperty(this,"position",ogPos,.3f);
//             t.Finished+=()=>{
//                 SlashEffect.inst.onCooldown = false; 
//                 SlashEffect.inst.drawTrail = true;
//             };
//         }

//     }
// }