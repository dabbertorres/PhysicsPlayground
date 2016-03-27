using System;
using SFML.System;

namespace PhysicsPlayground
{
	public static class Vector2fExt
	{
		public static float Dot(this Vector2f left, Vector2f right)
		{
			return left.X * right.X + left.Y * right.Y;
		}

		// returns a scalar, the sign of which can be used to determine rotation direction
		public static float Cross(this Vector2f left, Vector2f right)
		{
			return left.X * right.Y - left.Y * right.X;
		}

		public static float Magnitude(this Vector2f vec)
		{
			return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
		}

		public static float MagnitudeSquared(this Vector2f vec)
		{
			return vec.X * vec.X + vec.Y * vec.Y;
		}

		public static Vector2f Unit(this Vector2f vec)
		{
			float len = vec.Magnitude();

			if(len == 0)
				return new Vector2f(0, 0);

			return new Vector2f(vec.X / len, vec.Y / len);
		}

		public static Vector2f Normal(this Vector2f vec)
		{
			return new Vector2f(-vec.Y, vec.X);
		}

		public static Vector2f Normalized(this Vector2f vec)
		{
			return vec.Normal().Unit();
		}
	}

	public static class Vector2iExt
	{
		public static int Dot(this Vector2i left, Vector2i right)
		{
			return left.X * right.X + left.Y * right.Y;
		}

		// returns a scalar, the sign of which can be used to determine rotation direction
		public static int Cross(this Vector2i left, Vector2i right)
		{
			return left.X * right.Y - left.Y * right.X;
		}

		public static float Magnitude(this Vector2i vec)
		{
			return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
		}

		public static int MagnitudeSquared(this Vector2i vec)
		{
			return vec.X * vec.X + vec.Y * vec.Y;
		}

		public static Vector2i Unit(this Vector2i vec)
		{
			int len = (int)vec.Magnitude();
			return new Vector2i(vec.X / len, vec.Y / len);
		}

		public static Vector2i Normal(this Vector2i vec)
		{
			return new Vector2i(-vec.Y, vec.X);
		}

		public static Vector2i Normalized(this Vector2i vec)
		{
			return vec.Normal().Unit();
		}
	}

	// Does not include Normal or Cross functions, due to the potential for returning negatives
	public static class Vector2uExt
	{
		public static uint Dot(this Vector2u left, Vector2u right)
		{
			return left.X * right.X + left.Y * right.Y;
		}

		public static float Magnitude(this Vector2u vec)
		{
			return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
		}

		public static uint MagnitudeSquared(this Vector2u vec)
		{
			return vec.X * vec.X + vec.Y * vec.Y;
		}

		public static Vector2u Unit(this Vector2u vec)
		{
			uint len = (uint)vec.Magnitude();
			return new Vector2u(vec.X / len, vec.Y / len);
		}
	}
}
