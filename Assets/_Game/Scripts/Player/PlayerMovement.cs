using UnityEngine;

using Diannara.ScriptableObjects.Input;

namespace Diannara.Player
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerMovement : MonoBehaviour
	{
		[Header("Physics")]
		[SerializeField] private float m_movementForce;

		[Header("References")]
		[SerializeField] private InputReader m_inputReader;

		private Rigidbody2D m_rigidbody;
		private Vector3 m_moveDirection;

		private bool m_isSwinging;

		private void Awake()
		{
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void OnDisable()
		{
			if (m_inputReader != null)
			{
				m_inputReader.OnMoveEvent -= OnMove;
				m_inputReader.OnSwingEvent -= OnSwing;
			}
		}

		private void OnEnable()
		{
			if(m_inputReader != null)
			{
				m_inputReader.OnMoveEvent += OnMove;
				m_inputReader.OnSwingEvent += OnSwing;
			}
		}

		private void OnMove(Vector2 movement)
		{
			m_moveDirection = new Vector3(movement.x, movement.y, 0.0f);
		}

		private void OnSwing(bool isSwinging)
		{
			m_isSwinging = isSwinging;
		}

		private void FixedUpdate()
		{
			m_rigidbody.AddForce(m_moveDirection * m_movementForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
		}
	}
}