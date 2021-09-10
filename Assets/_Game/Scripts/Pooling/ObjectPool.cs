using System.Collections.Generic;
using UnityEngine;

namespace Diannara.ScriptableObjects.Pooling
{
	[CreateAssetMenu(fileName = "ObjectPool.asset", menuName = "Diannara/Pooling/Object Pool")]
	public class ObjectPool : ScriptableObject
	{
		private const string METHOD_NAME_ONSPAWN = "OnSpawn";
		private const string METHOD_NAME_ONDESPAWN = "OnDespawn";

		[Header("Settings")]
		[SerializeField] private GameObject m_prefab;
		[SerializeField] private int m_initialSize;

		[Header("Dynamic Pool Settings")]
		[SerializeField] private bool m_canGrow;
		[SerializeField] private int m_growBySize;

		// Non-serialzied values will not persist between plays
		[System.NonSerialized] private bool m_isInitalized;
		[System.NonSerialized] private Transform m_parentContainer;
		[System.NonSerialized] private Transform m_parent;
		[System.NonSerialized] private Queue<GameObject> m_pool = new Queue<GameObject>();

		public int Count => m_pool.Count;
		public GameObject Prefab => m_prefab;

		public Transform ParentContainer
		{
			get
			{
				if (m_parentContainer == null)
				{
					m_parentContainer = new GameObject($"ObjectPool - [{m_prefab.name}]").transform;
					m_parentContainer.SetParent(m_parent);
				}
				return m_parentContainer;
			}
		}


		public void Clear()
		{
			Debug.Log($"ObjectPool :: Clear() :: Cleaning up ObjectPool for {m_prefab.name}...");
			while (m_pool.Count > 0)
			{
				GameObject go = m_pool.Dequeue();
				Destroy(go);
			}

			m_pool.Clear();
			m_isInitalized = false;
			Debug.Log($"ObjectPool :: Clear() :: ObjectPool for {m_prefab.name} has been cleared!");
		}

		public void Initialize()
		{
			if (m_isInitalized)
			{
				return;
			}

			GrowPool(m_initialSize);
			m_isInitalized = true;
		}

		public void GrowPool(int size)
		{
			for (int i = 0; i < size; i++)
			{
				GameObject go = Instantiate(m_prefab, Vector3.zero, Quaternion.identity);
				go.transform.parent = ParentContainer;
				go.SetActive(false);

				m_pool.Enqueue(go);
			}
		}

		public void SetParent(Transform parent)
		{
			m_parent = parent;
			m_parentContainer.SetParent(m_parent);
		}

		public GameObject Spawn(Transform transform, Transform parent = null)
		{
			return Spawn(transform.position, transform.rotation, m_prefab.transform.localScale, parent);
		}

		public GameObject Spawn(Vector3 position, Quaternion rotiation, Vector3 scale, Transform parent = null)
		{
			if (m_pool.Count <= 0 && m_canGrow)
			{
				GrowPool(m_growBySize);
			}

			if (m_pool.Count > 0)
			{
				GameObject go = TakeFromPool(position, rotiation, scale, parent);
				TriggerOnSpawn(go);
				return go;
			}

			return null;
		}

		private GameObject TakeFromPool(Vector3 position, Quaternion rotiation, Vector3 scale, Transform parent = null)
		{
			GameObject go = m_pool.Dequeue();
			go.transform.parent = parent;
			go.transform.position = position;
			go.transform.rotation = rotiation;
			go.transform.localScale = scale;
			go.SetActive(true);
			return go;
		}

		private void TriggerOnSpawn(GameObject target)
		{
			target.BroadcastMessage(METHOD_NAME_ONSPAWN, SendMessageOptions.DontRequireReceiver);
		}

		private void TriggerOnDespawn(GameObject target)
		{
			target.BroadcastMessage(METHOD_NAME_ONDESPAWN, SendMessageOptions.DontRequireReceiver);
		}

		public void Return(GameObject go)
		{
			TriggerOnDespawn(go);

			go.SetActive(false);
			go.transform.localScale = Vector3.one;
			go.transform.rotation = Quaternion.identity;
			go.transform.position = Vector3.zero;
			go.transform.parent = ParentContainer;
			m_pool.Enqueue(go);
		}
	}
}