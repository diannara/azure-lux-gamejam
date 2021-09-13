using Diannara.Enums;

namespace Diannara.Gameplay.GameState
{
	public interface IGameState
	{
		public GameStateType GetGameStateType();

		public void OnStateEnter();

		public void OnStateExit();
	}
}