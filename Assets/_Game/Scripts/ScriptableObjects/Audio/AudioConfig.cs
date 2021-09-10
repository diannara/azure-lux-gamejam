using UnityEngine;
using UnityEngine.Audio;

namespace Diannara.ScriptableObjects.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig.asset", menuName = "Diannara/Audio/Audio Config")]
    public class AudioConfig : ScriptableObject
    {
        private enum PriorityLevel
        {
            Highest = 0,
            Higher = 64,
            Standard = 128,
            Low = 194,
            VeryLow = 256,
        }

        public AudioMixerGroup OutputAudioMixerGroup;

        [SerializeField] private PriorityLevel m_priorityLevel = PriorityLevel.Standard;
        [HideInInspector]
        public int Priority
        {
            get { return (int)m_priorityLevel; }
            set { m_priorityLevel = (PriorityLevel)value; }
        }

        [Header("Sound Properties")]
        public bool Mute = false;
        [Range(0.0f, 1.0f)] public float Volume = 1.0f;
        [Range(-3.0f, 3.0f)] public float Pitch = 1.0f;
        [Range(-1.0f, 1.0f)] public float PanStereo = 0.0f;
        [Range(0.0f, 1.1f)] public float ReverbZoneMix = 1.0f;

        [Header("Spatialisation")]
        [Range(0.0f, 1.0f)] public float SpatialBlend = 1f;
        public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
        [Range(0.1f, 5.0f)] public float MinDistance = 0.1f;
        [Range(5.0f, 100.0f)] public float MaxDistance = 50f;
        [Range(0, 360)] public int Spread = 0;
        [Range(0.0f, 5.0f)] public float DopplerLevel = 1f;

        [Header("Ignores")]
        public bool BypassEffects = false;
        public bool BypassListenerEffects = false;
        public bool BypassReverbZones = false;
        public bool IgnoreListenerVolume = false;
        public bool IgnoreListenerPause = false;

        public void ApplyTo(AudioSource audioSource)
        {
            audioSource.outputAudioMixerGroup = this.OutputAudioMixerGroup;
            audioSource.mute = this.Mute;
            audioSource.bypassEffects = this.BypassEffects;
            audioSource.bypassListenerEffects = this.BypassListenerEffects;
            audioSource.bypassReverbZones = this.BypassReverbZones;
            audioSource.priority = this.Priority;
            audioSource.volume = this.Volume;
            audioSource.pitch = this.Pitch;
            audioSource.panStereo = this.PanStereo;
            audioSource.spatialBlend = this.SpatialBlend;
            audioSource.reverbZoneMix = this.ReverbZoneMix;
            audioSource.dopplerLevel = this.DopplerLevel;
            audioSource.spread = this.Spread;
            audioSource.rolloffMode = this.RolloffMode;
            audioSource.minDistance = this.MinDistance;
            audioSource.maxDistance = this.MaxDistance;
            audioSource.ignoreListenerVolume = this.IgnoreListenerVolume;
            audioSource.ignoreListenerPause = this.IgnoreListenerPause;
        }
    }
}