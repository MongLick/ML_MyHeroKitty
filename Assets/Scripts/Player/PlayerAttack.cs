using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[Header("Specs")]
	[SerializeField] PlayerController player;
	[SerializeField] LayerMask monsterLayer;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (monsterLayer.Contain(collision.gameObject.layer))
		{
			IDamageable damageable = collision.GetComponent<IDamageable>();
			if (damageable != null)
			{
				damageable.TakeDamage(player.Damage);
			}
		}
	}
}
