using UnityEngine;

using Diannara.ScriptableObjects.Audio;
using Diannara.ScriptableObjects.Channels;
using Diannara.ScriptableObjects.Pooling;

namespace Diannara.Gameplay.Collectables
{
	public abstract class BaseCollectible : MonoBehaviour, ICollectable
	{
		[Header("Audio")]
		[SerializeField] protected AudioCue m_pickupSound;

		[Header("Channels")]
		[SerializeField] protected AudioCueEventChannel m_audioCueEventChannel;

		[Header("Pools")]
		[SerializeField] protected ObjectPool m_pool;

		public void DestroyCollectable()
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

		protected abstract void HandlePickup(GameObject obj);

		protected void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.gameObject.CompareTag("Player"))
			{
				HandlePickup(collision.gameObject);
				PlayPickupSound();
				DestroyCollectable();
			}
		}

		protected void PlayPickupSound()
		{
			if(m_audioCueEventChannel == null || m_pickupSound == null)
			{
				return;
			}

			m_audioCueEventChannel.RequestAudio(m_pickupSound);
		}
	}
}