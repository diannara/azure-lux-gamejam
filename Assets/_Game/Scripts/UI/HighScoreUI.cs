using UnityEngine;

using TMPro;

namespace Diannara.UI
{
	public class HighScoreUI : MonoBehaviour
	{
		[Header("UI")]
		[SerializeField] private TMP_Text m_username;
		[SerializeField] private TMP_Text m_score;

		public void UpdateScoreUI(string username, string score)
		{
			m_username.text = username;
			m_score.text = score;
		}	
	}
}