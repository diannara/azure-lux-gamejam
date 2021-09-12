using UnityEngine;

using Diannara.ScriptableObjects.Pooling;

namespace Diannara.ScriptableObjects.Gameplay
{
	[CreateAssetMenu(fileName = "SpawnSettings.asset", menuName = "Diannara/Spawning/Spawn Settings")]
	public class SpawnSettings : ScriptableObject
	{
		[SerializeField] private int m_maxEnemyCount;
		[SerializeField] private float m_spawnRate;
		[SerializeField] private ObjectPool m_pool;

		public int MaxEnemyCount => m_maxEnemyCount;
		public float SpawnRate => m_spawnRate;
		public ObjectPool Pool => m_pool;
	}
}