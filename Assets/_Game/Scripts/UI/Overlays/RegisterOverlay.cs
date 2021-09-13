using UnityEngine;

using TMPro;

using PlayFab;
using PlayFab.ClientModels;

using Diannara.Enums;
using Diannara.ScriptableObjects.Channels;

namespace Diannara.UI.Overlays
{
    public class RegisterOverlay : Overlay
    {
        [Header("Channels")]
        [SerializeField] private OverlayRequestChannel m_overlayRequestChannel;

        [Header("UI")]
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_InputField confirmPasswordInputField;
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField confirmEmailInputField;

		public override void Show()
		{
            usernameInputField.text = "";
            passwordInputField.text = "";
            confirmPasswordInputField.text = "";
            emailInputField.text = "";
            confirmEmailInputField.text = "";

            base.Show();
		}

		public void OnCloseClicked()
		{
            m_overlayRequestChannel?.CloseOverlay(OverlayScreen.Register);
        }

        public void OnRegisterClicked()
        {
            RegisterPlayFabAccount();
        }

        public void RegisterPlayFabAccount()
        {
            string username = usernameInputField.text;
            string password = passwordInputField.text;
            string confirmPassword = confirmPasswordInputField.text;
            string email = emailInputField.text;
            string confirmEmail = confirmEmailInputField.text;

            if(string.IsNullOrEmpty(username))
			{
                m_overlayRequestChannel?.OpenDialog("Username is requiured!");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                m_overlayRequestChannel?.OpenDialog("Password is requiured!");
                return;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                m_overlayRequestChannel?.OpenDialog("Password confirmation is requiured!");
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                m_overlayRequestChannel?.OpenDialog("Email address is requiured!");
                return;
            }

            if (string.IsNullOrEmpty(confirmEmail))
            {
                m_overlayRequestChannel?.OpenDialog("Email address confirmation is requiured!");
                return;
            }

            if (password.CompareTo(confirmPassword) != 0)
			{
                m_overlayRequestChannel?.OpenDialog("Passwords do not match!");
                return;
            }

            if(email.CompareTo(confirmEmail) != 0)
			{
                m_overlayRequestChannel?.OpenDialog("Emails do not match!");
                return;
			}

            RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest {
                Email = emailInputField.text,
                Password = passwordInputField.text,
                Username = usernameInputField.text,
                DisplayName = usernameInputField.text,
            };
            PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterFailed);
        }

        private void RegisterSuccess(RegisterPlayFabUserResult result)
        {
            m_overlayRequestChannel?.CloseAllOverlays();
            m_overlayRequestChannel?.OpenOverlay(OverlayScreen.MainMenu);
        }

        private void RegisterFailed(PlayFabError error)
        {
            m_overlayRequestChannel?.OpenDialog($"Account registration failed! {error.ErrorMessage}");
        }
    }
}