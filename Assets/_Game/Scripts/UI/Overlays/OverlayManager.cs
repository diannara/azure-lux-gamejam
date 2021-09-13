using System.Collections.Generic;
using UnityEngine;

using Diannara.Enums;
using Diannara.ScriptableObjects.Channels;
using Diannara.UI.Overlays;

namespace Diannara.Invasion.Managers
{
    public class OverlayManager : MonoBehaviour
    {
        [Header("Overlays")]
        [SerializeField] private Overlay[] m_overlays;

        [Header("References")]
        [SerializeField] private Overlay m_currentOverlay;
        [SerializeField] private ErrorDialogOverlay m_errorDialog;

        [Header("Channel")]
        [SerializeField] private OverlayRequestChannel m_overlayRequestChannel;

        private Stack<Overlay> m_overlayStack = new Stack<Overlay>();

        private Dictionary<OverlayScreen, Overlay> m_overlayDictionary = new Dictionary<OverlayScreen, Overlay>();

        private void Awake()
        {
            foreach (Overlay overlay in m_overlays)
            {
                overlay.Hide();
                if (overlay == m_currentOverlay)
                {
                    overlay.Show();
                }

                m_overlayDictionary.Add(overlay.Screen, overlay);
            }
        }

        private void ClearOverlayStack()
        {
            while (m_overlayStack.Count > 0)
            {
                Overlay overlay = m_overlayStack.Pop();
                Close(overlay.Screen);
            }
            m_overlayStack.Clear();
        }

        private void Close(OverlayScreen screen)
        {
            if(m_currentOverlay != null && m_currentOverlay.Screen == screen)
            {
                m_currentOverlay.Hide();
                m_currentOverlay = null;

                if (m_overlayStack.Count > 0 && m_overlayStack.Peek() != null)
                {
                    Overlay previous = m_overlayStack.Pop();
                    m_currentOverlay = previous;
                    m_currentOverlay.Show();
                }
            }
        }

        private void CloseAllOverlays()
		{
            ClearOverlayStack();
            if(m_currentOverlay != null)
            {
                Close(m_currentOverlay.Screen);
            }
        }

        private void Open(OverlayScreen screen, bool closePrevious, bool hidePrevious)
        {
            if (m_currentOverlay != null)
            {
                if(closePrevious)
				{
                    Close(m_currentOverlay.Screen);
                }
                else
				{
                    if (hidePrevious)
                    {
                        m_currentOverlay.Hide();
                    }

                    m_overlayStack.Push(m_currentOverlay);
                }
            }

            if (m_overlayDictionary.TryGetValue(screen, out Overlay overlay))
            {
                if (m_currentOverlay != overlay)
                {
                    overlay.Show();
                    m_currentOverlay = overlay;
                }
                else
                {
                    overlay.Show();
                }
            }
        }

        private void OpenDialog(string message)
		{
            m_errorDialog.SetMessage(message);
            Open(OverlayScreen.ErrorDialog, false, false);
        }

        public void OnDisable()
        {
            if (m_overlayRequestChannel != null)
            {
                m_overlayRequestChannel.OnOpenOverlayRequest -= Open;
                m_overlayRequestChannel.OnCloseOverlayRequest -= Close;
                m_overlayRequestChannel.OnClearOverlayStackRequest -= ClearOverlayStack;
                m_overlayRequestChannel.OnCloseAllOverlaysRequest -= CloseAllOverlays;
                m_overlayRequestChannel.OnOpenDialogRequest -= OpenDialog;
            }
        }

        public void OnEnable()
        {
            if (m_overlayRequestChannel != null)
            {
                m_overlayRequestChannel.OnOpenOverlayRequest += Open;
                m_overlayRequestChannel.OnCloseOverlayRequest += Close;
                m_overlayRequestChannel.OnClearOverlayStackRequest += ClearOverlayStack;
                m_overlayRequestChannel.OnCloseAllOverlaysRequest += CloseAllOverlays;
                m_overlayRequestChannel.OnOpenDialogRequest += OpenDialog;
            }
        }
    }
}