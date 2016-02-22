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
			List<Vector2f> vertices = new List<Vector2f>(2);

			Vector2f pos = Position;

			// return the ends of a diameter parallel to the given axis
			vertices.Add(pos + axis * -circle.Radius);
			vertices.Add(pos + axis * circle.Radius);

			return new Projection(axis, vertices);
		}

		public override List<Vector2f> GetProjectionAxes()
		{
			List<Vector2f> ret = new List<Vector2f>(4);

			// axes to test are at angles (in order): 0, -90, -45, 45
			ret.Add(new Vector2f(circle.Radius, 0).Normalized());
			ret.Add(new Vector2f(0, circle.Radius).Normalized());
			ret.Add(new Vector2f(circle.Radius, circle.Radius).Normalized());
			ret.Add(new Vector2f(circle.Radius, -circle.Radius).Normalized());

			return ret;
		}

		public override float GetRadiusOn(Vector2f axis)
		{
			return circle.Radius;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform = Transform;
			target.Draw(circle, states);
		}
	}
}
