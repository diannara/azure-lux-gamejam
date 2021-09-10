using UnityEngine;

namespace Diannara.ScriptableObjects.Audio
{
    public enum SequenceMode
    {
        Random,
        RandomNoImmediateRepeat,
        Sequential
    }

    [CreateAssetMenu(fileName = "AudioCue.asset", menuName = "Diannara/Audio/Audio Cue")]
    public class AudioCue : ScriptableObject
    {
        [Header("Settings")]
        public SequenceMode SequenceMode = SequenceMode.RandomNoImmediateRepeat;
        public bool IsLooping = false;

        [Header("Pitch")]
        public bool RandomizePitch = false;
        [MinMaxSlider(0.5f, 3.0f)] public Vector2 PitchRange = new Vector2(1.0f, 1.0f);

        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] m_audioClips = default;

        private int m_nextClipToPlay = -1;
        private int m_lastClipPlayed = -1;

        public AudioClip GetNextClip()
        {
            if (m_audioClips.Length <= 0)
            {
                return null;
            }

            if (m_audioClips.Length == 1)
            {
                return m_audioClips[0];
            }

            if (m_nextClipToPlay == -1)
            {
                m_nextClipToPlay = (SequenceMode == SequenceMode.Sequential) ? 0 : Random.Range(0, m_audioClips.Length);
            }
            else
            {
                switch (SequenceMode)
                {
                    case SequenceMode.Random:
                        m_nextClipToPlay = Random.Range(0, m_audioClips.Length);
                        break;

                    case SequenceMode.RandomNoImmediateRepeat:
                        do
                        {
                            m_nextClipToPlay = Random.Range(0, m_audioClips.Length);
                        }
                        while (m_nextClipToPlay == m_lastClipPlayed);
                        break;

                    case SequenceMode.Sequential:
                        m_nextClipToPlay = (int)Mathf.Repeat(++m_nextClipToPlay, m_audioClips.Length);
                        break;
                }
            }

            m_lastClipPlayed = m_nextClipToPlay;
            return m_audioClips[m_nextClipToPlay];
        }
    }
}