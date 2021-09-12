using UnityEngine;

namespace Diannara.Gameplay
{
	[RequireComponent(typeof(Health))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class ChainReaction : MonoBehaviour
	{
		[Header("Damage Settings")]
		[SerializeField] private int m_damage;

		[Header("Collision Settings")]
		[SerializeField] private string m_collisionTag;
		[SerializeField] private float m_minimumForceForDamage;
		[SerializeField] private float m_hitForceModifier;

		public int Damage => m_damage;

		private Health m_health;
		private Rigidbody2D m_rigidbody;

		private bool m_enableChainReaction;

		private void Awake()
		{
			m_health = GetComponent<Health>();
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if(!m_enableChainReaction)
			{
				return;
			}

			if (!collision.gameObject.CompareTag(m_collisionTag))
			{
				return;
			}

			float impulse = GameUtilities.CalulcateImpulse(collision);
			Debug.Log($"Weapon :: Collision Detected! :: {gameObject.name} hit {collision.rigidbody.gameObject.name} ({collision.gameObject.tag}) with a force of {impulse} in the direction of {m_rigidbody.velocity}.");
			//Debug.Break();

			ImpactHandler impactHandler = collision.gameObject.GetComponent<ImpactHandler>();
			if (impactHandler != null)
			{
				// 1. Knockback target
				impactHandler.Knockback(m_rigidbody.velocity.normalized, m_hitForceModifier);
			}

			Health health = collision.gameObject.GetComponent<Health>();
			if(health != null && impulse > m_minimumForceForDamage)
			{
				// 2. Even if the target being hit is currently invincible, we are still going to cause damage
				health.TakeDamage(m_damage, true);
			}
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
			m_enableChainReaction = status;
		}
	}
}