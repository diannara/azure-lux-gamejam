using UnityEngine;

using Diannara.ScriptableObjects.Variables;

namespace Diannara.UI
{
	public class PlayerHealthUI : MonoBehaviour
	{
		[Header("Variables")]
		[SerializeField] private IntStat m_playerHealth;

		[Header("References")]
		[SerializeField] private PlayerHeartUI[] m_hearts;

		private void OnDisable()
		{
			if(m_playerHealth != null)
			{
				m_playerHealth.OnChanged -= OnPlayerHealthChanged;
			}
		}

		private void OnEnable()
		{
			if(m_playerHealth != null)
			{
				m_playerHealth.OnChanged += OnPlayerHealthChanged;
				UpdateHealthUI(m_playerHealth.Value);
			}
		}

		private void OnPlayerHealthChanged(int current, int previous)
		{
			UpdateHealthUI(current);
		}

		private void UpdateHealthUI(int current)
		{
			// Debug.Log($"PlayerHealthUI :: UpdateHealthUI() :: There are {current} hearts!");
			for(int i = 0; i < m_hearts.Length; i++)
			{
				if(current < (i + 1))
				{
					m_hearts[i].ToggleHeart(false);
				}
				else
				{
					m_hearts[i].ToggleHeart(true);
				}
			}
		}
	}
}