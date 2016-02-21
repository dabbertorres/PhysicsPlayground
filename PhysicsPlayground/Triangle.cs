using System;
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
			Vector2f[] corners = new Vector2f[3];
			
			corners[0] = Transform.TransformPoint(new Vector2f(width / 2f, 0));		// top
			corners[1] = Transform.TransformPoint(new Vector2f(width, height));		// right
			corners[2] = Transform.TransformPoint(new Vector2f(0, height));			// left

			return new Projection(axis, corners);
		}

		public override List<Vector2f> GetProjectionAxes()
		{
			List<Vector2f> ret = new List<Vector2f>(3);

			var posAndOrig = Position - Origin;

			// we want the rotation included, but do not want the position and origin included
			ret.Add(Transform.TransformPoint((triangle.GetPoint(1) - triangle.GetPoint(0)).Unit()) - posAndOrig);
			ret.Add(Transform.TransformPoint((triangle.GetPoint(2) - triangle.GetPoint(1)).Unit()) - posAndOrig);
			ret.Add(Transform.TransformPoint((triangle.GetPoint(0) - triangle.GetPoint(2)).Unit()) - posAndOrig);

			return ret;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform = Transform;
			target.Draw(triangle, states);
		}
	}
}
