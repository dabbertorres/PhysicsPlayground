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
			Vector2f[] corners = new Vector2f[4];
			
			corners[0] = Transform.TransformPoint(new Vector2f(0, 0));              // top-left
			corners[1] = Transform.TransformPoint(new Vector2f(rect.Size.X, 0));	// top-right
			corners[2] = Transform.TransformPoint(rect.Size);                       // bot-right
			corners[3] = Transform.TransformPoint(new Vector2f(0, rect.Size.Y));	// bot-left

			return new Projection(axis, corners);
		}

		public override List<Vector2f> GetProjectionAxes()
		{
			List<Vector2f> ret = new List<Vector2f>(2);

			var posAndOrig = Position - Origin;

			// we want the rotation included, but do not want the position and origin included
			ret.Add(Transform.TransformPoint(new Vector2f(rect.Size.X, 0).Unit()) - posAndOrig);
			ret.Add(Transform.TransformPoint(new Vector2f(0, rect.Size.Y).Unit()) - posAndOrig);

			return ret;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform = Transform;
			target.Draw(rect, states);
		}
	}
}
