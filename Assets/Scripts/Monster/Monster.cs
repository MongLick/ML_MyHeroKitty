using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour, IDamageable
{
	[Header("UnityAction")]
	private UnityAction<float> onHealthChanged;
	public UnityAction<float> OnHealthChanged { get { return onHealthChanged; } set { onHealthChanged = value; } }

	[Header("Components")]
	[SerializeField] Animator animator;
	[SerializeField] PooledObject pooledObject;
	[SerializeField] Transform player;
	[SerializeField] GameObject attack;

	[Header("Vector")]
	private Vector3 moveDirection;
	private Vector3 startPosition;

	[Header("Specs")]
	[SerializeField] float health;
	public float Health { get { return health; } set { health = value; onHealthChanged?.Invoke(health); } }
	[SerializeField] float maxHealth;
	public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
	[SerializeField] float damage;
	public float Damage { get { return damage; } }
	[SerializeField] float initialHealth;
	[SerializeField] float moveSpeed;
	[SerializeField] float detectionRange;
	[SerializeField] float randomDirectionChangeInterval;
	[SerializeField] float movementBoundary;
	[SerializeField] float lastDirectionChangeTime;
	[SerializeField] float followRange;
	[SerializeField] float stopFollowRange;
	[SerializeField] float attackCooldown;
	[SerializeField] float lastAttackTime;
	private bool isDead;
	public bool IsDead { get { return isDead; } }

	private void Start()
	{
		FindPlayer();
	}

	private void OnEnable()
	{
		lastDirectionChangeTime = Time.time;
		lastAttackTime = Time.time;
		Health = initialHealth;
		isDead = false;
		startPosition = transform.position;
		SetRandomDirection();
	}

	private void Update()
	{
		if (isDead)
		{
			return;
		}

		if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRange)
		{
			FollowPlayer();
		}
		else
		{
			RandomMovement();
		}
	}

	private void RandomMovement()
	{
		animator.SetBool("Move", true);

		if (Vector3.Distance(transform.position, startPosition) > movementBoundary)
		{
			Vector3 directionToCenter = (startPosition - transform.position).normalized;
			moveDirection = directionToCenter;
		}

		if (Time.time >= lastDirectionChangeTime + randomDirectionChangeInterval)
		{
			SetRandomDirection();
			lastDirectionChangeTime = Time.time;
		}

		transform.position += moveDirection * moveSpeed * Time.deltaTime;

		RotateBasedOnDirection(moveDirection);
	}

	private void FollowPlayer()
	{
		if (Vector3.Distance(transform.position, player.position) > stopFollowRange)
		{
			animator.SetBool("Move", true);
			Vector3 directionToPlayer = (player.position - transform.position).normalized;
			moveDirection = directionToPlayer;

			transform.position += moveDirection * moveSpeed * Time.deltaTime;

			RotateBasedOnDirection(moveDirection);
		}
		else
		{
			animator.SetBool("Move", false);

			if (Time.time >= lastAttackTime + attackCooldown)
			{
				animator.SetTrigger("Attack");
				lastAttackTime = Time.time;
			}

			animator.SetBool("Idle", true);
		}
	}

	private void SetRandomDirection()
	{
		float randomX = Random.Range(-1f, 1f);
		float randomY = Random.Range(-1f, 1f);
		moveDirection = new Vector3(randomX, randomY, 0).normalized;
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
		Manager.Monster.RemoveMonster(this);
		Manager.Data.GameData.deadMonster++;
		Manager.Game.CheckStageClear();
		pooledObject.Release();
	}

	private void FindPlayer()
	{
		GameObject playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null)
		{
			player = playerObject.transform;
		}
	}

	public void MonsterAttackTrue()
	{
		attack.gameObject.SetActive(true);
	}

	public void MonsterAttackFalse()
	{
		attack.gameObject.SetActive(false);
	}

	private void RotateBasedOnDirection(Vector3 direction)
	{
		if (direction.x > 0)
		{
			transform.rotation = Quaternion.Euler(0, 180, 0);
		}
		else if (direction.x < 0)
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}
}
