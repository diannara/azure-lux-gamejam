using UnityEngine;
using UnityEngine.Events;

using Diannara.ScriptableObjects.SceneManagement;

namespace Diannara.ScriptableObjects.Channels
{
	[CreateAssetMenu(fileName = "SceneLoadEventChannel.asset", menuName = "Diannara/Channels/Scene Load Event Channel")]
	public class SceneLoadEventChannel : ScriptableObject
	{
		public UnityAction OnSceneTransitionStart;
		public UnityAction OnSceneTransitionEnd;
		public UnityAction<GameScene, bool> OnSceneUnload;
		public UnityAction<GameScene, bool> OnSceneLoad;

		public void RaiseSceneUnloadEvent(GameScene scene, bool wasPersistent)
		{
			OnSceneLoad?.Invoke(scene, wasPersistent);
		}

		public void RaiseSceneLoadEvent(GameScene scene, bool wasPersistent)
		{
			OnSceneLoad?.Invoke(scene, wasPersistent);
		}

		public void RaiseSceneTransitionStartEvent()
		{
			OnSceneTransitionStart?.Invoke();
		}

		public void RaiseSceneTransitionEndEvent()
		{
			OnSceneTransitionEnd?.Invoke();
		}
	}
}