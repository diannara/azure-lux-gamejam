using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Diannara.ScriptableObjects.SceneManagement
{
	[CreateAssetMenu(fileName = "GameScene.asset", menuName = "Diannara/Scene Management/Game Scene")]
	public class GameScene : ScriptableObject
	{
		public AssetReference SceneReference;
	}
}