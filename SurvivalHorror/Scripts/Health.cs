using System.Collections;
using System.Collections.Generic;
using Godot;

[System.Serializable]
public partial class Health : Node3D
{
    [Signal] public delegate void OnHitEventHandler();
    [Signal] public delegate void OnHealEventHandler();
    [Signal] public delegate void OnDieEventHandler();
    [Signal] public delegate void OnInitEventHandler();
    [Export] public float maxHealth,currentHealth;
 
    public bool hasDied;

    public override void _Ready()
    {AssignHealth(maxHealth);}

    public void AssignHealth(float maxHealth_)
    {
        maxHealth = maxHealth_;
        currentHealth = maxHealth;
        EmitSignal(SignalName.OnInit);
    }
    
    public void Heal(float healAmount)
    {
        if(currentHealth < maxHealth)
        {
            float heal = Mathf.Min(maxHealth -  currentHealth, healAmount);
            currentHealth = currentHealth + heal;
            EmitSignal(SignalName.OnHeal);
        }
    }
    
    public void Damage(float damage)
    {
        if(currentHealth > 0)
        {
            GD.Print("Hit");
           
            currentHealth = currentHealth - damage;
            EmitSignal(SignalName.OnHit);
            if(currentHealth <= 0)
            {Die();}
        }
        else if(currentHealth <= 0)
        {Die();}
    }
    
    public void Die()
    {
        hasDied = true;
        EmitSignal(SignalName.OnDie);
        GD.Print("I AM DEAD!");
   
    }
}
