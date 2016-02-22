using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace PhysicsPlayground
{
	public class Triangle : Shape
	{
		private ConvexShape triangle;
		private float width;
		private float height;

		public Triangle(float width, float height) : base(0.5f * width * height)
		{
			float halfWidth = width / 2f;
			float halfHeight = height / 2f;

			triangle = new ConvexShape(3);
			triangle.SetPoint(0, new Vector2f(halfWidth, 0));
			triangle.SetPoint(1, new Vector2f(width, height));
			triangle.SetPoint(2, new Vector2f(0, height));
			triangle.FillColor = Color.Yellow;

			Origin = new Vector2f(halfWidth, halfHeight);

			this.width = width;
			this.height = height;
		}

		public override Projection GetProjection(Vector2f axis)
		{
			List<Vector2f> vertices = new List<Vector2f>(3);

			vertices.Add(Transform.TransformPoint(width / 2f, 0));  // top
			vertices.Add(Transform.TransformPoint(width, height));  // right
			vertices.Add(Transform.TransformPoint(0, height));      // left

			return new Projection(axis, vertices);
		}

		public override List<Vector2f> GetProjectionAxes()
		{
			List<Vector2f> ret = new List<Vector2f>(3);

			// we don't want the position and origin affecting the normalized axis
			var posAndOrig = Position + Origin;

			var pt0 = Transform.TransformPoint(triangle.GetPoint(0)) - posAndOrig;
			var pt1 = Transform.TransformPoint(triangle.GetPoint(1)) - posAndOrig;
			var pt2 = Transform.TransformPoint(triangle.GetPoint(2)) - posAndOrig;

			ret.Add((pt1 - pt0).Normalized());
			ret.Add((pt2 - pt1).Normalized());
			ret.Add((pt0 - pt2).Normalized());

			return ret;
		}

		public override float GetRadiusOn(Vector2f axis)
		{
			return new Vector2f(axis.X * width, axis.Y * height).Magnitude() / 2f;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform = Transform;
			target.Draw(triangle, states);
		}
	}
}
