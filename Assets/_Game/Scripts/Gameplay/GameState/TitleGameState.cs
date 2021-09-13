using Diannara.Enums;
using Diannara.ScriptableObjects.Channels;

namespace Diannara.Gameplay.GameState
{
	public class TitleGameState : BaseGameState
	{
		private GameStateManager m_gameStateManager;
		private OverlayRequestChannel m_overlayRequestChannel;

		public TitleGameState(GameStateManager gameStateManager, OverlayRequestChannel channel)
		{
			m_gameStateManager = gameStateManager;
			m_overlayRequestChannel = channel;
		}

		public override GameStateType GetGameStateType()
		{
			return GameStateType.Title;
		}

		public override void OnStateEnter()
		{
			m_gameStateManager.Player.SetActive(false);

			m_overlayRequestChannel.CloseAllOverlays();
			m_overlayRequestChannel.OpenOverlay(OverlayScreen.MainMenu, true, false);
		}

		public override void OnStateExit()
		{
			m_overlayRequestChannel.CloseOverlay(OverlayScreen.MainMenu);
		}
	}
}