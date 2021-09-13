using UnityEngine;

using Diannara.ScriptableObjects.Variables;

namespace Diannara.Gameplay.Collectables
{
	public class CoinsCollectable : BaseCollectible
	{
		[Header("Settings")]
		[SerializeField] protected int m_value;

		[Header("Variables")]
		[SerializeField] protected IntVariable m_score;

		protected override void HandlePickup(GameObject obj)
		{
			m_score?.Add(m_value);
		}
	}
}