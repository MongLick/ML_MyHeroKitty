using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] PlayerController player;
	[SerializeField] RectTransform rect;
	[SerializeField] Transform target;
	[SerializeField] Slider healthBar;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
	}

	private void OnEnable()
	{
		player.OnHealthChanged += UpdateView;
	}

	private void OnDisable()
	{
		player.OnHealthChanged -= UpdateView;
	}

	private void Start()
	{
		healthBar.maxValue = player.MaxHealth; 
		healthBar.value = player.Health;
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
