using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
	[Header("UnityAction")]
	private UnityAction onBossTry;
	public UnityAction OnBossTry { get { return onBossTry; } set { onBossTry = value; } }
	private UnityAction onStage;
	public UnityAction OnStage { get { return onStage; } set { onStage = value; } }

	[Header("Components")]
	[SerializeField] MonsterSpawner monsterSpawner;
	public MonsterSpawner MonsterSpawner { get { return monsterSpawner; } set { monsterSpawner = value; } }
	[SerializeField] BossMonster boss;
	public BossMonster Boss { get { return boss; } set { boss = value; } }

	[Header("Specs")]
	[SerializeField] int monsterCount;
	public int MonsterCount { get { return monsterCount; } set { monsterCount = value; } }

	private void Start()
	{
		Manager.Data.GameData.stage = 1;
		Manager.Data.GameData.deadMonster = 0;
		Manager.Data.GameData.maxDeadMonster = 20;
		Manager.Data.GameData.isBossTry = false;
	}

	public void MonsterWaveStart()
	{
		monsterSpawner.MonsterGetPool();
	}

	public void CheckStageClear()
	{
		if (Manager.Data.GameData.deadMonster >= Manager.Data.GameData.maxDeadMonster && !Manager.Data.GameData.isBossTry)
		{
			Manager.Data.GameData.isBossTry = true;
			TryBoos();
		}
		else if (Manager.Data.GameData.isBossTry && Manager.Data.GameData.isBossDefeated)
		{
			RetryStage();
		}
	}

	public void TryBoos()
	{
		monsterSpawner.SpawnBoss();
		onBossTry?.Invoke();
	}

	public void RetryStage()
	{
		boss.BossDefeatFailed();
		boss = null;
		Manager.Data.GameData.deadMonster = 0;
		Manager.Data.GameData.isBossTry = false;
		Manager.Data.GameData.isBossDefeated = false;
		monsterSpawner.MonsterReturn();
		StartCoroutine(MonsterDelay());
	}

	public void NextStage()
	{
		onStage?.Invoke();
		boss = null;
		Manager.Data.GameData.stage++;
		Manager.Data.GameData.deadMonster = 0;
		Manager.Data.GameData.isBossTry = false;
		Manager.Data.GameData.isBossDefeated = false;
		StartCoroutine(MonsterDelay());
	}

	private IEnumerator MonsterDelay()
	{
		yield return new WaitForSeconds(1f);
		MonsterWaveStart();
	}
}
