using UnityEngine;

using Diannara.Enums;

namespace Diannara.Gameplay.GameState
{
	public class PausedGameState : BaseGameState
	{
		private GameStateManager m_gameStateManager;

		public PausedGameState(GameStateManager gameStateManager)
		{
			m_gameStateManager = gameStateManager;
		}

		public override GameStateType GetGameStateType()
		{
			return GameStateType.Paused;
		}

		public override void OnStateEnter()
		{
			Time.timeScale = 0.0f;
		}

		public override void OnStateExit()
		{
			Time.timeScale = 1.0f;
		}
	}
}