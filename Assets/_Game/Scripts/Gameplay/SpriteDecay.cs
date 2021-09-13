using System.Collections;

using DG.Tweening;

using UnityEngine;

using Diannara.ScriptableObjects.Pooling;

namespace Diannara.Gameplay
{
	public class SpriteDecay : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private float m_decayDelay;
		[SerializeField] private float m_decayTime;

		[Header("References")]
		[SerializeField] private ObjectPool m_pool;
		[SerializeField] private SpriteRenderer m_sprite;

		private void OnDespawn()
		{
			if (m_sprite == null)
			{
				return;
			}

			m_sprite.DOFade(1.0f, 0.0f);
		}

		private void OnSpawn()
		{
			if (m_sprite == null)
			{
				return;
			}

			StartCoroutine(Decay());
		}

		private IEnumerator Decay()
		{
			yield return new WaitForSeconds(m_decayDelay);

			m_sprite.DOFade(0.0f, m_decayTime).OnComplete(() => {
				if (m_pool != null)
				{
					m_pool.Return(this.gameObject);
				}
				else
				{
					Destroy(this.gameObject);
				}
			});
		}
	}
}