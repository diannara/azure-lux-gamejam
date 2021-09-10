using System;
using System.Collections.Generic;

using Diannara.ScriptableObjects.Audio;

namespace Diannara.Audio
{
	public class SoundEmitterVault
	{
		private List<AudioCueKey> m_emittersKey;
		private List<SoundEmitter> m_emittersList;

		public SoundEmitterVault()
		{
			m_emittersKey = new List<AudioCueKey>();
			m_emittersList = new List<SoundEmitter>();
		}

		public AudioCueKey GetKey(AudioCue cue)
		{
			return new AudioCueKey(Guid.NewGuid(), cue);
		}

		public void Add(AudioCueKey key, SoundEmitter emitter)
		{
			m_emittersKey.Add(key);
			m_emittersList.Add(emitter);
		}

		public AudioCueKey Add(AudioCue cue, SoundEmitter emitter)
		{
			AudioCueKey emitterKey = GetKey(cue);
			emitter.SetAudioCueKey(emitterKey);
			m_emittersKey.Add(emitterKey);
			m_emittersList.Add(emitter);

			return emitterKey;
		}

		public bool Get(AudioCueKey key, out SoundEmitter emitter)
		{
			int index = m_emittersKey.FindIndex(x => x == key);

			if (index < 0)
			{
				emitter = null;
				return false;
			}

			emitter = m_emittersList[index];
			return true;
		}

		public bool Remove(AudioCueKey key)
		{
			int index = m_emittersKey.FindIndex(x => x == key);
			return RemoveAt(index);
		}

		private bool RemoveAt(int index)
		{
			if (index < 0)
			{
				return false;
			}

			m_emittersKey.RemoveAt(index);
			m_emittersList.RemoveAt(index);

			return true;
		}
	}
}