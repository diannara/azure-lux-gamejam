using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

using Diannara.ScriptableObjects.SceneManagement;
using Diannara.ScriptableObjects.Channels;

namespace Diannara.SceneManagement
{
	public class InitializationLoader : MonoBehaviour
	{
		[Header("Scenes")]
		[SerializeField] private GameScene m_sceneToInitalize;
		[SerializeField] private GameScene[] m_scenesToLoad;

		[Header("Channels")]
		[SerializeField] private AssetReference m_sceneLoadRequestChannelSO;

		private void Start()
		{
			m_sceneToInitalize.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += OnSceneLoadCompleted;
		}

		private void OnSceneLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
		{
			m_sceneLoadRequestChannelSO.LoadAssetAsync<SceneLoadRequestChannel>().Completed += OnSceneRequestChannelLoadCompleted;
		}

		private void OnSceneRequestChannelLoadCompleted(AsyncOperationHandle<SceneLoadRequestChannel> obj)
		{
			SceneLoadRequestChannel sceneLoadRequestChannel = (SceneLoadRequestChannel)m_sceneLoadRequestChannelSO.Asset;
			sceneLoadRequestChannel.RequestSceneLoad(m_scenesToLoad);

			// We can now unload the initialization screen
			SceneManager.UnloadSceneAsync(0);
		}
	}
}