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

			// axes to test are at angles (degrees):
			ret.Add(new Vector2f(circle.Radius, 0).Unit());					// 0
			ret.Add(new Vector2f(0, circle.Radius).Unit());					// 90
			ret.Add(new Vector2f(circle.Radius, circle.Radius).Unit());		// 45
			ret.Add(new Vector2f(circle.Radius, -circle.Radius).Unit());	// -45

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

		protected override List<Vector2f> GetGlobalPoints()
		{
			List<Vector2f> vertices = new List<Vector2f>(4);

			// return points for every 45 deg
			var horz = new Vector2f(1, 0) * circle.Radius;
			var diag = new Vector2f(1, 1).Unit() * circle.Radius;
			var vert = new Vector2f(0, 1) * circle.Radius;

			vertices.Add(Transform.TransformPoint(horz));				// 0
			vertices.Add(Transform.TransformPoint(diag));				// 45
			vertices.Add(Transform.TransformPoint(vert));				// 90
			vertices.Add(Transform.TransformPoint(-diag.X, diag.Y));	// 135
			vertices.Add(Transform.TransformPoint(-horz.X, horz.Y));    // 180
			vertices.Add(Transform.TransformPoint(-diag.X, -diag.Y));   // 225
			vertices.Add(Transform.TransformPoint(vert.X, -vert.Y));    // 270
			vertices.Add(Transform.TransformPoint(diag.X, -diag.Y));	// 315

			return vertices;
		}
	}
}
