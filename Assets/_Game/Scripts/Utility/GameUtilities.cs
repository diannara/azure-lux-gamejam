using UnityEngine;

public static class GameUtilities
{
	public static float CalulcateImpulse(Collision2D collision)
	{
		float impulse = 0.0f;

		foreach (ContactPoint2D cp in collision.contacts)
		{
			impulse += cp.normalImpulse;
		}

		return impulse;
	}
}