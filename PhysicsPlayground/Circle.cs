using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace PhysicsPlayground
{
	public class Circle : Shape
	{
		private CircleShape circle;

		public Circle(float radius) : base((float)Math.PI * radius * radius)
		{
			circle = new CircleShape(radius);
			circle.FillColor = Color.Green;
			Origin = new Vector2f(1, 1).Unit() * radius;
		}

		public override Projection GetProjection(Vector2f axis)
		{
			Vector2f[] edges = new Vector2f[2];

			Vector2f pos = Position;

			edges[0] = pos + axis * -circle.Radius;
			edges[1] = pos + axis * circle.Radius;

			return new Projection(axis, edges);
		}

		public override List<Vector2f> GetProjectionAxes()
		{
			List<Vector2f> ret = new List<Vector2f>(4);

			ret.Add(new Vector2f(circle.Radius, 0).Unit());
			ret.Add(new Vector2f(0, circle.Radius).Unit());
			ret.Add(new Vector2f(circle.Radius, circle.Radius).Unit());
			ret.Add(new Vector2f(circle.Radius, -circle.Radius).Unit());

			return ret;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform = Transform;
			target.Draw(circle, states);
		}
	}
}
