using UnityEngine;

using Diannara.Enums;

namespace Diannara.UI.Overlays
{
	public class Overlay : MonoBehaviour
	{
		[Header("Overlay Settings")]
		[SerializeField] private OverlayScreen m_screen;

		public OverlayScreen Screen => m_screen;
		public bool IsOpen { get; protected set; }

		public virtual void Show()
		{
			this.gameObject.SetActive(true);
			IsOpen = true;
		}

		public virtual void Hide()
		{
			this.gameObject.SetActive(false);
			IsOpen = false;
		}
	}
}