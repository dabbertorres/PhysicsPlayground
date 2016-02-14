using System;
using SFML.System;

namespace PhysicsPlayground
{
	public struct Projection
	{
		public struct Overlap
		{
			public readonly Vector2f axis;
			public readonly float amount;

			public bool True
			{
				get { return amount != 0; }
			}

			public Overlap(Vector2f axis, float amount)
			{
				this.axis = axis;
				this.amount = amount;
			}

			public static bool operator <(Overlap left, Overlap right)
			{
				if(left.amount == 0 || right.amount == 0)
					return true;

				return Math.Abs(left.amount) < Math.Abs(right.amount);
			}

			public static bool operator >(Overlap left, Overlap right)
			{
				if(left.amount == 0 || right.amount == 0)
					return true;

				return Math.Abs(left.amount) > Math.Abs(right.amount);
			}
		}

		public readonly Vector2f axis;
		public readonly float max;
		public readonly float min;

		public Projection(Vector2f axis, Vector2f[] vertices)
		{
			this.axis = axis;

			min = axis.Dot(vertices[0]);
			max = min;

			for(int i = 1; i < vertices.Length; ++i)
			{
				float p = axis.Dot(vertices[i]);

				if(p < min)
					min = p;
				else if(p > max)
					max = p;
			}
		}

		public static Overlap operator -(Projection left, Projection right)
		{
			if(left.axis != right.axis)
				throw new InvalidOperationException("Projections must have the same axis to overlap.");

			float overlap = 0;

			if(left.min <= right.max && right.max <= left.max)
				overlap = right.max - left.min;
			else if(right.min <= left.max && left.max <= right.max)
				overlap = left.max - right.min;

			return new Overlap(left.axis, overlap);
		}
	}
}
