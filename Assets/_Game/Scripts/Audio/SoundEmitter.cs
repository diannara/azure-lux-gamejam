using DG.Tweening;

using System.Collections;

using UnityEngine;
using UnityEngine.Events;

using Diannara.ScriptableObjects.Audio;

namespace Diannara.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundEmitter : MonoBehaviour
	{
		public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

		private AudioSource m_audioSource;

		public bool IsLooping => m_audioSource.loop;
		public bool IsPlaying => m_audioSource.isPlaying;

		public AudioClip CurrentClip => m_audioSource.clip;

		public AudioCueKey Key { get; private set; }

		private bool m_overridePitch = false;
		private float m_overridePitchValue = 1.0f;

		private void Awake()
		{
			m_audioSource = GetComponent<AudioSource>();
			m_audioSource.playOnAwake = false;
		}

		public void ClearAudioCueKey()
		{
			Key = AudioCueKey.Invalid;
		}

		public void PlayAudioClip(AudioClip clip, AudioConfig settings, bool hasToLoop, Vector3 position = default)
		{
			m_audioSource.clip = clip;
			settings.ApplyTo(m_audioSource);

			if (m_overridePitch)
			{
				m_audioSource.pitch = m_overridePitchValue;
			}

			m_audioSource.transform.position = position;
			m_audioSource.loop = hasToLoop;
			m_audioSource.Play();

			if (!hasToLoop)
			{
				StartCoroutine(FinishedPlaying(clip.length));
			}
		}

		public void FadeMusicIn(AudioClip musicClip, AudioConfig audioConfig, float duration, float startTime = 0f)
		{
			PlayAudioClip(musicClip, audioConfig, true);
			m_audioSource.volume = 0f;

			if (startTime <= m_audioSource.clip.length)
			{
				m_audioSource.time = startTime;
			}

			m_audioSource.DOFade(1f, duration);
		}

		public float FadeMusicOut(float duration)
		{
			m_audioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;
			return m_audioSource.time;
		}

		public void Finish()
		{
			if (m_audioSource.loop)
			{
				m_audioSource.loop = false;
				float timeRemaining = m_audioSource.clip.length - m_audioSource.time;
				StartCoroutine(FinishedPlaying(timeRemaining));
			}
		}

		private void NotifyBeingDone()
		{
			OnSoundFinishedPlaying?.Invoke(this);
		}

		private void OnFadeOutComplete()
		{
			NotifyBeingDone();
		}

		public void OverridePitch(bool overridePitch, float pitchValue = 1.0f)
		{
			m_overridePitch = overridePitch;
			m_overridePitchValue = pitchValue;
		}

		private void OnDestroy()
		{
			m_audioSource.DOKill();
		}

		public void Resume()
		{
			m_audioSource.Play();
		}

		public void Pause()
		{
			m_audioSource.Pause();
		}

		public void SetAudioCueKey(AudioCueKey key)
		{
			Key = key;
		}

		public void Stop()
		{
			m_audioSource.Stop();
		}

		private IEnumerator FinishedPlaying(float length)
		{
			yield return new WaitForSeconds(length);
			OnSoundFinishedPlaying?.Invoke(this);
		}
	}
}