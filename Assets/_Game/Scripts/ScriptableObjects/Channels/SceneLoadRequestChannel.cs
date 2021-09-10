using UnityEngine;
using UnityEngine.Events;

using Diannara.ScriptableObjects.SceneManagement;

namespace Diannara.ScriptableObjects.Channels
{
	[CreateAssetMenu(fileName = "SceneLoadRequestChannel.asset", menuName = "Diannara/Channels/Scene Load Request Channel")]
	public class SceneLoadRequestChannel : ScriptableObject
	{
		public UnityAction<GameScene[]> OnSceneLoadRequest;

		public void RequestSceneLoad(GameScene[] scenes)
		{
			if (OnSceneLoadRequest != null)
			{
				OnSceneLoadRequest.Invoke(scenes);
			}
			else
			{
				Debug.LogWarning("SceneLoadRequestChannel :: RequestSceneLoad() :: Scene load request was sent but no one was listening for it!");
			}
		}
	}
}