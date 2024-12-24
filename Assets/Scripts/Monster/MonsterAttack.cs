using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
	[Header("Specs")]
	[SerializeField] Monster monster;
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
