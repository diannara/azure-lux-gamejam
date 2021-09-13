using Diannara.Enums;
using Diannara.ScriptableObjects.Channels;

namespace Diannara.Gameplay.GameState
{
	public class GameOverGameState : BaseGameState
	{
		private GameStateManager m_gameStateManager;
		private OverlayRequestChannel m_overlayRequestChannel;

		public GameOverGameState(GameStateManager gameStateManager, OverlayRequestChannel channel)
		{
			m_gameStateManager = gameStateManager;
			m_overlayRequestChannel = channel;
		}

		public override GameStateType GetGameStateType()
		{
			return GameStateType.GameOver;
		}

		public override void OnStateEnter()
		{
			m_gameStateManager.Player.SetActive(false);
			m_overlayRequestChannel.OpenOverlay(OverlayScreen.GameOver);
		}

		public override void OnStateExit()
		{
			m_overlayRequestChannel.CloseOverlay(OverlayScreen.GameOver);
		}
	}
}