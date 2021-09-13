using UnityEngine;
using UnityEngine.UI;

namespace Diannara.UI
{
	public class PlayerHeartUI : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Image m_heartSprite;

		public void ToggleHeart(bool enable)
		{
			if(m_heartSprite == null)
			{
				Debug.LogWarning("PlayerHeartUI :: ToggleHeart :: Heart sprite not assigned!");
				return;
			}
			m_heartSprite.gameObject.SetActive(enable);
		}
	}
}