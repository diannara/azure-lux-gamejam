using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

using Diannara.Input;

namespace Diannara.ScriptableObjects.Input
{
	[CreateAssetMenu(fileName = "InputReader.Asset", menuName = "Diannara/Input/Input Reader")]
	public class InputReader : ScriptableObject, InputMaster.IPlayerActions
	{
		public UnityAction<Vector2> OnMoveEvent;
		public UnityAction<bool> OnSwingEvent;

		private InputMaster m_inputMaster;

		public void OnDisable()
		{
			if (m_inputMaster != null)
			{
				m_inputMaster.Player.Disable();
			}
		}

		public void OnEnable()
		{
			if (m_inputMaster == null)
			{
				m_inputMaster = new InputMaster();
				m_inputMaster.Player.SetCallbacks(this);
			}
			m_inputMaster.Player.Enable();
		}

		public void OnMovement(InputAction.CallbackContext context)
		{
			OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
		}

		public void OnSwing(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
			{
				OnSwingEvent?.Invoke(true);
			}

			if (context.phase == InputActionPhase.Canceled)
			{
				OnSwingEvent?.Invoke(false);
			}
		}
	}
}