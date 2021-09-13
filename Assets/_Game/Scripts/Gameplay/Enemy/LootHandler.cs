using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Diannara.ScriptableObjects.Pooling;

namespace Diannara.Gameplay.Enemy
{
	[RequireComponent(typeof(Health))]
	public class LootHandler : MonoBehaviour
	{
		[Range(1, 100)]
		[SerializeField] private float m_spawnChance;

		[SerializeField] private ObjectPool[] m_lootTable;

		private Health m_health;
		private Transform m_transform;

		private void Awake()
		{
			m_transform = this.transform;
			m_health = GetComponent<Health>();
		}

		private void OnDisable()
		{
			if (m_health != null)
			{
				m_health.OnDeath -= OnDeath;
			}
		}

		private void OnEnable()
		{
			if (m_health != null)
			{
				m_health.OnDeath += OnDeath;
			}
		}

		private void OnDeath(Vector3 position)
		{
			SpawnLoot(position);
		}

		private void SpawnLoot(Vector3 position)
		{
			if(m_lootTable.Length <= 0)
			{
				return;
			}

			int randomSpawnChance = Random.Range(0, 100);
			if (randomSpawnChance >= (100 - m_spawnChance))
			{
				int randomIndex = Random.Range(0, m_lootTable.Length);

				ObjectPool randomLootPool = m_lootTable[randomIndex];
				randomLootPool.Spawn(position, Quaternion.identity, Vector3.one);
			}
		}
	}
}