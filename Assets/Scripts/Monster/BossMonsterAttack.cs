using UnityEngine;

public class BossMonsterAttack : MonoBehaviour
{
	[Header("Specs")]
	[SerializeField] BossMonster monster;
	[SerializeField] LayerMask playerLayer;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (playerLayer.Contain(collision.gameObject.layer))
		{
			IDamageable damageable = collision.GetComponent<IDamageable>();
			if (damageable != null)
			{
				damageable.TakeDamage(monster.Damage);
			}
		}
	}
}
