using System.Collections;

using UnityEngine;
using UnityEngine.Events;

using Diannara.Enums;
using Diannara.ScriptableObjects.Audio;
using Diannara.ScriptableObjects.Channels;
using Diannara.ScriptableObjects.Pooling;
using Diannara.ScriptableObjects.Variables;

namespace Diannara.Gameplay
{
	public class Health : MonoBehaviour
	{
		public UnityAction<int> OnHealtChanged;
		public UnityAction<bool> OnInvincibleStatusChanged;
		public UnityAction<Vector3> OnDeath;

		[Header("Settings")]
		[SerializeField] private CharacterType m_characterType;
		[SerializeField] private int m_maxHealth;
		[SerializeField] private int m_currentHealth;
		[SerializeField] private bool m_isDead;

		[Header("Audio")]
		[SerializeField] private AudioCue m_takeDamageAudio;

		[Header("Channel")]
		[SerializeField] private AudioCueEventChannel m_audioCueEventChannel;
		[SerializeField] private DeathEventChannel m_deathEventChannel;

		[Header("Variables")]
		[SerializeField] private IntStat m_healthStat;

		[Header("Pools")]
		[SerializeField] private ObjectPool m_coinsPickupPool;
		[SerializeField] private ObjectPool m_damageEffectPool;
		[SerializeField] private ObjectPool m_deadbodyPool;

		[Header("Invincibility")]
		[SerializeField] private bool m_canBecomeInvincible;
		[SerializeField] private bool m_isInvincible;
		[SerializeField] private float m_invincibleTime;

		public CharacterType Type => m_characterType;
		public int MaxHealth => m_maxHealth;
		public int CurrentHealth => m_currentHealth;
		public bool IsDead => m_isDead;
		public bool IsInvincible => m_isInvincible;

		private Animator m_animator;
		private Coroutine m_invincibleCoroutine;
		private Transform m_transform;

		public void ActivateShield(float duration)
		{
			if(m_isInvincible)
			{
				StopCoroutine(m_invincibleCoroutine);
				m_invincibleCoroutine = null;

				m_isInvincible = false;

				if(m_animator != null)
				{
					m_animator.SetBool("IsInvincible", false);
				}
			}

			m_invincibleCoroutine = StartCoroutine(InvincibleCoroutine(duration));
		}

		private void Awake()
		{
			m_transform = this.transform;
			m_animator = GetComponentInChildren<Animator>();
		}

		private void Start()
		{
			ResetHealth();
		}

		private void HandleDeath(bool forceDamage)
		{
			m_isDead = true;
			m_deadbodyPool?.Spawn(m_transform);
			m_deathEventChannel?.RaiseDeathEvent(m_characterType);

			if (forceDamage)
			{
				m_coinsPickupPool?.Spawn(m_transform);
			}

			OnDeath?.Invoke(m_transform.position);
		}

		public void Heal(int health)
		{
			if(m_isDead)
			{
				return;
			}

			if(m_currentHealth == m_maxHealth)
			{
				return;
			}

			m_currentHealth += health;
			m_healthStat?.Add(health);

			if (m_currentHealth > m_maxHealth)
			{
				m_currentHealth = m_maxHealth;
			}
			OnHealtChanged?.Invoke(m_currentHealth);
		}

		private IEnumerator InvincibleCoroutine(float duration)
		{
			m_isInvincible = true;
			OnInvincibleStatusChanged?.Invoke(m_isInvincible);
			UpdateAnimator();

			yield return new WaitForSeconds(duration);

			m_isInvincible = false;
			OnInvincibleStatusChanged?.Invoke(m_isInvincible);
			UpdateAnimator();

			m_invincibleCoroutine = null;
		}

		public void TakeDamage(int damage, bool forceDamage = false)
		{
			if(m_isDead)
			{
				return;
			}

			if(m_isInvincible && !forceDamage)
			{
				return;
			}

			m_currentHealth -= damage;
			m_healthStat?.Subtract(damage);

			if (m_currentHealth <= 0)
			{
				m_currentHealth = 0;
			}

			OnHealtChanged?.Invoke(m_currentHealth);

			PlayDamageSound();

			if(m_currentHealth <= 0)
			{
				HandleDeath(forceDamage);
			}
			else
			{
				SpawnDamageEffect();
			}

			if (!m_isDead && m_canBecomeInvincible && !forceDamage)
			{
				m_invincibleCoroutine = StartCoroutine(InvincibleCoroutine(m_invincibleTime));
			}
		}

		private void PlayDamageSound()
		{
			if(m_audioCueEventChannel == null || m_takeDamageAudio == null)
			{
				return;
			}

			m_audioCueEventChannel.RequestAudio(m_takeDamageAudio);
		}

		public void SetHealth(int health, bool isDead)
		{
			m_healthStat?.SetValue(health);
			m_healthStat?.SetMaximum(m_maxHealth);
			m_healthStat?.SetMinimum(0);

			m_currentHealth = health;
			m_isDead = isDead;

			OnHealtChanged?.Invoke(m_currentHealth);
		}

		private void SpawnDamageEffect()
		{
			if(m_damageEffectPool == null)
			{
				return;
			}

			m_damageEffectPool.Spawn(m_transform);
		}

		public void ResetHealth()
		{
			SetHealth(m_maxHealth, false);

			if(m_invincibleCoroutine != null)
			{
				StopCoroutine(m_invincibleCoroutine);

				m_isInvincible = false;
				UpdateAnimator();
			}
		}

		private void UpdateAnimator()
		{
			if (m_animator == null)
			{
				return;
			}
			m_animator.SetBool("IsInvincible", m_isInvincible);
		}
	}
}