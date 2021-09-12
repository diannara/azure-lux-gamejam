using System.Collections;

using UnityEngine;
using UnityEngine.Events;

using Diannara.ScriptableObjects.Audio;
using Diannara.ScriptableObjects.Channels;
using Diannara.ScriptableObjects.Pooling;

namespace Diannara.Gameplay
{
	public class Health : MonoBehaviour
	{
		public UnityAction<int> OnHealtChanged;
		public UnityAction<bool> OnInvincibleStatusChanged;
		public UnityAction OnDeath;

		[SerializeField] private int m_maxHealth;
		[SerializeField] private int m_currentHealth;
		[SerializeField] private bool m_isDead;

		[Header("Audio")]
		[SerializeField] private AudioCue m_takeDamageAudio;

		[Header("Channel")]
		[SerializeField] private AudioCueEventChannel m_audioCueEventChannel;

		[Header("Pools")]
		[SerializeField] private ObjectPool m_damageEffectPool;

		[Header("Invincibility")]
		[SerializeField] private bool m_canBecomeInvincible;
		[SerializeField] private bool m_isInvincible;
		[SerializeField] private float m_invincibleTime;

		public int MaxHealth => m_maxHealth;
		public int CurrentHealth => m_currentHealth;
		public bool IsDead => m_isDead;
		public bool IsInvincible => m_isInvincible;

		private Animator m_animator;
		private Coroutine m_invincibleCoroutine;
		private Transform m_transform;

		private void Awake()
		{
			m_transform = this.transform;
			m_animator = GetComponentInChildren<Animator>();
		}

		private void Start()
		{
			ResetHealth();
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
			if(m_currentHealth > m_maxHealth)
			{
				m_currentHealth = m_maxHealth;
			}

			OnHealtChanged?.Invoke(m_currentHealth);
		}

		private IEnumerator InvincibleCoroutine()
		{
			m_isInvincible = true;
			OnInvincibleStatusChanged?.Invoke(m_isInvincible);
			UpdateAnimator();

			yield return new WaitForSeconds(m_invincibleTime);

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
			OnHealtChanged?.Invoke(m_currentHealth);

			PlayDamageSound();
			SpawnDamageEffect();

			if (m_currentHealth <= 0)
			{
				m_currentHealth = 0;
				m_isDead = true;
				OnDeath?.Invoke();
			}

			if(!m_isDead && m_canBecomeInvincible && !forceDamage)
			{
				m_invincibleCoroutine = StartCoroutine(InvincibleCoroutine());
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
			m_currentHealth = health;
			m_isDead = isDead;

			OnHealtChanged?.Invoke(m_currentHealth);
		}

		public void SpawnDamageEffect()
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