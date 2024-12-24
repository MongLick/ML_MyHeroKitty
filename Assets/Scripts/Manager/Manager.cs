using UnityEngine;

public static class Manager
{
	public static PoolManager Pool { get { return PoolManager.Instance; } }
	public static GameManager Game { get { return GameManager.Instance; } }
	public static MonsterManager Monster { get { return MonsterManager.Instance; } }
	public static DataManager Data { get { return DataManager.Instance; } }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		PoolManager.ReleaseInstance();
		PoolManager.CreateInstance();

		GameManager.ReleaseInstance();
		GameManager.CreateInstance();

		MonsterManager.ReleaseInstance();
		MonsterManager.CreateInstance();

		DataManager.ReleaseInstance();
		DataManager.CreateInstance();
	}
}
