using UnityEngine;

namespace Diannara.Gameplay.Enemy
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Health))]
	public class EnemyMovement : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private float m_speed;
		[SerializeField] private bool m_disableMovement;

		private Health m_health;
		private Rigidbody2D m_rigidbody;
		private Transform m_transform;
		private Transform m_target;

		private void Awake()
		{
			m_transform = this.transform;
			m_health = GetComponent<Health>();
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void OnDisable()
		{
			m_health.OnInvincibleStatusChanged -= OnInvincibleStatusChanged;
		}

		private void OnEnable()
		{
			m_health.OnInvincibleStatusChanged += OnInvincibleStatusChanged;
		}

		private void OnInvincibleStatusChanged(bool status)
		{
			m_disableMovement = status;
		}

		public void SetTarget(Transform target)
		{
			m_target = target;
		}

		private void HandleMovement()
		{
			if(m_target == null || m_disableMovement)
			{
				return;
			}

			Vector2 direction = (m_target.position - m_transform.position).normalized;
			m_rigidbody.MovePosition(m_rigidbody.position + direction * m_speed * Time.fixedDeltaTime);
		}

		private void FixedUpdate()
		{
			HandleMovement();
		}
	}
}