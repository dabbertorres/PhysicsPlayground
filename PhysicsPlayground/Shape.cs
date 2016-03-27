using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace PhysicsPlayground
{
	public abstract class Shape : Transformable, Drawable
	{
		private const float DEG_2_RAD = (float)Math.PI / 180f;
		private const float RAD_2_DEG = 180f / (float)Math.PI;

		public readonly float mass;

		public Vector2f Velocity
		{
			get;
			set;
		}

		public Vector2f Acceleration
		{
			get;
			set;
		}

		// radians
		public new float Rotation
		{
			get { return base.Rotation * DEG_2_RAD; }
			set { base.Rotation = value * RAD_2_DEG; }
		}

		// radians per sec
		public float AngularVelocity
		{
			get;
			set;
		}
		
		// radians per sec per sec
		public float AngularAcceleration
		{
			get;
			set;
		}

		public Shape(float mass)
		{
			this.mass = mass;
		}

		// dts = delta time in seconds
		public void Update(float dts)
		{
			Velocity += Acceleration * dts;
			Position += Velocity * dts;

			AngularVelocity += AngularAcceleration * dts;
			Rotation += AngularVelocity * dts;

			// zero out all acceleration
			Acceleration = new Vector2f(0, 0);
			AngularAcceleration = 0;
		}

		// apply a force at the center of the shape
		public void ApplyForce(Vector2f force)
		{
			Acceleration += force / mass;
		}

		// apply a force at 'fromCenter' relative to center of the shape
		public void ApplyTorque(Vector2f force, Vector2f fromCenter)
		{
			var radius = fromCenter.Magnitude();

			// check if 'fromCenter' is outside of the shape.
			if(radius > GetRadiusOn(fromCenter))
				return;
			
			// torque is the amount of force in the perpendicular direction
			var torque = force.Cross(fromCenter);

			// linear force is the amount of force in the parallel direction
			var linearForce = force.Dot(fromCenter) / (radius != 0 ? radius : 1f);

			// moment of inertia of the mass at this radius
			var inertia = mass * radius * radius;

			AngularAcceleration += -torque / (inertia != 0 ? inertia : 1);
			ApplyForce(linearForce * force.Unit());
		}

		// returns true if the shape has a vertex on 'leftOrBelow' of a line at 'value'.
		// 'vertical': is the line horizontal or vertical?
		// If above conditions are true, 'fromCenter' is assigned a value from the center
		// of the shape to the most extreme vertex meeting the condition
		public bool HasVertexOver(float value, bool vertical, bool leftOrAbove, ref Vector2f fromCenter)
		{
			Func<Vector2f, float> dist;
			Func<float, bool> test;
			
			if(vertical)
				dist = pt => pt.X - value;
			else
				dist = pt => pt.Y - value;

			if(leftOrAbove)
				test = d => d < 0;
			else
				test = d => d > 0;
				
			float distance = 0;

			var points = GetGlobalPoints();
			foreach(var pt in points)
			{
				float d = dist(pt);
				if(test(d))
				{
					float da = Math.Abs(d);
					if(da > distance)
					{
						fromCenter = pt - Position;
						distance = da;
					}
					else if(da == distance)
					{
						// if we have an equal distance, then we don't want to set 'fromCenter' to only one of the points
						// we want it to be the center between the two
						var ptRel = pt - Position;
						fromCenter.X = (fromCenter.X + ptRel.X) / 2;
						fromCenter.Y = (fromCenter.Y + ptRel.Y) / 2;
					}
				}
			}

			return distance != 0;
		}

		// returns a Projection of the shape's vertices onto the given axis
		public abstract Projection GetProjection(Vector2f axis);

		// returns the unit normals of each axis this shape needs tested
		public abstract List<Vector2f> GetProjectionAxes();

		public abstract float GetRadiusOn(Vector2f axis);

		public abstract void Draw(RenderTarget target, RenderStates states);

		protected abstract List<Vector2f> GetGlobalPoints();
	}
}
