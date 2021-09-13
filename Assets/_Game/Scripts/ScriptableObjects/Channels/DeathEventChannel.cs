using UnityEngine;
using UnityEngine.Events;

using Diannara.Enums;

namespace Diannara.ScriptableObjects.Channels
{
    [CreateAssetMenu(fileName = "DeathEventChannel.asset", menuName = "Diannara/Channels/Death Event Channel")]
    public class DeathEventChannel : ScriptableObject
    {
        public UnityAction<CharacterType> OnDeathEvent;

        public void RaiseDeathEvent(CharacterType type)
        {
            if (OnDeathEvent != null)
            {
                OnDeathEvent.Invoke(type);
            }
            else
            {
                Debug.LogWarning("A DeathEvent was raised, but nobody picked it up.");
            }
        }
    }
}