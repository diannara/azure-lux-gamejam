using UnityEngine;
using UnityEngine.UI;

using TMPro;

using PlayFab;
using PlayFab.ClientModels;

using Diannara.Enums;
using Diannara.ScriptableObjects.Channels;

namespace Diannara.UI.Overlays
{
	public class TitleOverlay : Overlay
	{
		[Header("Channels")]
		[SerializeField] private GameStateHandler m_gameStateEventChannel;
		[SerializeField] private OverlayRequestChannel m_overlayRequestChannel;

		[Header("UI")]
		[SerializeField] private TMP_Text m_loginText;
		[SerializeField] private Button m_loginButton;
		[SerializeField] private Button m_logoutButton;

		private void GetAccountInfo()
		{
			PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
			result => {
				UpdateLoginUI(true, result.AccountInfo.Username);
			},
			error => Debug.LogError(error.GenerateErrorReport()));
		}

		private void GetPlayerProfile()
		{
			PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
			{
				ProfileConstraints = new PlayerProfileViewConstraints()
				{
					ShowDisplayName = true
				}
			},
			result => {
				string username = result.PlayerProfile.DisplayName;
				if(string.IsNullOrEmpty(username))
				{
					GetAccountInfo();
					return;
				}
				UpdateLoginUI(true, username);
			},
			error => Debug.LogError(error.GenerateErrorReport()));
		}

		public override void Show()
		{
			if (PlayFabClientAPI.IsClientLoggedIn())
			{
				GetPlayerProfile();
			}
			else
			{
				UpdateLoginUI(false, null);
			}

			base.Show();
		}

		public void OnPlayButtonClicked()
		{
			if(m_gameStateEventChannel != null)
			{
				m_gameStateEventChannel.RequestGameStateChange(GameStateType.Playing);
			}
		}

		public void OnHowToPlayButtonClicked()
		{
			m_overlayRequestChannel?.OpenOverlay(OverlayScreen.HowToPlay, false);
		}

		public void OnHighScoresButtonClicked()
		{
			m_overlayRequestChannel?.OpenOverlay(OverlayScreen.HighScores, false);
		}

		public void OnSettingsButtonClicked()
		{
			m_overlayRequestChannel?.OpenOverlay(OverlayScreen.Settings, false);
		}

		public void OnCreditsButtonClicked()
		{
			m_overlayRequestChannel?.OpenOverlay(OverlayScreen.Credits, false);
		}

		public void OnLoginButtonClicked()
		{
			m_overlayRequestChannel?.OpenOverlay(OverlayScreen.Login, false, false);
		}

		public void OnLogoutButtonClicked()
		{
			PlayFabClientAPI.ForgetAllCredentials();
			UpdateLoginUI(false, null);
		}

		private void UpdateLoginUI(bool isLoggedIn, string username)
		{
			if(isLoggedIn)
			{
				m_loginButton.gameObject.SetActive(false);
				m_logoutButton.gameObject.SetActive(true);

				m_loginText.text = $"Welcome, {username}";
			}
			else
			{
				m_loginButton.gameObject.SetActive(true);
				m_logoutButton.gameObject.SetActive(false);

				m_loginText.text = "Play as Guest";
			}
		}

		public void OnQuitButtonClicked()
		{
			Application.Quit();
		}
	}
}