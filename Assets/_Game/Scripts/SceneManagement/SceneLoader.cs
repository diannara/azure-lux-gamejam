using UnityEngine;
using UnityEngine.SceneManagement;

namespace Diannara.Scenes
{
	public class SceneLoader : MonoBehaviour
	{
		private void Start()
		{
			SceneManager.LoadScene(1);
		}
	}
}