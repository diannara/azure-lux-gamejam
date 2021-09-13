using UnityEngine;
using UnityEngine.Events;

using Diannara.Enums;

namespace Diannara.ScriptableObjects.Channels
{
    [CreateAssetMenu(fileName = "OverlayRequestChanel.asset", menuName = "Diannara/Overlays/Overlay Request Channel")]
    public class OverlayRequestChannel : ScriptableObject
    {
        public UnityAction OnCloseAllOverlaysRequest;
        public UnityAction OnClearOverlayStackRequest;
        public UnityAction<OverlayScreen, bool, bool> OnOpenOverlayRequest;
        public UnityAction<OverlayScreen> OnCloseOverlayRequest;

        public UnityAction<string> OnOpenDialogRequest;

        public void OpenDialog(string message)
        {
            OnOpenDialogRequest?.Invoke(message);
        }

        public void OpenOverlay(OverlayScreen overlay, bool closePreviouse = false, bool hidePrevious = false)
        {
            OnOpenOverlayRequest?.Invoke(overlay, closePreviouse, hidePrevious);
        }

        public void CloseOverlay(OverlayScreen overlay)
        {
            OnCloseOverlayRequest?.Invoke(overlay);
        }

        public void CloseAllOverlays()
		{
            OnCloseAllOverlaysRequest?.Invoke();
		}

        public void ClearOverlayStack()
        {
            OnClearOverlayStackRequest?.Invoke();
        }
    }
}