using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] PooledObject[] monsterPrefabs;
	[SerializeField] Transform bossSpawnPoint;
	[SerializeField] GameObject[] bossMonsterPrefabs;

	[Header("Vector")]
	[SerializeField] Vector2 spawnOffSetRange;

	private void Start()
	{
		Manager.Game.MonsterSpawner = this;
		for (int i = 0; i < monsterPrefabs.Length; i++)
		{
			Manager.Pool.CreatePool(monsterPrefabs[i], 10, 15);
		}
	}

	public void MonsterGetPool()
	{
		for (int i = 0; i < Manager.Game.MonsterCount; i++)
		{
			Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
			PooledObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
			Vector3 spawnPosition = spawnPoint.position + new Vector3(
				Random.Range(-spawnOffSetRange.x, spawnOffSetRange.x),
				Random.Range(-spawnOffSetRange.y, spawnOffSetRange.y),
				0f
			);
			PooledObject pooledMonster = Manager.Pool.GetPool(monsterPrefab, spawnPosition, Quaternion.identity);
			Monster monster = pooledMonster.GetComponent<Monster>();
			if (monster != null)
			{
				Manager.Monster.AddMonster(monster);
			}
		}
	}

	public void MonsterReturn()
	{
		foreach (Monster monster in Manager.Monster.Monsters)
		{
			if (monster != null)
			{
				PooledObject pooledMonster = monster.GetComponent<PooledObject>();
				if (pooledMonster != null)
				{
					pooledMonster.Release();
				}
				else
				{
					Destroy(monster.gameObject);
				}
			}
		}

		Manager.Monster.Monsters.Clear();
	}

	public void SpawnBoss()
	{
		MonsterReturn();
		GameObject bossPrefab = bossMonsterPrefabs[Manager.Data.GameData.stage - 1];
		GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
		Manager.Game.Boss = boss.GetComponent<BossMonster>();
	}
}
