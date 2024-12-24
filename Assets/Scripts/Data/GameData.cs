using System;
using UnityEngine;

[Serializable]
public class GameData
{
	[Header("Specs")]
	public float bossTime;
	public int stage;
	public int deadMonster;
	public int maxDeadMonster;
	public bool isBossTry;
	public bool isBossDefeated;
}
