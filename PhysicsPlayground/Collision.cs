using System;
using System.Collections.Generic;
using System.Linq;
using SFML.System;

namespace PhysicsPlayground
{
	public class Collision
	{
		public readonly bool result;
		public readonly Vector2f position;
		public readonly Vector2f projection;

		public Collision(Shape left, Shape right)
		{
			List<Vector2f> axes = left.GetProjectionAxes();

			// get rid of duplicate axes
			axes = axes.Union(right.GetProjectionAxes()).ToList();

			Vector2f smallest = new Vector2f();

			foreach(Vector2f axis in axes)
			{
				var overlap = left.GetProjection(axis) - right.GetProjection(axis);

				// there must be overlap on all axes for a collision to occur
				if(overlap.Magnitude() == 0)
				{
					result = false;
					return;
				}

				if(IsSmaller(overlap, smallest))
					smallest = overlap;
			}

			// if we reach this point, the shapes collided
			result = true;
			projection = smallest / 2f;
			position = left.Position + projection;
		}

		private static bool IsSmaller(Vector2f left, Vector2f right)
		{
			var leftMag = left.Magnitude();
			var rightMag = right.Magnitude();

			if(leftMag == 0 || rightMag == 0)
				return true;

			return Math.Abs(leftMag) < Math.Abs(rightMag);
		}
	}
}
