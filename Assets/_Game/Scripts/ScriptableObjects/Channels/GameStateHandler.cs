using UnityEngine;
using UnityEngine.Events;

using Diannara.Enums;

namespace Diannara.ScriptableObjects.Channels
{
    [CreateAssetMenu(fileName = "GameStateHandler.asset", menuName = "Diannara/Channels/Game State Handler")]
    public class GameStateHandler : ScriptableObject
    {
        public UnityAction<GameStateType> OnGameStateChangeRequest;
        public UnityAction<GameStateType, GameStateType> OnGameStateChanged;

        public void RequestGameStateChange(GameStateType state)
        {
            if (OnGameStateChangeRequest != null)
            {
                OnGameStateChangeRequest.Invoke(state);
            }
            else
            {
                Debug.LogWarning("A GameStateRequest was raised, but nobody picked it up.");
            }
        }

        public void RaiseGameStateChangedEvent(GameStateType current, GameStateType previous)
        {
            if(OnGameStateChanged != null)
            {
                OnGameStateChanged.Invoke(current, previous);
            }
            else
            {
                Debug.LogWarning("A GameStateChangedEvent was raised, but nobody picked it up.");
            }
        }
    }
}