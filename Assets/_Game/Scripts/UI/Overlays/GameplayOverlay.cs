using UnityEngine;

using Diannara.Gameplay.Collectables;
using Diannara.Gameplay.Enemy;

namespace Diannara.UI.Overlays
{
    public class GameplayOverlay : Overlay
    {
        [Header("Settings")]
        [SerializeField] private float m_maxScanRange;

        [Header("Closest Enemy")]
        [SerializeField] private RectTransform m_closestEnemyPositionIndicator;
        [SerializeField] private float m_closestEnemyPositionOffset;
        [SerializeField] private float m_closestEnemyIndicatorHideDistanceModifier;

        [Header("Closest Collectable")]
        [SerializeField] private RectTransform m_closestCollectablePositionIndicator;
        [SerializeField] private float m_closestCollectablePositionOffset;
        [SerializeField] private float m_closestCollectableIndicatorHideDistanceModifier;

        [Header("References")]
        [SerializeField] private Camera m_camera;
        [SerializeField] private GameObject m_player;

        private EnemyController FindClosestEnemy()
		{
            EnemyController closestEnemy = null;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_player.transform.position, m_maxScanRange);
            foreach(Collider2D collider in colliders)
			{
                EnemyController enemy = collider.GetComponent<EnemyController>();
                if(enemy != null)
				{
                    if(closestEnemy == null)
					{
                        closestEnemy = enemy;
					}
                    else
					{
                        float newTargetDistance = Vector3.Distance(m_player.transform.position, enemy.transform.position);
                        float currentTargetDistance = Vector3.Distance(m_player.transform.position, closestEnemy.transform.position);

                        if (newTargetDistance < currentTargetDistance)
                        {
                            closestEnemy = enemy;
                        }
                    }
                }
            }
            return closestEnemy;
		}

        private BaseCollectible FindClosestCollectable()
        {
            BaseCollectible closestCollectable = null;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_player.transform.position, m_maxScanRange);
            foreach (Collider2D collider in colliders)
            {
                BaseCollectible collectable = collider.GetComponent<BaseCollectible>();
                if (collectable != null)
                {
                    if (closestCollectable == null)
                    {
                        closestCollectable = collectable;
                    }
                    else
                    {
                        float newTargetDistance = Vector3.Distance(m_player.transform.position, collectable.transform.position);
                        float currentTargetDistance = Vector3.Distance(m_player.transform.position, closestCollectable.transform.position);

                        if (newTargetDistance < currentTargetDistance)
                        {
                            closestCollectable = collectable;
                        }
                    }
                }
            }
            return closestCollectable;
        }

		private void FixedUpdate()
		{
            HandleClosestEnemyIndicator();
            HandleClosestCollectableIndicator();
        }

        private void HandleClosestEnemyIndicator()
		{
            EnemyController closestEnemy = FindClosestEnemy();
            if (closestEnemy == null)
            {
                m_closestEnemyPositionIndicator.gameObject.SetActive(false);
            }
            else
            {
                Vector3 directionToClosestEnemy = (closestEnemy.transform.position - m_player.transform.position).normalized;
                m_closestEnemyPositionIndicator.anchoredPosition = directionToClosestEnemy * m_closestEnemyPositionOffset;
                m_closestEnemyPositionIndicator.eulerAngles = new Vector3(0.0f, 0.0f, GameUtilities.GetAngleFromVector(directionToClosestEnemy));

                float distanceToClosestEnemy = Vector3.Distance(closestEnemy.transform.position, m_player.transform.position);
                m_closestEnemyPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > m_camera.orthographicSize * m_closestEnemyIndicatorHideDistanceModifier);
            }
        }

        private void HandleClosestCollectableIndicator()
		{
            BaseCollectible closestCollectable = FindClosestCollectable();
            if (closestCollectable == null)
            {
                m_closestCollectablePositionIndicator.gameObject.SetActive(false);
            }
            else
            {
                Vector3 directionToClosestCollectable = (closestCollectable.transform.position - m_player.transform.position).normalized;
                m_closestCollectablePositionIndicator.anchoredPosition = directionToClosestCollectable * m_closestCollectablePositionOffset;
                m_closestCollectablePositionIndicator.eulerAngles = new Vector3(0.0f, 0.0f, GameUtilities.GetAngleFromVector(directionToClosestCollectable));

                float distanceToClosestCollectable = Vector3.Distance(closestCollectable.transform.position, m_player.transform.position);
                m_closestCollectablePositionIndicator.gameObject.SetActive(distanceToClosestCollectable > m_camera.orthographicSize * m_closestCollectableIndicatorHideDistanceModifier);
            }
        }
    }
}