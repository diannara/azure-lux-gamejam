using UnityEngine;
using UnityEngine.Events;

namespace Diannara.ScriptableObjects.Channels
{
    [CreateAssetMenu(fileName = "VoidEventChannel.asset", menuName = "Diannara/Channels/Void Event Channel")]
    public class VoidEventChannel : ScriptableObject
    {
        public UnityAction OnEventRaised;

        public void RaiseEvent()
        {
            if (OnEventRaised != null)
            {
                OnEventRaised.Invoke();
            }
            else
            {
                Debug.LogWarning("A VoidEvent was raised, but nobody picked it up.");
            }
        }
    }
}