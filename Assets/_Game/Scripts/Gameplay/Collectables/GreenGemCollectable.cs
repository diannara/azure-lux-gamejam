using UnityEngine;

using Diannara.Gameplay.Player;

namespace Diannara.Gameplay.Collectables
{
    public class GreenGemCollectable : BaseCollectible
    {
		[Header("Settings")]
		[SerializeField] protected float m_speedBoostIncrease;
		[SerializeField] protected float m_speedBoostDuration;

		protected override void HandlePickup(GameObject obj)
		{
			PlayerMovement movement = obj.GetComponent<PlayerMovement>();
			if(movement != null)
			{
				movement.ActivateSpeedBoost(m_speedBoostIncrease, m_speedBoostDuration);
			}
		}
	}
}