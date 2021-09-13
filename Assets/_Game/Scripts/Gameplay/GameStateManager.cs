using System.Collections.Generic;

using UnityEngine;

using Diannara.Enums;
using Diannara.Gameplay.GameState;
using Diannara.ScriptableObjects.Channels;
using Diannara.ScriptableObjects.Variables;

namespace Diannara.Gameplay
{
	public class GameStateManager : MonoBehaviour
	{
		[Header("Channels")]
		[SerializeField] private GameStateHandler m_gameStateHandler;
		[SerializeField] private DeathEventChannel m_deathEventChannel;
		[SerializeField] private OverlayRequestChannel m_overlayRequestChannel;

		[Header("Variables")]
		[SerializeField] private IntVariable m_score;

		[Header("References")]
		[SerializeField] private GameObject m_player;

		private Dictionary<GameStateType, IGameState> m_states = new Dictionary<GameStateType, IGameState>();

		private IGameState m_currentGameState;

		private GameStateType m_curentGameStateType = GameStateType.Startup;
		private GameStateType m_previousGameStateType = GameStateType.Startup;

		public GameObject Player => m_player;
		public GameStateType CurentGameStateType => m_curentGameStateType;
		public GameStateType PreviousGameStateType => m_previousGameStateType;


		private void HandleGameStateChangeRequest(GameStateType state)
		{
			SetState(state);
		}

		private void ScorePoints(int points)
		{
			m_score.Add(points);
		}

		private void OnDeathEvent(CharacterType type)
		{
			if(type == CharacterType.Player)
			{
				OnPlayerDeath();
			}
			else
			{
				ScorePoints(10);
			}
		}

		private void OnDisable()
		{
			if(m_deathEventChannel != null)
			{
				m_deathEventChannel.OnDeathEvent -= OnDeathEvent;
			}

			if(m_gameStateHandler != null)
			{
				m_gameStateHandler.OnGameStateChangeRequest -= HandleGameStateChangeRequest;
			}
		}

		private void OnEnable()
		{
			if (m_deathEventChannel != null)
			{
				m_deathEventChannel.OnDeathEvent += OnDeathEvent;
			}

			if (m_gameStateHandler != null)
			{
				m_gameStateHandler.OnGameStateChangeRequest += HandleGameStateChangeRequest;
			}
		}

		private void OnPlayerDeath()
		{
			SetState(GameStateType.GameOver);
		}

		private void SetState(GameStateType state)
		{
			if (m_currentGameState != null)
			{
				// Debug.Log($"GameStateManager :: SetState() :: Leaving {m_currentGameState.GetGameStateType()}");
				m_previousGameStateType = m_currentGameState.GetGameStateType();
				m_currentGameState.OnStateExit();
			}

			if (m_states.TryGetValue(state, out IGameState newGameState))
			{
				// Debug.Log($"GameStateManager :: SetState() :: Entering {newGameState.GetGameStateType()}");
				m_curentGameStateType = newGameState.GetGameStateType();
				m_currentGameState = newGameState;
				m_currentGameState.OnStateEnter();

				m_gameStateHandler?.RaiseGameStateChangedEvent(m_curentGameStateType, m_previousGameStateType);
			}
		}

		private void Start()
		{
			TitleGameState titleState = new TitleGameState(this, m_overlayRequestChannel);
			PlayingGameState playingState = new PlayingGameState(this, m_overlayRequestChannel, m_score);
			PausedGameState pausedState = new PausedGameState(this);
			GameOverGameState gameOverState = new GameOverGameState(this, m_overlayRequestChannel);

			m_states.Add(GameStateType.Title, titleState);
			m_states.Add(GameStateType.Playing, playingState);
			m_states.Add(GameStateType.Paused, pausedState);
			m_states.Add(GameStateType.GameOver, gameOverState);

			SetState(GameStateType.Title);
		}
	}
}