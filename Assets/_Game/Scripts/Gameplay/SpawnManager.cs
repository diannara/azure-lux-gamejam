using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Diannara.Enums;
using Diannara.Gameplay.Collectables;
using Diannara.Gameplay.Enemy;
using Diannara.ScriptableObjects.Channels;
using Diannara.ScriptableObjects.Gameplay;

namespace Diannara.Gameplay
{
	public class SpawnManager : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private bool m_enableSpawning = true;
		[SerializeField] private float m_spawnRadius;

		[Header("Channels")]
		[SerializeField] private GameStateHandler m_gameStateHandler;
		[SerializeField] private DeathEventChannel m_deathEventChannel;

		[Header("Spawners")]
		[SerializeField] private SpawnSettings[] m_spawnSettings;

		[Header("References")]
		[SerializeField] private Transform m_enemyTarget;

		private Dictionary<CharacterType, int> m_spawnCount = new Dictionary<CharacterType, int>();

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

		private void OnDisable()
		{
			if(m_deathEventChannel != null)
			{
				m_deathEventChannel.OnDeathEvent -= OnDeath;
			}

			if(m_gameStateHandler != null)
			{
				m_gameStateHandler.OnGameStateChanged -= OnGameStateChange;
			}
		}

		private void OnEnable()
		{
			if(m_deathEventChannel != null)
			{
				m_deathEventChannel.OnDeathEvent += OnDeath;
			}

			if (m_gameStateHandler != null)
			{
				m_gameStateHandler.OnGameStateChanged += OnGameStateChange;
			}
		}

		private IEnumerator SpawnEnemies(CharacterType type, SpawnSettings settings)
		{
			yield return new WaitForSeconds(settings.SpawnDelay);

			while (true)
			{
				if (m_spawnCount.TryGetValue(type, out int currentCount))
				{
					if (m_enableSpawning && currentCount < settings.MaxEnemyCount)
					{
						m_spawnCount[type] += 1;

						GameObject enemyGO = settings.Pool.Spawn(GetRandomSpawnPoint(), Quaternion.identity, Vector3.one);
						EnemyMovement enemy = enemyGO.GetComponent<EnemyMovement>();
						if (enemy != null)
						{
							enemy.SetTarget(m_enemyTarget);
						}
					}
				}
				yield return new WaitForSeconds(settings.SpawnRate);
			}
		}

		private void OnGameStateChange(GameStateType current, GameStateType previous)
		{
			if(current == GameStateType.Playing)
			{
				StartSpawning();
			}
			else
			{
				StopSpawning();
			}
		}

		private void StartSpawning()
		{
			m_enableSpawning = true;

			m_spawnCount.Clear();

			foreach (SpawnSettings settings in m_spawnSettings)
			{
				GameObject go = settings.Pool.Prefab;
				Health health = go.GetComponent<Health>();

				if(health != null)
				{
					m_spawnCount.Add(health.Type, 0);
					StartCoroutine(SpawnEnemies(health.Type, settings));
				}
			}
		}

		private void StopSpawning()
		{
			m_enableSpawning = false;

			StopAllCoroutines();

			EnemyController[] enemies = FindObjectsOfType<EnemyController>();
			foreach(EnemyController enemy in enemies)
			{
				enemy.DestroyEnemy();
			}

			ICollectable[] collectables = FindObjectsOfType<BaseCollectible>();
			foreach (ICollectable collectable in collectables)
			{
				collectable.DestroyCollectable();
			}
		}

		public void OnDeath(CharacterType type)
		{
			if(m_spawnCount.ContainsKey(type))
			{
				m_spawnCount[type] -= 1;
			}
		}
	}
}