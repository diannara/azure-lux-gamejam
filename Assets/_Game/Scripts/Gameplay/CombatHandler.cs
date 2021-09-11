using UnityEngine;

namespace Diannara.Gameplay
{
	[RequireComponent(typeof(Health))]
	public class CombatHandler : MonoBehaviour
	{
		[SerializeField] private string m_collisionTag;
		[SerializeField] private float m_minimumVelocityForDamage;

		private Health m_health;

		private void Awake()
		{
			m_health = GetComponent<Health>();
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.CompareTag(m_collisionTag) && collision.rigidbody.velocity.magnitude >= m_minimumVelocityForDamage)
			{
				Debug.Log($"CombatHandler :: Collision Detected :: {collision.gameObject.name} : {collision.gameObject.tag} : {collision.rigidbody.velocity.magnitude}");
				Weapon weapon = collision.gameObject.GetComponent<Weapon>();
				if (weapon != null)
				{
					m_health.TakeDamage(weapon.Damage);
				}
			}
		}

		private void OnDeath()
		{
			Destroy(this.gameObject);
		}

		private void OnDisable()
		{
			m_health.OnDeath -= OnDeath;
		}

		private void OnEnable()
		{
			m_health.OnDeath += OnDeath;
		}
	}
}