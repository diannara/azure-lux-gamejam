using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

using Diannara.ScriptableObjects.SceneManagement;
using Diannara.ScriptableObjects.Channels;

namespace SparkleParty.RootJam.Managers
{
	public class SceneLoader : MonoBehaviour
	{
		[Header("Events")]
		[SerializeField] private SceneLoadEventChannel m_sceneLoadEventChannel;
		// [SerializeField] private LoadingScreenEventChannel m_loadingScreenEventChannel;

		[Header("Channels")]
		[SerializeField] private SceneLoadRequestChannel m_sceneChangeRequestChannel;
		// [SerializeField] private LoadingScreenRequestChannel m_loadingScreenRequestChannel;

		private List<AsyncOperationHandle<SceneInstance>> m_loadingOperationHandles = new List<AsyncOperationHandle<SceneInstance>>();

		private GameScene[] m_currentlyLoadedScenes = new GameScene[] { };
		private GameScene[] m_scenesToLoad;

		private void OnDisable()
		{
			if (m_sceneChangeRequestChannel != null)
			{
				m_sceneChangeRequestChannel.OnSceneLoadRequest -= OnSceneLoadRequest;
			}

			/*
			if (m_loadingScreenEventChannel != null)
			{
				m_loadingScreenEventChannel.OnLoadingScreenOpenEvent -= OnLoadingScreenOpenEvent;
				m_loadingScreenEventChannel.OnLoadingScreenCloseEvent -= OnLoadingScreenCloseEvent;
			}
			*/
		}

		private void OnEnable()
		{
			if (m_sceneChangeRequestChannel != null)
			{
				m_sceneChangeRequestChannel.OnSceneLoadRequest += OnSceneLoadRequest;
			}

			/*
			if (m_loadingScreenEventChannel != null)
			{
				m_loadingScreenEventChannel.OnLoadingScreenOpenEvent += OnLoadingScreenOpenEvent;
				m_loadingScreenEventChannel.OnLoadingScreenCloseEvent += OnLoadingScreenCloseEvent;
			}
			*/
		}

		private void OnLoadingScreenOpenEvent()
		{
			HandleSceneLoading();
		}

		private void OnLoadingScreenCloseEvent()
		{
		}

		private void OnSceneLoadRequest(GameScene[] scenes)
		{
			// Debug.Log($"SceneLoader :: OnSceneChangeRequest() :: Request to load new scenes recieved...");
			m_scenesToLoad = scenes;
			HandleSceneLoading();
		}

		private void HandleSceneLoading()
		{
			if (m_scenesToLoad.Length <= 0)
			{
				Debug.LogWarning("SceneLoader :: HandleSceneLoading() :: No scenes to load...", this);
				return;
			}

			m_sceneLoadEventChannel?.RaiseSceneTransitionStartEvent();

			StartCoroutine(UnloadScenes());
		}

		private void LoadNewScenes()
		{
			m_loadingOperationHandles.Clear();
			foreach (GameScene scene in m_scenesToLoad)
			{
				m_loadingOperationHandles.Add(scene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0));
			}
			StartCoroutine(LoadSceneProcess());
		}

		private IEnumerator LoadSceneProcess()
		{
			bool doneLoading = (m_loadingOperationHandles.Count == 0);

			while (!doneLoading)
			{
				foreach (AsyncOperationHandle handle in m_loadingOperationHandles)
				{
					if (handle.Status != AsyncOperationStatus.Succeeded)
					{
						break;
					}
					else
					{
						doneLoading = true;
					}
				}
				yield return null;
			}

			m_currentlyLoadedScenes = m_scenesToLoad;

			SetActiveScene();
		}

		private IEnumerator UnloadScenes()
		{
			//Skip one frame so the loading screen has a chance to render
			yield return null;

			foreach (GameScene scene in m_currentlyLoadedScenes)
			{
				scene.SceneReference.UnLoadScene();
			}

			LoadNewScenes();
		}

		private void SetActiveScene()
		{
			Scene scene = ((SceneInstance)m_loadingOperationHandles[0].Result).Scene;
			SceneManager.SetActiveScene(scene);

			LightProbes.TetrahedralizeAsync();

			m_sceneLoadEventChannel?.RaiseSceneTransitionEndEvent();
		}
	}
}