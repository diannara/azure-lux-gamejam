using UnityEngine;
using UnityEngine.Audio;

using Diannara.ScriptableObjects.Audio;
using Diannara.ScriptableObjects.Channels;
using Diannara.ScriptableObjects.Pooling;

namespace Diannara.Audio
{
	public class AudioManager : MonoBehaviour
	{
		[Header("Player Audio Settings")]
		[SerializeField] private PlayerAudioSettings m_playerAudioSettings;

		[Header("Soundtrack Settings")]
		[SerializeField] private float m_fadeOutDuration = 2f;
		[SerializeField] private float m_fadeInDuration = 1f;

		[Header("Pools")]
		[SerializeField] private ObjectPool m_soundEmitterPool;

		[Header("Channels")]
		[SerializeField] private AudioCueEventChannel m_sfxEventChannel;
		[SerializeField] private AudioCueEventChannel m_musicEventChannel;

		[Header("Audio Control")]
		[SerializeField] private AudioMixer m_audioMixer;

		[Range(0.0f, 1.0f)]
		[SerializeField] private float m_masterVolume = 1.0f;

		[Range(0.0f, 1.0f)]
		[SerializeField] private float m_musicVolume = 1.0f;

		[Range(0.0f, 1.0f)]
		[SerializeField] private float m_sfxVolume = 1.0f;

		[Range(0.0f, 1.0f)]
		[SerializeField] private float m_uiVolume = 1.0f;

		private Transform m_transform;
		private SoundEmitter m_musicSoundEmitter;
		private SoundEmitterVault m_soundEmitterVault;

		private void Awake()
		{
			m_transform = this.transform;

			m_soundEmitterVault = new SoundEmitterVault();

			m_soundEmitterPool.Initialize();
			m_soundEmitterPool.SetParent(m_transform);
		}

		private bool FinishAudioCue(AudioCueKey key)
		{
			bool isFound = m_soundEmitterVault.Get(key, out SoundEmitter emitter);
			if (isFound)
			{
				emitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
				emitter.Finish();
			}
			else
			{
				Debug.LogWarning($"AudioManager :: FinishAudioCue() :: A request to finish an AudioCueKey {key.Guid} was received, but the AudioCue was not found.");
			}

			return isFound;
		}

		public float GetGroupVolume(string parameterName)
		{
			if (m_audioMixer.GetFloat(parameterName, out float rawVolume))
			{
				return MixerValueToNormalized(rawVolume);
			}
			else
			{
				Debug.LogError($"AudioManager :: GetGroupVolume() :: The AudioMixer parameter {parameterName} was not found!");
				return 0.0f;
			}
		}

		private float MixerValueToNormalized(float mixerValue)
		{
			return 1.0f + (mixerValue / 80.0f);
		}

		private float NormalizedToMixerValue(float normalizedValue)
		{
			return (normalizedValue - 1.0f) * 80.0f;
		}

		private void OnDestroy()
		{
			if (m_sfxEventChannel != null)
			{
				m_sfxEventChannel.OnAudioCuePlayRequested -= PlayAudioCue;
				m_sfxEventChannel.OnAudioCueFinishRequested -= FinishAudioCue;
				m_sfxEventChannel.OnAudioCueStopRequested -= StopAudioCue;
			}

			if (m_musicEventChannel != null)
			{
				m_musicEventChannel.OnAudioCuePlayRequested -= PlayMusicTrack;
				m_musicEventChannel.OnAudioCueStopRequested -= StopMusicTrack;
			}

			if (m_playerAudioSettings != null)
			{
				m_playerAudioSettings.OnValuesSaved -= OnValuesSaved;
				m_playerAudioSettings.OnMasterVolumeChanged -= OnMasterVolumeChanged;
				m_playerAudioSettings.OnMusicVolumeChanged -= OnMusicVolumeChanged;
				m_playerAudioSettings.OnSfxVolumeChanged -= OnSfxVolumeChanged;
				m_playerAudioSettings.OnUiVolumeChanged -= OnUiVolumeChanged;
			}
		}

		private void OnEnable()
		{
			if (m_sfxEventChannel != null)
			{
				m_sfxEventChannel.OnAudioCuePlayRequested += PlayAudioCue;
				m_sfxEventChannel.OnAudioCueFinishRequested += FinishAudioCue;
				m_sfxEventChannel.OnAudioCueStopRequested += StopAudioCue;
			}

			if (m_musicEventChannel != null)
			{
				m_musicEventChannel.OnAudioCuePlayRequested += PlayMusicTrack;
				m_musicEventChannel.OnAudioCueStopRequested += StopMusicTrack;
			}

			if (m_playerAudioSettings != null)
			{
				m_playerAudioSettings.OnValuesSaved += OnValuesSaved;
				m_playerAudioSettings.OnMasterVolumeChanged += OnMasterVolumeChanged;
				m_playerAudioSettings.OnMusicVolumeChanged += OnMusicVolumeChanged;
				m_playerAudioSettings.OnSfxVolumeChanged += OnSfxVolumeChanged;
				m_playerAudioSettings.OnUiVolumeChanged += OnUiVolumeChanged;
			}
		}

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				SetGroupVolume("MasterVolume", m_masterVolume);
				SetGroupVolume("MusicVolume", m_musicVolume);
				SetGroupVolume("SFXVolume", m_sfxVolume);
				SetGroupVolume("UIVolume", m_uiVolume);
			}
		}

		private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
		{
			StopAndCleanEmitter(soundEmitter);
		}

		private void OnStopMusicEmitter(SoundEmitter soundEmitter)
		{
			soundEmitter.ClearAudioCueKey();
			soundEmitter.OnSoundFinishedPlaying -= OnStopMusicEmitter;
			soundEmitter.Stop();
			soundEmitter.OverridePitch(false);

			if (m_soundEmitterPool != null)
			{
				m_soundEmitterPool.Return(soundEmitter.gameObject);
			}
		}

		private void OnValuesSaved(float masterVolume, float musicVolume, float sfxVolume, float uiVolume)
		{
			m_masterVolume = masterVolume;
			m_musicVolume = musicVolume;
			m_sfxVolume = sfxVolume;
			m_uiVolume = uiVolume;

			SetGroupVolume("MasterVolume", masterVolume);
			SetGroupVolume("MusicVolume", musicVolume);
			SetGroupVolume("SFXVolume", sfxVolume);
			SetGroupVolume("UIVolume", uiVolume);
		}

		private void OnMasterVolumeChanged(float volume)
		{
			m_masterVolume = volume;
			SetGroupVolume("MasterVolume", volume);
		}

		private void OnMusicVolumeChanged(float volume)
		{
			m_musicVolume = volume;
			SetGroupVolume("MusicVolume", volume);
		}

		private void OnSfxVolumeChanged(float volume)
		{
			m_sfxVolume = volume;
			SetGroupVolume("SFXVolume", volume);
		}

		private void OnUiVolumeChanged(float volume)
		{
			m_uiVolume = volume;
			SetGroupVolume("UIVolume", volume);
		}

		public AudioCueKey PlayMusicTrack(AudioCue audioCue, AudioConfig audioConfig, Vector3 position = default)
		{
			if (audioCue == null)
			{
				Debug.LogWarning("AudioManager :: PlayMusicTrack() :: AudioCue sent along was NULL!");
				return AudioCueKey.Invalid;
			}

			if (audioConfig == null)
			{
				Debug.LogWarning("AudioManager :: PlayMusicTrack() :: AudioConfig sent along was NULL!");
				return AudioCueKey.Invalid;
			}

			AudioClip clip = audioCue.GetNextClip();
			if (clip == null)
			{
				Debug.LogWarning("AudioManager :: PlayMusicTrack() :: AudioCue sent along did not contain any clips to play!!");
				return AudioCueKey.Invalid;
			}

			if (m_musicSoundEmitter != null && m_musicSoundEmitter.IsPlaying)
			{
				if (m_musicSoundEmitter.CurrentClip == clip)
				{
					// If the clip is already playing, no need to do anything
					return AudioCueKey.Invalid;
				}

				//Music is already playing, need to fade it out
				m_musicSoundEmitter.FadeMusicOut(m_fadeOutDuration);
			}

			GameObject go = m_soundEmitterPool.Spawn(m_transform, m_transform);
			if (go.TryGetComponent(out SoundEmitter soundEmitter))
			{
				m_musicSoundEmitter = soundEmitter;

				if (audioCue.RandomizePitch)
				{
					float pitchValue = Random.Range(audioCue.PitchRange.x, audioCue.PitchRange.y);
					m_musicSoundEmitter.OverridePitch(true, pitchValue);
				}

				m_musicSoundEmitter.SetAudioCueKey(AudioCueKey.Invalid);
				m_musicSoundEmitter.FadeMusicIn(clip, audioConfig, m_fadeInDuration, 0.0f);
				m_musicSoundEmitter.OnSoundFinishedPlaying += OnStopMusicEmitter;
			}

			return AudioCueKey.Invalid;
		}

		public AudioCueKey PlayAudioCue(AudioCue audioCue, AudioConfig audioConfig, Vector3 position = default)
		{
			if (audioCue == null)
			{
				Debug.LogWarning("AudioManager :: PlayAudioCue() :: AudioCue sent along was NULL!");
				return AudioCueKey.Invalid;
			}

			if (audioConfig == null)
			{
				Debug.LogWarning("AudioManager :: PlayAudioCue() :: AudioConfig sent along was NULL!");
				return AudioCueKey.Invalid;
			}

			AudioClip clipToPlay = audioCue.GetNextClip();
			if (clipToPlay == null)
			{
				Debug.LogWarning("AudioManager :: PlayAudioCue() :: AudioCue sent along did not contain any clips to play!!");
				return AudioCueKey.Invalid;
			}

			GameObject go = m_soundEmitterPool.Spawn(m_transform, m_transform);
			if (go.TryGetComponent(out SoundEmitter soundEmitter))
			{
				if (audioCue.RandomizePitch)
				{
					float pitchValue = Random.Range(audioCue.PitchRange.x, audioCue.PitchRange.y);
					soundEmitter.OverridePitch(true, pitchValue);
				}

				soundEmitter.PlayAudioClip(clipToPlay, audioConfig, audioCue.IsLooping, position);
				if (!audioCue.IsLooping)
				{
					soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
				}
			}

			return m_soundEmitterVault.Add(audioCue, soundEmitter);
		}

		public void SetGroupVolume(string parameterName, float normalizedVolume)
		{
			bool volumeSet = m_audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
			if (!volumeSet)
			{
				Debug.LogError("AudioManager :: GetGroupVolume() :: The AudioMixer parameter was not found!");
			}
		}

		private void Start()
		{
			m_playerAudioSettings?.LoadFromPlayerPrefs();

			m_masterVolume = GetGroupVolume("MasterVolume");
			m_musicVolume = GetGroupVolume("MusicVolume");
			m_sfxVolume = GetGroupVolume("SFXVolume");
			m_sfxVolume = GetGroupVolume("UIVolume");
		}

		private bool StopAudioCue(AudioCueKey key)
		{
			bool isFound = m_soundEmitterVault.Get(key, out SoundEmitter emitter);

			if (isFound)
			{
				StopAndCleanEmitter(emitter);
				m_soundEmitterVault.Remove(key);
			}

			return isFound;
		}

		private void StopAndCleanEmitter(SoundEmitter soundEmitter)
		{
			AudioCueKey key = soundEmitter.Key;

			soundEmitter.ClearAudioCueKey();
			if (!soundEmitter.IsLooping)
			{
				soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
			}

			soundEmitter.OverridePitch(false);
			soundEmitter.Stop();

			if (m_soundEmitterPool != null)
			{
				m_soundEmitterPool.Return(soundEmitter.gameObject);
			}

			m_sfxEventChannel?.NotifyAudioFinished(key);
			m_soundEmitterVault.Remove(key);
		}

		private bool StopMusicTrack(AudioCueKey key)
		{
			//If music is playing, fade it out
			if (m_musicSoundEmitter != null && m_musicSoundEmitter.IsPlaying)
			{
				m_musicSoundEmitter.FadeMusicOut(m_fadeOutDuration);
				return true;
			}
			return false;
		}
	}
}