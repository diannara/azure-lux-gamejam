using UnityEngine;
using UnityEngine.Events;

namespace Diannara.Gameplay
{
	public class Health : MonoBehaviour
	{
		public UnityAction<int> OnHealtChanged;
		public UnityAction OnDeath;

		[SerializeField] private int m_maxHealth;
		[SerializeField] private int m_currentHealth;
		[SerializeField] private bool m_isDead;

		public int MaxHealth => m_maxHealth;
		public int CurrentHealth => m_currentHealth;
		public bool IsDead => m_isDead;

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

		public void TakeDamage(int damage)
		{
			if(m_isDead)
			{
				return;
			}

			m_currentHealth -= damage;
			if(m_currentHealth <= 0)
			{
				m_currentHealth = 0;
				m_isDead = true;
				OnDeath?.Invoke();
			}

			OnHealtChanged?.Invoke(m_currentHealth);
		}

		public void SetHealth(int health, bool isDead)
		{
			m_currentHealth = health;
			m_isDead = isDead;

			OnHealtChanged?.Invoke(m_currentHealth);
		}

		public void ResetHealth()
		{
			SetHealth(m_maxHealth, false);
		}
	}
}