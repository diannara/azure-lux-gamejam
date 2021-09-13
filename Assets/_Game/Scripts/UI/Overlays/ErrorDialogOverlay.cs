using UnityEngine;

using TMPro;

using Diannara.Enums;
using Diannara.ScriptableObjects.Channels;

namespace Diannara.UI.Overlays
{
	public class ErrorDialogOverlay : Overlay
	{
		[Header("Channels")]
		[SerializeField] private OverlayRequestChannel overlayRequestChannel;

		[Header("UI")]
		[SerializeField] private TMP_Text m_messageText;

		public void OnClosedButtonClicked()
		{
			overlayRequestChannel?.CloseOverlay(OverlayScreen.ErrorDialog);
		}

		public void SetMessage(string message)
		{
			m_messageText.text = message;
		}
	}
}