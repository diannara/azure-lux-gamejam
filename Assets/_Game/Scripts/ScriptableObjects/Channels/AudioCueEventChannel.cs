using UnityEngine;
using UnityEngine.Events;

using Diannara.Audio;
using Diannara.ScriptableObjects.Audio;

namespace Diannara.ScriptableObjects.Channels
{
    [CreateAssetMenu(fileName = "AudioCueEventChannel.asset", menuName = "Diannara/Channels/Audio Cue Event Channel")]
    public class AudioCueEventChannel : ScriptableObject
    {
        public delegate AudioCueKey AudioCuePlayAction(AudioCue audioCue, AudioConfig audioConfiguration, Vector3 positionInSpace);
        public delegate bool AudioCueStopAction(AudioCueKey emitterKey);
        public delegate bool AudioCueFinishAction(AudioCueKey emitterKey);

        public AudioCueFinishAction OnAudioCueFinishRequested;
        public AudioCuePlayAction OnAudioCuePlayRequested;
        public AudioCueStopAction OnAudioCueStopRequested;

        public UnityAction<AudioCueKey> OnAudioFinished;

        public AudioCueKey RequestAudio(AudioCue audioCue, AudioConfig audioConfigration, Vector3 positionInSpace = default)
        {
            AudioCueKey key = AudioCueKey.Invalid;

            if (OnAudioCuePlayRequested != null)
            {
                key = OnAudioCuePlayRequested.Invoke(audioCue, audioConfigration, positionInSpace);
            }
            else
            {
                Debug.LogWarning("AudioCueEventChannel :: RequestAudio() :: An AudioCue request was raised, but nobody was listening!");
            }

            return key;
        }

        public bool FinishAudio(AudioCueKey key)
        {
            bool requestSuccessful = false;

            if (OnAudioCueFinishRequested != null)
            {
                requestSuccessful = OnAudioCueFinishRequested.Invoke(key);
            }
            else
            {
                Debug.LogWarning("AudioCueEventChannel :: FinishAudio() :: A finish audio request was raised, but nobody was listening!");
            }

            return requestSuccessful;
        }

        public bool StopAudio(AudioCueKey key)
        {
            bool requestSuccessful = false;

            if (OnAudioCueStopRequested != null)
            {
                requestSuccessful = OnAudioCueStopRequested.Invoke(key);
            }
            else
            {
                Debug.LogWarning("AudioCueEventChannel :: StopAudio() :: A stop audio request was raised, but nobody was listening!");
            }

            return requestSuccessful;
        }

        public void NotifyAudioFinished(AudioCueKey key)
        {
            if (OnAudioFinished != null)
            {
                OnAudioFinished?.Invoke(key);
            }
        }
    }
}