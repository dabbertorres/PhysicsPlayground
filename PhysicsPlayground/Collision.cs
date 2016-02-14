using System.Collections.Generic;
using System.Linq;
using SFML.System;

namespace PhysicsPlayground
{
	public class Collision
	{
		public readonly bool result;
		public readonly Vector2f position;

		public Collision(Shape left, Shape right)
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
			Vector2f magnitude = smallest.axis * smallest.amount / 2f;
			position = left.Position + magnitude;

			// move shapes out of each other
			left.Position += magnitude;
			right.Position -= magnitude;

			// take a little "heat" energy
			magnitude *= Game.COLLISION_RATIO;

			// apply a force along collision axis to each shape
			left.ApplyForce(magnitude);
			right.ApplyForce(-magnitude);
		}
	}
}
