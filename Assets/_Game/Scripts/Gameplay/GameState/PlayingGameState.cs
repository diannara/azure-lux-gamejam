using Diannara.Enums;
using Diannara.Gameplay.Player;
using Diannara.ScriptableObjects.Channels;
using Diannara.ScriptableObjects.Variables;

namespace Diannara.Gameplay.GameState
{
	public class PlayingGameState : BaseGameState
	{
		private GameStateManager m_gameStateManager;
		private OverlayRequestChannel m_overlayRequestChannel;
		private IntVariable m_score;

		public PlayingGameState(GameStateManager gameStateManager, OverlayRequestChannel channel, IntVariable score)
		{
			m_gameStateManager = gameStateManager;
			m_overlayRequestChannel = channel;
			m_score = score;
		}

		public override GameStateType GetGameStateType()
		{
			return GameStateType.Playing;
		}

		public override void OnStateEnter()
		{
			m_overlayRequestChannel.OpenOverlay(OverlayScreen.Gameplay, true);

			if(m_gameStateManager.PreviousGameStateType != GameStateType.Paused)
			{
				m_gameStateManager.Player.SetActive(true);
				PlayerController pc = m_gameStateManager.Player.GetComponent<PlayerController>();
				if(pc != null)
				{
					pc.ResetPlayer();
				}

				m_score.SetValue(0);
			}
		}

		public override void OnStateExit()
		{
			m_overlayRequestChannel.CloseOverlay(OverlayScreen.Gameplay);

			if (m_gameStateManager.CurentGameStateType != GameStateType.Paused)
			{
				m_gameStateManager.Player.SetActive(false);
			}
		}
	}
}