using UnityEngine;

namespace Diannara.Gameplay
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class ImpactHandler : MonoBehaviour
	{
		[SerializeField] private float m_normalDrag;
		[SerializeField] private float m_knockbackDrag;

		private Rigidbody2D m_rigidbody;

		private Health m_health;

		private void Awake()
		{
			m_rigidbody = GetComponent<Rigidbody2D>();
			m_health = GetComponent<Health>();
		}

		public void Knockback(Vector3 direction, float force)
		{
			m_rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
		}

		private void OnDisable()
		{
			if (m_health != null)
			{
				m_health.OnInvincibleStatusChanged -= OnInvincibleStatusChanged;
			}
		}

		private void OnEnable()
		{
			if (m_health != null)
			{
				m_health.OnInvincibleStatusChanged += OnInvincibleStatusChanged;
			}
		}

		private void OnInvincibleStatusChanged(bool status)
		{
			if(status)
			{
				m_rigidbody.drag = m_knockbackDrag;
			}
			else
			{
				m_rigidbody.drag = m_normalDrag;
			}
		}

		private void OnSpawn()
		{
			m_rigidbody.drag = m_normalDrag;
		}
	}
}