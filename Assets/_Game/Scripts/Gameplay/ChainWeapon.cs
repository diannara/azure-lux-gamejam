using UnityEngine;

namespace Diannara.Gameplay
{
	[RequireComponent(typeof(LineRenderer))]
	public class ChainWeapon : MonoBehaviour
	{
		[Header("Chain Positions")]
		[SerializeField] private Transform m_source;
		[SerializeField] private Transform m_destination;

		[Header("Graphics")]
		[SerializeField] private Sprite m_chainSprite;

		private LineRenderer m_lineRender;

		private Vector3[] points = new Vector3[2];

		private void Awake()
		{
			m_lineRender = GetComponent<LineRenderer>();

			if (m_chainSprite != null)
			{
				m_lineRender.material.SetTexture("_MainTex", m_chainSprite.texture);
				m_lineRender.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
			}
		}

		private void Update()
		{
			UpdateLineRenderer();
		}

		private void UpdateLineRenderer()
		{
			if(m_source == null || m_destination == null)
			{
				return;
			}

			points[0] = m_source.position;
			points[1] = m_destination.position;
			m_lineRender.SetPositions(points);
		}
	}
}