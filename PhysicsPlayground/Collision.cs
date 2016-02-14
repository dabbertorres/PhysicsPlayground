using System.Collections.Generic;
using System.Linq;
using SFML.System;

namespace PhysicsPlayground
{
	public class Collision
	{
		public readonly bool result;
		public readonly Vector2f position;

		public Collision(Shape left, Shape right, ref Vector2f projection)
		{
			List<Vector2f> axes = left.GetProjectionAxes();

			// get rid of duplicate axes
			axes = axes.Union(right.GetProjectionAxes()).ToList();

			Projection.Overlap smallest = new Projection.Overlap();

			foreach(Vector2f axis in axes)
			{
				var overlap = left.GetProjection(axis) - right.GetProjection(axis);

				// there must be overlap on all axes for a collision to occur
				if(!overlap.True)
				{
					result = false;
					return;
				}

				if(overlap < smallest)
					smallest = overlap;
			}

			// if we reach this point, the shapes collided
			result = true;
			projection = smallest.axis * smallest.amount / 2f;
			position = left.Position + projection;
		}
	}
}
