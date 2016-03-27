using System;
using System.Collections.Generic;
using SFML.System;

namespace PhysicsPlayground
{
	public struct Projection
	{
		public readonly Vector2f axis;
		public readonly float max;
		public readonly float min;

		public Projection(Vector2f axis, List<Vector2f> vertices)
		{
			this.axis = axis;

			min = axis.Dot(vertices[0]);
			max = min;

			for(int i = 1; i < vertices.Count; ++i)
			{
				float p = axis.Dot(vertices[i]);

				if(p < min)
					min = p;
				else if(p > max)
					max = p;
			}
		}

		public bool IsInvalid()
		{
			return float.IsNaN(axis.X) || float.IsNaN(axis.Y);
		}

		public static Vector2f operator -(Projection left, Projection right)
		{
			// just return 0 overlap if an axis is invalid
			if(left.IsInvalid() || right.IsInvalid())
				return new Vector2f();
			
			if(left.axis != right.axis)
				throw new InvalidOperationException("Projections must have the same axis to overlap.");

			float overlap = 0;

			if(left.min <= right.max && right.max <= left.max)
				overlap = right.max - left.min;
			else if(right.min <= left.max && left.max <= right.max)
				overlap = left.max - right.min;

			return left.axis * overlap;
		}
	}
}
