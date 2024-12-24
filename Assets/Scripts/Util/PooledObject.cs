using System.Collections;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
	[Header("Components")]
	private ObjectPool pool;
	public ObjectPool Pool { get { return pool; } set { pool = value; } }

	[Header("Specs")]
	[SerializeField] float releaseTime;
	[SerializeField] bool autoRelease;

	private void OnEnable()
	{
		if (autoRelease)
		{
			StartCoroutine(ReleaseRoutine());
		}
	}

	public void Release()
	{
		if (pool != null)
		{
			pool.ReturnPool(this);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	IEnumerator ReleaseRoutine()
	{
		yield return new WaitForSeconds(releaseTime);
		Release();
	}
}
