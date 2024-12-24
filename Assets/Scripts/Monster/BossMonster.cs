using UnityEngine;
using UnityEngine.Events;

public class BossMonster : MonoBehaviour, IDamageable
{
	[Header("UnityAction")]
	private UnityAction<float> onHealthChanged;
	public UnityAction<float> OnHealthChanged { get { return onHealthChanged; } set { onHealthChanged = value; } }

	[Header("Components")]
	[SerializeField] Animator animator;
	[SerializeField] GameObject attack;
	[SerializeField] Transform player;

	[Header("Specs")]
	[SerializeField] float health;
	public float Health { get { return health; } set { health = value; onHealthChanged?.Invoke(health); } }
	[SerializeField] float maxHealth;
	public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
	[SerializeField] float damage;
	public float Damage { get { return damage; } }
	[SerializeField] float initialHealth;
	[SerializeField] float attackCooldown;
	[SerializeField] float lastAttackTime;
	[SerializeField] float stopFollowRange;
	private bool isDead;
	public bool IsDead { get { return isDead; } }

	private void OnEnable()
	{
		animator.SetBool("Idle", true);
		Health = initialHealth;
		isDead = false;
		lastAttackTime = Time.time;
		FindPlayer();
	}

	private void Update()
	{
		if (!(Vector3.Distance(transform.position, player.position) > stopFollowRange))
		{
			if (Time.time >= lastAttackTime + attackCooldown)
			{
				animator.SetTrigger("Attack");
				lastAttackTime = Time.time;
			}
		}
	}

	public void TakeDamage(float damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		isDead = true;
		Destroy(gameObject);
		Manager.Game.NextStage();
	}

	public void BossDefeatFailed()
	{
		Destroy(gameObject);
	}

	public void MonsterAttackTrue()
	{
		attack.gameObject.SetActive(true);
	}

	public void MonsterAttackFalse()
	{
		attack.gameObject.SetActive(false);
	}

	private void FindPlayer()
	{
		GameObject playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null)
		{
			player = playerObject.transform;
		}
	}
}
