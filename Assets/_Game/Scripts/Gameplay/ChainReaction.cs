using UnityEngine;

using Diannara.ScriptableObjects.Pooling;

namespace Diannara.Gameplay
{
	[RequireComponent(typeof(Health))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class ChainReaction : MonoBehaviour
	{
		[Header("Damage Settings")]
		[SerializeField] private int m_damage;

		[Header("Pools")]
		[SerializeField] private ObjectPool m_chainImpactEffects;

		[Header("Collision Settings")]
		[SerializeField] private string m_collisionTag;
		[SerializeField] private float m_minimumForceForDamage;
		[SerializeField] private float m_hitForceModifier;

		public int Damage => m_damage;

		private Health m_health;
		private Rigidbody2D m_rigidbody;
		private ImpactHandler m_impactHandler;
		private Transform m_transform;

		private bool m_enableChainReaction;

		private void Awake()
		{
			m_transform = this.transform;
			m_health = GetComponent<Health>();
			m_impactHandler = GetComponent<ImpactHandler>();
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void HandleDamage(Collision2D collision, float impulse)
		{
			m_health.TakeDamage(m_damage, true);

			Health health = collision.gameObject.GetComponent<Health>();
			if (health != null && impulse > m_minimumForceForDamage)
			{
				health.TakeDamage(m_damage, true);
			}
		}

		private void HandleImpact(Collision2D collision)
		{
			if(m_impactHandler)
			{
				m_impactHandler.Knockback(collision.rigidbody.velocity.normalized, m_hitForceModifier);
			}

			ImpactHandler impactHandler = collision.gameObject.GetComponent<ImpactHandler>();
			if (impactHandler != null)
			{
				impactHandler.Knockback(m_rigidbody.velocity.normalized, m_hitForceModifier);
			}
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

			// Debug.Log($"ChainReaction :: Collision Detected! :: {gameObject.name} hit {collision.rigidbody.gameObject.name} ({collision.gameObject.tag}) with a force of {impulse} in the direction of {m_rigidbody.velocity}.");
			// Debug.Break();

			if (m_chainImpactEffects != null)
			{
				m_chainImpactEffects.Spawn(m_transform);
			}

			HandleImpact(collision);
			HandleDamage(collision, impulse);
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