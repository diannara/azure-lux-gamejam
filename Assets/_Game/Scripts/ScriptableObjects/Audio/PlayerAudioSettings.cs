using UnityEngine;
using UnityEngine.Events;

namespace Diannara.ScriptableObjects.Audio
{
	[CreateAssetMenu(fileName = "PlayerAudioSettings.asset", menuName = "Diannara/Audio/Player Audio Settings")]
	public class PlayerAudioSettings : ScriptableObject
	{
		private static readonly string AUDIO_MASTER_VOLUME_PPKEY = "Audio_MasterVolume";
		private static readonly string AUDIO_MUSIC_VOLUME_PPKEY = "Audio_MusicVolume";
		private static readonly string AUDIO_SFX_VOLUME_PPKEY = "Audio_SfxVolume";
		private static readonly string AUDIO_UI_VOLUME_PPKEY = "Audio_UIVolume";

		public UnityAction<float, float, float, float> OnValuesSaved;
		public UnityAction<float> OnMasterVolumeChanged;
		public UnityAction<float> OnMusicVolumeChanged;
		public UnityAction<float> OnSfxVolumeChanged;
		public UnityAction<float> OnUiVolumeChanged;

		[SerializeField] private float m_masterVolume;
		[SerializeField] private float m_musicVolume;
		[SerializeField] private float m_sfxVolume;
		[SerializeField] private float m_uiVolume;

		public float MasterVolume => m_masterVolume;
		public float MusicVolume => m_musicVolume;
		public float SfxVolume => m_sfxVolume;
		public float UiVolume => m_uiVolume;

		public void LoadFromPlayerPrefs()
		{
			m_masterVolume = PlayerPrefs.GetFloat(AUDIO_MASTER_VOLUME_PPKEY, 1.0f);
			m_musicVolume = PlayerPrefs.GetFloat(AUDIO_MUSIC_VOLUME_PPKEY, 1.0f);
			m_sfxVolume = PlayerPrefs.GetFloat(AUDIO_SFX_VOLUME_PPKEY, 1.0f);
			m_uiVolume = PlayerPrefs.GetFloat(AUDIO_UI_VOLUME_PPKEY, 1.0f);
		}

		public void SaveToPlayerPrefs()
		{
			PlayerPrefs.SetFloat(AUDIO_MASTER_VOLUME_PPKEY, m_masterVolume);
			PlayerPrefs.SetFloat(AUDIO_MUSIC_VOLUME_PPKEY, m_musicVolume);
			PlayerPrefs.SetFloat(AUDIO_SFX_VOLUME_PPKEY, m_sfxVolume);
			PlayerPrefs.SetFloat(AUDIO_UI_VOLUME_PPKEY, m_uiVolume);

			PlayerPrefs.Save();

			OnValuesSaved?.Invoke(m_masterVolume, m_musicVolume, m_sfxVolume, m_uiVolume);
		}

		public void SetMasterVolume(float volume)
		{
			m_masterVolume = volume;
			OnMasterVolumeChanged?.Invoke(m_masterVolume);
		}

		public void SetMusicVolume(float volume)
		{
			m_musicVolume = volume;
			OnMusicVolumeChanged?.Invoke(m_musicVolume);
		}

		public void SetSfxVolume(float volume)
		{
			m_sfxVolume = volume;
			OnSfxVolumeChanged?.Invoke(m_sfxVolume);
		}

		public void SetUiVolume(float volume)
		{
			m_uiVolume = volume;
			OnUiVolumeChanged?.Invoke(m_uiVolume);
		}
	}
}