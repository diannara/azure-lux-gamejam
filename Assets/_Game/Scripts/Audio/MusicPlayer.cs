using UnityEngine;

using Diannara.ScriptableObjects.Audio;
using Diannara.ScriptableObjects.Channels;

namespace Diannara.Audio
{
	public class MusicPlayer : MonoBehaviour
	{
		[Header("Scene Data")]
		[SerializeField] protected AudioCue m_backgroundMusic;

		[Header("Channels")]
		[SerializeField] protected AudioCueEventChannel m_audioCueEventChannel;

		private void Start()
		{
			PlayerBackgroundMusic();
		}

		public virtual void PlayerBackgroundMusic()
		{
			m_audioCueEventChannel?.RequestAudio(m_backgroundMusic);
		}
	}
}