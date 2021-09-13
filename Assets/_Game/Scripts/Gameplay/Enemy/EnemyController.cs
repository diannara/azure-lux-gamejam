using UnityEngine;

using Diannara.ScriptableObjects.Pooling;

namespace Diannara.Gameplay.Enemy
{
	[RequireComponent(typeof(Health))]
	public class EnemyController : MonoBehaviour
	{
		[Header("Pools")]
		[SerializeField] private ObjectPool m_pool;

		private Health m_health;

		private void Awake()
		{
			m_health = GetComponent<Health>();
		}

		public void DestroyEnemy()
		{
			if (m_pool != null)
			{
				m_pool.Return(this.gameObject);
			}
			else
			{
				Destroy(this.gameObject);
			}
		}

		private void OnDeath(Vector3 position)
		{
			DestroyEnemy();
		}

		private void OnDisable()
		{
			m_health.OnDeath -= OnDeath;
		}

		private void OnEnable()
		{
			m_health.OnDeath += OnDeath;
		}

		private void OnSpawn()
		{
			m_health.ResetHealth();
		}
	}
}