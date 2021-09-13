using UnityEngine;
using UnityEngine.Events;

namespace Diannara.ScriptableObjects.Variables
{
	[CreateAssetMenu(fileName = "IntVariable.asset", menuName = "Diannara/Values/Int Variable")]
	public class IntVariable : VariableSO
	{
		public UnityAction<int> OnChanged;

		[SerializeField] private int m_currentValue;
		[SerializeField] private int m_startingValue;

		public int Value => m_currentValue;

		public void Add(int value)
		{
			m_currentValue += value;
			OnChanged?.Invoke(m_currentValue);
		}

		public void SetValue(int value)
		{
			if (m_currentValue == value)
			{
				return;
			}

			m_currentValue = value;
			OnChanged?.Invoke(m_currentValue);
		}

		public void Subtract(int value)
		{
			m_currentValue -= value;
			OnChanged?.Invoke(m_currentValue);
		}

		public void Reset()
		{
			m_currentValue = m_startingValue;
			OnChanged?.Invoke(m_currentValue);
		}
	}
}
