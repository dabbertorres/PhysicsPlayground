using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace PhysicsPlayground
{
	public class Point : Shape
	{
		private Vector2f point;

		public Point(Vector2f point) : base(1)
		{
			this.point = point;
		}

		public override Projection GetProjection(Vector2f axis)
		{
			// just our one point
			return new Projection(axis, new List<Vector2f> { point });
		}

		public override List<Vector2f> GetProjectionAxes()
		{
			// a point has no axes to worry about
			return new List<Vector2f>(0);
		}

		public override float GetRadiusOn(Vector2f axis)
		{
			// a point has no radius
			return 0f;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			// do nothing
		}
	}
}
