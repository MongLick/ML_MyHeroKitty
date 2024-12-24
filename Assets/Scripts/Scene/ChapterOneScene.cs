using System.Collections;
using UnityEngine;

public class ChapterOneScene : BaseScene
{
	private void Start()
	{
		StartCoroutine(LoadingRoutine());
	}

	public override IEnumerator LoadingRoutine()
	{
		yield return new WaitForSeconds(1f);
		Manager.Game.MonsterWaveStart();
	}
}
