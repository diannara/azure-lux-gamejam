using Diannara.Enums;

namespace Diannara.Gameplay.GameState
{
	public abstract class BaseGameState : IGameState
	{
		public abstract GameStateType GetGameStateType();

		public abstract void OnStateEnter();

		public abstract void OnStateExit();
	}
}