using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace PhysicsPlayground
{
	public abstract class Shape : Transformable, Drawable
	{
		public readonly float mass;

		public Vector2f Velocity
		{
			get;
			set;
		}

		public Vector2f AngularVelocity
		{
			get;
			set;
		}

		public Vector2f Acceleration
		{
			get;
			set;
		}

		public Shape(float mass)
		{
			this.mass = mass;
		}

		public void ApplyForce(Vector2f force)
		{
			Acceleration += force / mass;
		}

		public abstract Projection GetProjection(Vector2f axis);
		public abstract List<Vector2f> GetProjectionAxes();

		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
