using Godot;
using System;
using System.Collections.Generic;
public partial class BattleManager : Node
{
	public static BattleManager inst;
	[Export] PackedScene enemyTest;
	[Export] EnemySpawn[] spawns;
	public List<Enemy> enemies = new List<Enemy>();
	public override void _Ready()
	{
		inst = this;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		SpawnEnemies();
	}

	public void SpawnEnemies(){
		for (int i = 0; i < 3; i++)
		{
			Enemy e =	spawns[i].Spawn(enemyTest);
			enemies.Add(e);
		}
	}
}
