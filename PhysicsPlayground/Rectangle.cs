using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace PhysicsPlayground
{
	public class Rectangle : Shape
	{
		private RectangleShape rect;

		public Rectangle(float width, float height) : base(width * height)
		{
			rect = new RectangleShape(new Vector2f(width, height));
			rect.FillColor = Color.Red;

			Origin = rect.Size / 2f;
		}

		public override Projection GetProjection(Vector2f axis)
		{
			return new Projection(axis, GetGlobalPoints());
		}

		public override List<Vector2f> GetProjectionAxes()
		{
			List<Vector2f> ret = new List<Vector2f>(2);

			// we don't want the position and origin affecting the normalized axis
			var posAndOrig = Position + Origin;

			var axis0 = Transform.TransformPoint(rect.Size.X, 0) - posAndOrig;
			var axis1 = Transform.TransformPoint(0, rect.Size.Y) - posAndOrig;

			ret.Add(axis0.Normalized());
			ret.Add(axis1.Normalized());

			return ret;
		}

		public override float GetRadiusOn(Vector2f axis)
		{
			return new Vector2f(axis.X * rect.Size.X, axis.Y * rect.Size.Y).Magnitude();
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform = Transform;
			target.Draw(rect, states);
		}

		protected override List<Vector2f> GetGlobalPoints()
		{
			List<Vector2f> vertices = new List<Vector2f>(4);

			vertices.Add(Transform.TransformPoint(rect.GetPoint(0)));	// top-left
			vertices.Add(Transform.TransformPoint(rect.GetPoint(1)));	// top-right
			vertices.Add(Transform.TransformPoint(rect.GetPoint(2)));	// bot-right
			vertices.Add(Transform.TransformPoint(rect.GetPoint(3)));	// bot-left

			return vertices;
		}
	}
}
