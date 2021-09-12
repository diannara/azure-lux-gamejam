using UnityEngine;

using Diannara.ScriptableObjects.Audio;
using Diannara.ScriptableObjects.Channels;
using Diannara.ScriptableObjects.Pooling;

namespace Diannara.Gameplay
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Weapon : MonoBehaviour
	{
		[Header("Weapon Settings")]
		[SerializeField] private int m_damage;

		[Header("Collision Settings")]
		[SerializeField] private string m_collisionTag;
		[SerializeField] private float m_minimumForceForDamage;
		[SerializeField] private float m_hitForceModifier;

		[Header("Pools")]
		[SerializeField] private ObjectPool m_lightImpactEffectsPool;

		[Header("Audio")]
		[SerializeField] private AudioCue m_lightHitAudio;

		[Header("Channels")]
		[SerializeField] private AudioCueEventChannel m_audioCueEventChannel;

		public int Damage => m_damage;

		private Rigidbody2D m_rigidbody;

		private void Awake()
		{
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if(!collision.gameObject.CompareTag(m_collisionTag))
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

			if(impulse < m_minimumForceForDamage)
			{
				PlayLightImpactSound();
				SpawnLightImpactEffect(collision.transform);
				return;
			}

			Health health = collision.gameObject.GetComponent<Health>();
			if(health == null)
			{
				return;
			}
			
			// 2. If the target is invincible or the attack was not strong enough, play light hit sound
			if(health.IsInvincible || impulse < m_minimumForceForDamage)
			{
				PlayLightImpactSound();
				SpawnLightImpactEffect(collision.transform);
			}
			else
			{
				// 3. Take Damage
				health.TakeDamage(m_damage);
			}
		}

		private void PlayLightImpactSound()
		{
			if (m_audioCueEventChannel != null && m_lightHitAudio != null)
			{
				m_audioCueEventChannel.RequestAudio(m_lightHitAudio);
			}
		}

		private void SpawnLightImpactEffect(Transform transform)
		{
			if (m_lightImpactEffectsPool != null)
			{
				m_lightImpactEffectsPool.Spawn(transform);
			}
		}
	}
}