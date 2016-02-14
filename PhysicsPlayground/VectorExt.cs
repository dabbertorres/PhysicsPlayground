using System;
using SFML.System;

namespace PhysicsPlayground
{
	public static class VectorExt
	{
		public static float Dot(this Vector2f left, Vector2f right)
		{
			return left.X * right.X + left.Y * right.Y;
		}

		public static float Length(this Vector2f vec)
		{
			return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
		}

		public static Vector2f Unit(this Vector2f vec)
		{
			float len = vec.Length();
			return new Vector2f(vec.X / len, vec.Y / len);
		}

		public static int Dot(this Vector2i left, Vector2i right)
		{
			return left.X * right.X + left.Y * right.Y;
		}

		public static float Length(this Vector2i vec)
		{
			return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
		}

		public static Vector2i Unit(this Vector2i vec)
		{
			int len = (int)vec.Length();
			return new Vector2i(vec.X / len, vec.Y / len);
		}

		public static uint Dot(this Vector2u left, Vector2u right)
		{
			return left.X * right.X + left.Y * right.Y;
		}

		public static float Length(this Vector2u vec)
		{
			return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
		}

		public static Vector2u Unit(this Vector2u vec)
		{
			uint len = (uint)vec.Length();
			return new Vector2u(vec.X / len, vec.Y / len);
		}
	}
}
