using UnityEngine;

namespace Diannara.Gameplay
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private int m_damage;

		public int Damage => m_damage;
	}
}