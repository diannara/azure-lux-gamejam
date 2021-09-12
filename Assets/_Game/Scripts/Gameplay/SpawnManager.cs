using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Diannara.Gameplay.Enemy;
using Diannara.ScriptableObjects.Gameplay;

namespace Diannara.Gameplay
{
	public class SpawnManager : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private bool m_enableSpawning = true;
		[SerializeField] private float m_spawnRadius;

		[Header("Spawners")]
		[SerializeField] private SpawnSettings[] m_spawnSettings;

		[Header("References")]
		[SerializeField] private Transform m_enemyTarget;

		private Dictionary<string, int> m_spawnCount = new Dictionary<string, int>();

		private Vector3 GetRandomSpawnPoint()
		{
			float angle = Random.Range(0.0f, 360.0f);

			float x = Mathf.Cos(angle) * m_spawnRadius;
			float y = Mathf.Sin(angle) * m_spawnRadius;

			Vector3 randomDirection = new Vector3(x, y, 0.0f);

			if (m_enemyTarget != null)
			{
				return m_enemyTarget.position + randomDirection;
			}

			return randomDirection;
		}

		private IEnumerator SpawnEnemies(string name, SpawnSettings settings)
		{

			while(true)
			{
				int currentCount = 0;
				m_spawnCount.TryGetValue(name, out currentCount);

				if(m_enableSpawning && currentCount < settings.MaxEnemyCount)
				{
					m_spawnCount[name] += 1;

					GameObject enemyGO = settings.Pool.Spawn(GetRandomSpawnPoint(), Quaternion.identity, Vector3.one);
					EnemyMovement enemy = enemyGO.GetComponent<EnemyMovement>();
					if(enemy != null)
					{
						enemy.SetTarget(m_enemyTarget);
					}
				}
				yield return new WaitForSeconds(settings.SpawnRate);
			}
		}

		private void Start()
		{
			StartSpawning();
		}

		public void StartSpawning()
		{
			foreach (SpawnSettings settings in m_spawnSettings)
			{
				string name = settings.Pool.Prefab.name;
				m_spawnCount.Add(name, 0);
				StartCoroutine(SpawnEnemies(name, settings));
			}
		}

		public void OnDeath()
		{

		}
	}
}