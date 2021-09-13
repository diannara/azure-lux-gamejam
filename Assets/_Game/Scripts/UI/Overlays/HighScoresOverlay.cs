using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

using Diannara.Enums;
using Diannara.ScriptableObjects.Channels;

namespace Diannara.UI.Overlays
{
	public class HighScoresOverlay : Overlay
	{
        [Header("Channels")]
        [SerializeField] private OverlayRequestChannel m_overlayRequestChannel;

        [Header("UI")]
		[SerializeField] private HighScoreUI[] m_highscores;
        [SerializeField] private GameObject m_scoresPanel;
        [SerializeField] private GameObject m_loginMessagePanel;

        public override void Show()
		{
            ShowLeaderboard();
			base.Show();
		}

		public void ShowLeaderboard()
        {
            m_scoresPanel.SetActive(false);
            m_loginMessagePanel.SetActive(false);

            if (PlayFabClientAPI.IsClientLoggedIn())
			{
                m_scoresPanel.SetActive(true);

                var request = new GetLeaderboardRequest
                {
                    StatisticName = "Scores",
                    StartPosition = 0,
                    MaxResultsCount = 10
                };
                PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, LeaderboardError);
            }
            else
			{
                m_loginMessagePanel.SetActive(true);
            }
        }

        public void OnCloseClicked()
        {
            m_overlayRequestChannel?.CloseOverlay(OverlayScreen.HighScores);
        }

        void OnGetLeaderboardSuccess(GetLeaderboardResult result)
        {
            foreach(HighScoreUI highscore in m_highscores)
			{
                highscore.UpdateScoreUI("", "");
            }

            for(int i = 0; i < result.Leaderboard.Count; i++)
            {
                PlayerLeaderboardEntry entry = result.Leaderboard[i];

                HighScoreUI highscore = m_highscores[i];
                highscore.UpdateScoreUI(entry.DisplayName, entry.StatValue.ToString());
            }
        }

        void LeaderboardError(PlayFabError error)
        {
            Debug.Log("Error: " + error);
        }
    }
}