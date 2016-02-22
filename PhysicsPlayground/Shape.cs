﻿using System;
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

		public Vector2f Acceleration
		{
			get;
			set;
		}

		public float AngularVelocity
		{
			get;
			set;
		}

		public float AngularAcceleration
		{
			get;
			set;
		}

		public Shape(float mass)
		{
			this.mass = mass;
		}

		// apply a force at the center of the shape
		public void ApplyForce(Vector2f force)
		{
			Acceleration += force / mass;
		}

		// apply a force at 'pos' relative to center of the shape
		public void ApplyForce(Vector2f force, Vector2f pos)
		{
			ApplyForce(force);

			var radius = pos.Magnitude();
			var mag = force.Magnitude();

			// if we're potentially applying a force (or zero N) at the center of the shape
			if(radius == 0 || mag == 0)
				return;

			// torque equation: T = F * r * sin(theta)

			// dot product uses cosine, we need sine, so convert with 1 - cosine == sine
			var sinComp = 1 - force.Dot(pos) / (mag * radius);

			var torque = force.Magnitude() * radius * sinComp;

			var rads = torque / (mass * radius * radius);

			var dir = Math.Sign(force.Cross(pos));

			// convert to degrees
			AngularAcceleration += dir * rads * 180f / (float)Math.PI;
		}

		// returns a Projection of the shape's vertices onto the given axis
		public abstract Projection GetProjection(Vector2f axis);

		// returns the unit normals of each axis this shape needs tested
		public abstract List<Vector2f> GetProjectionAxes();

		public abstract float GetRadiusOn(Vector2f axis);

		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
