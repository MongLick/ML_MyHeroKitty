using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IDamageable
{
	[Header("UnityAction")]
	private UnityAction<float> onHealthChanged;
	public UnityAction<float> OnHealthChanged { get { return onHealthChanged; } set { onHealthChanged = value; } }

	[Header("Components")]
	[SerializeField] Animator animator;
	[SerializeField] Monster targetMonster;
	[SerializeField] BossMonster bossMonster;
	[SerializeField] GameObject attack;
	[SerializeField] Transform spawnPoint;

	[Header("Vector")]
	private Vector3 targetPosition;

	[Header("Specs")]
	[SerializeField] float attackRange;
	[SerializeField] float moveSpeed;
	[SerializeField] float damage;
	public float Damage { get { return damage; } }
	[SerializeField] float health;
	public float Health { get { return health; } set { health = value; onHealthChanged?.Invoke(health); } }
	[SerializeField] float maxHealth;
	public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
	[SerializeField] float attackCooltime;
	private bool isAttacking;

	private void OnEnable()
	{
		Manager.Game.OnBossTry += BossTry;
		Manager.Game.OnStage += ResetPlayer;
	}

	private void OnDisable()
	{
		Manager.Game.OnBossTry -= BossTry;
		Manager.Game.OnStage -= ResetPlayer;
	}

	private void Update()
	{
		if (targetMonster == null || targetMonster.IsDead)
		{
			targetMonster = Manager.Monster.GetClosestMonster(transform.position);
		}
		else
		{
			RotateTowardsTarget(targetMonster.transform);

			if (Vector3.Distance(transform.position, targetMonster.transform.position) <= attackRange)
			{
				if (!isAttacking)
				{
					StartCoroutine(Attack(targetMonster));
				}
			}
			else
			{
				MoveToTarget(targetMonster);
			}
		}
	}

	private void MoveToTarget(Monster target)
	{
		animator.SetBool("Idle", false);
		animator.SetBool("Move", true);
		targetPosition = target.transform.position;
		Vector3 direction = (targetPosition - transform.position).normalized;
		transform.position += direction * moveSpeed * Time.deltaTime;
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
		ResetPlayer();
		Manager.Game.RetryStage();
	}

	private void RotateTowardsTarget(Transform target)
	{
		if (target.position.x > transform.position.x)
		{
			transform.rotation = Quaternion.Euler(0, 180, 0);
		}
		else
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}

	public void PlayerAttackTrue()
	{
		attack.gameObject.SetActive(true);
	}

	public void PlayerAttackFalse()
	{
		attack.gameObject.SetActive(false);
	}

	private void BossTry()
	{
		ResetPlayer();
		StartCoroutine(MoveToBoss());
	}

	private void ResetPlayer()
	{
		StopAllCoroutines();
		animator.SetBool("Idle", true);
		animator.SetBool("Move", false);
		animator.Play("Idle");
		Health = maxHealth;
		isAttacking = false;
		targetMonster = null;
		bossMonster = null;
		transform.position = spawnPoint.position;
	}

	private IEnumerator MoveToBoss()
	{
		bossMonster = Manager.Game.Boss;

		while (Vector3.Distance(transform.position, bossMonster.transform.position) > attackRange * 2f)
		{
			Vector3 direction = (bossMonster.transform.position - transform.position).normalized;
			transform.position += direction * moveSpeed * Time.deltaTime;

			RotateTowardsTarget(bossMonster.transform);

			yield return null;
		}

		yield return BossAttack(bossMonster);
	}

	private IEnumerator Attack(Monster monster)
	{
		animator.SetBool("Idle", false);
		animator.SetBool("Move", false);
		animator.SetTrigger("Attack");
		animator.SetBool("Idle", true);

		isAttacking = true;

		yield return new WaitForSeconds(attackCooltime);

		isAttacking = false;

		if (monster.IsDead)
		{
			damage++;
			targetMonster = null;
		}
	}

	private IEnumerator BossAttack(BossMonster monster)
	{
		while (!monster.IsDead)
		{
			animator.SetBool("Idle", false);
			animator.SetBool("Move", false);
			animator.SetTrigger("Attack");
			animator.SetBool("Idle", true);

			isAttacking = true;

			yield return new WaitForSeconds(attackCooltime);

			isAttacking = false;
		}

		bossMonster = null;
	}
}
