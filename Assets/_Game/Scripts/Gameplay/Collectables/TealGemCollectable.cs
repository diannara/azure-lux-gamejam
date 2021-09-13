using UnityEngine;

namespace Diannara.Gameplay.Collectables
{
    public class TealGemCollectable : BaseCollectible
    {
		[Header("Settings")]
		[SerializeField] protected float m_shieldDuration;

		protected override void HandlePickup(GameObject obj)
		{
			Health health = obj.GetComponent<Health>();
			if (health != null)
			{
				health.ActivateShield(m_shieldDuration);
			}
		}
	}
}