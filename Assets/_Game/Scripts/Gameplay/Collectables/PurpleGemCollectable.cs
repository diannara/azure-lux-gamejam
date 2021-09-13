using UnityEngine;

using Diannara.Gameplay.Player;

namespace Diannara.Gameplay.Collectables
{
    public class PurpleGemCollectable : BaseCollectible
    {
		[Header("Settings")]
		[SerializeField] protected int m_damageBuff;
		[SerializeField] protected float m_damageBuffDuration;

		protected override void HandlePickup(GameObject obj)
		{
			PlayerController pc = obj.GetComponent<PlayerController>();
			if(pc != null)
			{
				pc.ActivateDamageBoost(m_damageBuff, m_damageBuffDuration);
			}
		}
	}
}
