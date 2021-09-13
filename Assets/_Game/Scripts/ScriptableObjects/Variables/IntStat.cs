using UnityEngine;
using UnityEngine.Events;

namespace Diannara.ScriptableObjects.Variables
{
	[CreateAssetMenu(fileName = "IntStat.asset", menuName = "Diannara/Values/Int Stat")]
	public class IntStat : VariableSO
	{
		public UnityAction<int, int> OnChanged;
		public UnityAction<int> OnMaximumChanged;
		public UnityAction<int> OnMinimumChanged;

		[SerializeField] private int m_currentValue;
		[SerializeField] private int m_maximumValue;
		[SerializeField] private int m_minimumValue;

		public int Value => m_currentValue;
		public int Maximum => m_maximumValue;
		public int Minimum => m_minimumValue;

		public void Add(int value)
		{
			int previousValue = m_currentValue;

			m_currentValue += value;
			m_currentValue = Mathf.Clamp(m_currentValue, m_minimumValue, m_maximumValue);

			if (previousValue != m_currentValue)
			{
				OnChanged?.Invoke(m_currentValue, previousValue);
			}
		}

		public void SetMaximum(int value)
		{
			m_maximumValue = value;

			OnMaximumChanged?.Invoke(m_maximumValue);

			int previousValue = m_currentValue;
			m_currentValue = Mathf.Clamp(m_currentValue, m_minimumValue, m_maximumValue);

			if (previousValue != m_currentValue)
			{
				OnChanged?.Invoke(m_currentValue, previousValue);
			}
		}

		public void SetMinimum(int value)
		{
			m_minimumValue = value;

			OnMinimumChanged?.Invoke(m_minimumValue);

			int previousValue = m_currentValue;
			m_currentValue = Mathf.Clamp(m_currentValue, m_minimumValue, m_maximumValue);

			if (previousValue != m_currentValue)
			{
				OnChanged?.Invoke(m_currentValue, previousValue);
			}
		}

		public void SetValue(int value)
		{
			if (m_currentValue == value)
			{
				return;
			}

			int previousValue = m_currentValue;
			m_currentValue = Mathf.Clamp(value, m_minimumValue, m_maximumValue);

			if (previousValue != m_currentValue)
			{
				OnChanged?.Invoke(m_currentValue, previousValue);
			}
		}

		public void Subtract(int value)
		{
			int previousValue = m_currentValue;

			m_currentValue -= value;
			m_currentValue = Mathf.Clamp(m_currentValue, m_minimumValue, m_maximumValue);

			if (previousValue != m_currentValue)
			{
				OnChanged?.Invoke(m_currentValue, previousValue);
			}
		}
	}
}
