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

			Vector2f halfSize = rect.Size / 2f;

			// Origin is center of the rectangle, not the top-left corner
			corners[0] = Transform.TransformPoint(-halfSize);                                   // top-left
			corners[1] = Transform.TransformPoint(halfSize - new Vector2f(0, rect.Size.Y));     // top-right
			corners[2] = Transform.TransformPoint(halfSize);                                    // bot-right
			corners[3] = Transform.TransformPoint(halfSize - new Vector2f(rect.Size.X, 0));     // bot-left

			return new Projection(axis, corners);
		}

		public override List<Vector2f> GetProjectionAxes()
		{
			List<Vector2f> ret = new List<Vector2f>(2);
			ret.Add(Transform.TransformPoint(rect.Size.X, 0).Unit());
			ret.Add(Transform.TransformPoint(0, rect.Size.Y).Unit());

			return ret;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform = Transform;
			target.Draw(rect, states);
		}
	}
}
