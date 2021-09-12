using UnityEngine;

using Diannara.ScriptableObjects.Input;

namespace Diannara.Gameplay.Player
{
	public class PlayerController : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private float m_minimumChainDistance;
		[SerializeField] private float m_maximumChainDistance;

		[Header("References")]
		[SerializeField] private InputReader m_inputReader;
		[SerializeField] private DistanceJoint2D m_distanceJoint;

		private void Start()
		{
			if(m_distanceJoint != null)
			{
				m_distanceJoint.distance = m_minimumChainDistance;
			}
		}

		private void OnDisable()
		{
			if(m_inputReader != null)
			{
				m_inputReader.OnSwingEvent -= OnSwing;
			}
		}

		private void OnEnable()
		{
			if(m_inputReader != null)
			{
				m_inputReader.OnSwingEvent += OnSwing;
			}
		}

		private void OnSwing(bool isSwinging)
		{
			if(m_distanceJoint == null)
			{
				return;
			}

			if(isSwinging)
			{
				//m_distanceJoint.autoConfigureDistance = false;
				m_distanceJoint.distance = m_maximumChainDistance;
			}
			else
			{
				//m_distanceJoint.autoConfigureDistance = true;
				m_distanceJoint.distance = m_minimumChainDistance;
			}
		}
	}
}