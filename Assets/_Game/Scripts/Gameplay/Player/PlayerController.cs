using UnityEngine;

namespace Diannara.Gameplay.Player
{
	[RequireComponent(typeof(Health))]
	[RequireComponent(typeof(PlayerMovement))]
	public class PlayerController : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Transform m_playerSpawn;
		[SerializeField] private Weapon m_weapon;

		private Health m_health;
		private PlayerMovement m_movement;
		private Transform m_transform;

		public void ActivateDamageBoost(int damage, float duration)
		{
			if(m_weapon != null)
			{
				m_weapon.ActivateDamageBuff(damage, duration);
			}
		}

		private void Awake()
		{
			m_transform = this.transform;
			m_health = GetComponent<Health>();
			m_movement = GetComponent<PlayerMovement>();
		}

		public void ResetPlayer()
		{
			m_health.ResetHealth();
			m_movement.StopMovement();

			if(m_weapon != null)
			{
				m_weapon.ResetWeapon();
			}	

			m_transform.position = m_playerSpawn.position;
		}
	}
}