using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
	[Header("Components")]
	[SerializeField] List<Monster> monsters = new List<Monster>();
	public List<Monster> Monsters { get { return monsters; } }

	public void AddMonster(Monster monster)
	{
		monsters.Add(monster);
	}

	public void RemoveMonster(Monster monster)
	{
		monsters.Remove(monster);
	}

	public Monster GetClosestMonster(Vector3 playerPosition)
	{
		Monster closestMonster = null;
		float closestDistance = Mathf.Infinity;

		foreach (Monster monster in monsters)
		{
			if (monster == null) continue;
			if (monster.IsDead) continue;

			float distance = Vector3.Distance(playerPosition, monster.transform.position);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestMonster = monster;
			}
		}

		return closestMonster;
	}
}
