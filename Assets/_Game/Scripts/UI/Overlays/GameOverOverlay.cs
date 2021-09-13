using System.Collections.Generic;

using UnityEngine;

using TMPro;

using PlayFab;
using PlayFab.ClientModels;

using Diannara.Enums;
using Diannara.ScriptableObjects.Channels;
using Diannara.ScriptableObjects.Variables;

namespace Diannara.UI.Overlays
{
    public class GameOverOverlay : Overlay
    {
        [Header("UI")]
        [SerializeField] private TMP_Text m_scoreText;

        [Header("Channels")]
        [SerializeField] private GameStateHandler m_gameStateHandler;

        [Header("Variables")]
        [SerializeField] private IntVariable m_score;

        private void LeaderboardUpdateSucces(UpdatePlayerStatisticsResult resutl)
		{
            Debug.Log($"GameOverOverlay :: LeaderboardUpdateSuccess() :: Score sent to leaderboard!");
        }

        private void LeaderboardUpdateFailed(PlayFabError error)
		{
            Debug.Log($"GameOverOverlay :: LeaderboardUpdateFailed() :: {error}");
		}


        public override void Show()
		{
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                SendToLeaderbaord(m_score.Value);
            }

            UpdateScoreTextUI();
            base.Show();
		}

		public void OnMenuClicked()
        {
            m_gameStateHandler?.RequestGameStateChange(GameStateType.Title);
        }

        public void OnRestartClicked()
        {
            m_gameStateHandler?.RequestGameStateChange(GameStateType.Playing);
        }

        private void SendToLeaderbaord(int score)
		{
            UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest {
                Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate { StatisticName = "Scores", Value = m_score.Value },
                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, LeaderboardUpdateSucces, LeaderboardUpdateFailed);
        }            

        private void UpdateScoreTextUI()
		{
            if(m_score == null && m_scoreText == null)
			{
                return;
			}

            m_scoreText.text = $"Final Score: {m_score.Value.ToString()}";
		}
    }
}