using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthSlider : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Monster monster;
	[SerializeField] RectTransform rect;
	[SerializeField] Transform target;
	[SerializeField] Slider healthBar;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
		healthBar.maxValue = monster.MaxHealth;
		healthBar.value = monster.Health;
	}

	private void OnEnable()
	{
		monster.OnHealthChanged += UpdateView;
	}

	private void OnDisable()
	{
		monster.OnHealthChanged -= UpdateView;
	}

	private void FixedUpdate()
	{
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);

		screenPosition.y += 200f;

		rect.position = screenPosition;
	}

	private void UpdateView(float value)
	{
		healthBar.value = value;
	}
}
