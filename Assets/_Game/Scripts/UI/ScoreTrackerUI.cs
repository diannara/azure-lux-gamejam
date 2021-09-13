using UnityEngine;

using TMPro;

using Diannara.ScriptableObjects.Variables;

namespace Diannara.UI
{
	public class ScoreTrackerUI : MonoBehaviour
	{
		[Header("Variables")]
		[SerializeField] private IntVariable m_score;

		[Header("References")]
		[SerializeField] private TMP_Text m_scoreText;

		private void Start()
		{
			m_score.SetValue(0);
			UpdateScoreUI(m_score.Value);
		}

		private void OnDisable()
		{
			if(m_score != null)
			{
				m_score.OnChanged -= OnScoreChanged;
			}
		}

		private void OnEnable()
		{
			if (m_score != null)
			{
				m_score.OnChanged += OnScoreChanged;
				UpdateScoreUI(m_score.Value);
			}
		}

		private void OnScoreChanged(int score)
		{
			UpdateScoreUI(score);
		}

		private void UpdateScoreUI(int score)
		{
			if(m_scoreText == null)
			{
				return;
			}
			m_scoreText.text = score.ToString();
		}
	}
}