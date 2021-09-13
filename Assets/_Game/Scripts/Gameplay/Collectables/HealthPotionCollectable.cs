using UnityEngine;

namespace Diannara.Gameplay.Collectables
{
    public class HealthPotionCollectable : BaseCollectible
    {
		[Header("Settings")]
		[SerializeField] protected int m_value;

		protected override void HandlePickup(GameObject obj)
		{
			Health health = obj.GetComponent<Health>();
			if (health != null)
			{
				health.Heal(m_value);
			}
		}
	}
}
