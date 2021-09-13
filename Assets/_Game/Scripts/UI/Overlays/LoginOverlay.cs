using UnityEngine;

using TMPro;

using PlayFab;
using PlayFab.ClientModels;

using Diannara.Enums;
using Diannara.ScriptableObjects.Channels;

namespace Diannara.UI.Overlays
{
    public class LoginOverlay : Overlay
    {
        [Header("Channels")]
        [SerializeField] private OverlayRequestChannel m_overlayRequestChannel;

        [Header("UI")]
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TMP_InputField passwordInputField;

		public override void Show()
		{
            usernameInputField.text = "";
            passwordInputField.text = "";

            base.Show();
		}

		public void OnCloseClicked()
        {
            m_overlayRequestChannel?.CloseOverlay(OverlayScreen.Login);
        }

        public void OnLoginClicked()
        {
            string username = usernameInputField.text;
            string password = passwordInputField.text;

            if (string.IsNullOrEmpty(username))
            {
                m_overlayRequestChannel?.OpenDialog("Username is requiured!");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                m_overlayRequestChannel?.OpenDialog("Password is requiured!");
                return;
            }

            var request = new LoginWithPlayFabRequest {
                Username = username,
                Password = password
            };
            PlayFabClientAPI.LoginWithPlayFab(request, LoginSuccess, LoginFailed);
        }

        private void LoginSuccess(LoginResult result)
		{
            Debug.Log("Success: " + result);
            m_overlayRequestChannel?.CloseAllOverlays();
            m_overlayRequestChannel?.OpenOverlay(OverlayScreen.MainMenu);
        }

        private void LoginFailed(PlayFabError error)
		{
            m_overlayRequestChannel?.OpenDialog($"Login failed! {error.ErrorMessage}");
        }

        public void OnRegisterClicked()
        {
            m_overlayRequestChannel?.OpenOverlay(OverlayScreen.Register, false, true);
        }
    }
}