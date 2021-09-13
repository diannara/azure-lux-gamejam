using System.Collections;

using UnityEngine;

using Diannara.ScriptableObjects.Input;

namespace Diannara.Gameplay.Player
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerMovement : MonoBehaviour
	{
		[Header("Physics")]
		[SerializeField] private float m_movementForce;

		[Header("References")]
		[SerializeField] private InputReader m_inputReader;
		[SerializeField] private Rigidbody2D m_maceRigidbody;

		private Rigidbody2D m_rigidbody;
		private Vector3 m_moveDirection;

		private bool m_isSpeedBuffActive;
		private float m_currentMovementForce;
		private Coroutine m_speedBoostCoroutine;

		public void ActivateSpeedBoost(float boost, float duration)
		{
			if(m_isSpeedBuffActive)
			{
				StopCoroutine(m_speedBoostCoroutine);
				m_speedBoostCoroutine = null;
				m_currentMovementForce = m_movementForce;
			}

			m_speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(boost, duration));
		}

		private void Awake()
		{
			m_currentMovementForce = m_movementForce;
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void OnDisable()
		{
			if (m_inputReader != null)
			{
				m_inputReader.OnMoveEvent -= OnMove;
			}
		}

		private void OnEnable()
		{
			if(m_inputReader != null)
			{
				m_inputReader.OnMoveEvent += OnMove;
			}

			m_currentMovementForce = m_movementForce;
		}

		private void OnMove(Vector2 movement)
		{
			m_moveDirection = new Vector3(movement.x, movement.y, 0.0f);
		}

		private void FixedUpdate()
		{
			m_rigidbody.AddForce(m_moveDirection * m_currentMovementForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
		}

		private IEnumerator SpeedBoostCoroutine(float boost, float duration)
		{
			m_isSpeedBuffActive = true;

			m_currentMovementForce += boost;
			yield return new WaitForSeconds(duration);
			m_speedBoostCoroutine = null;
			m_currentMovementForce = m_movementForce;

			m_isSpeedBuffActive = false;
		}

		public void StopMovement()
		{
			if(m_speedBoostCoroutine != null)
			{
				StopCoroutine(m_speedBoostCoroutine);
				m_speedBoostCoroutine = null;

				m_currentMovementForce = m_movementForce;
			}	

			m_rigidbody.velocity = Vector3.zero;
			m_rigidbody.angularVelocity = 0.0f;

			if (m_maceRigidbody != null)
			{
				m_maceRigidbody.velocity = Vector2.zero;
				m_maceRigidbody.angularVelocity = 0.0f;
			}
		}
	}
}