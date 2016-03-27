using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;

namespace PhysicsPlayground
{
	internal class Game
	{
		public static readonly Vector2u WINDOW_SIZE = new Vector2u(800, 600);
		
		public const float COLLISION_RATIO = 0.75f;
		public const float FRICTION_RATIO = 0.99f;
		public const float SHAPE_SIZE = 20f;
		public static readonly Vector2f GRAVITY = new Vector2f(0, 100);
		public static readonly Time dt = Time.FromSeconds(1f / 120f);

		private static List<Shape> shapes;
		private static Shape selectedShape = null;

		private static RenderWindow window;
		private static Clock clock = new Clock();
		private static Time lag = Time.FromSeconds(0);
		private static bool paused = false;

		internal static void Main(string[] args)
		{
			shapes = new List<Shape>();

			window = new RenderWindow(new VideoMode(WINDOW_SIZE.X, WINDOW_SIZE.Y), "Physics Playground", Styles.Titlebar | Styles.Close);
			window.SetVerticalSyncEnabled(true);
			window.SetKeyRepeatEnabled(false);
			SetupEvents();

			Time lastTime = clock.ElapsedTime;

			while(window.IsOpen)
			{
				Time now = clock.ElapsedTime;
				Time frame = now - lastTime;
				lastTime = now;
				lag += frame;

				while(lag >= dt)
				{
					window.DispatchEvents();

					if(!paused)
					{
						Update();
					}

					lag -= dt;
				}

				Draw();
			}
		}

		private static void Draw()
		{
			window.Clear(Color.Black);

			foreach(Shape s in shapes)
			{
				window.Draw(s);
			}

			window.Display();
		}

		private static void Update()
		{
			foreach(Shape shape in shapes)
			{
				// run collision handling
				foreach(Shape other in shapes)
				{
					if(shape != other)
					{
						var collision = new Collision(shape, other);
						if(collision.result)
						{
							// get vectors from each shape's center to the collision point
							var sPos = collision.position - shape.Position;
							var oPos = collision.position - other.Position;

							// move shapes out of each other
							shape.Position -= collision.projection;
							other.Position += collision.projection;

							var finalForce = collision.projection * (other.mass + shape.mass) * COLLISION_RATIO;

							// apply a force along collision axis to each shape at the center of the collision
							shape.ApplyTorque(-finalForce, sPos);
							other.ApplyTorque(finalForce, oPos);
						}
					}
				}

				shape.Acceleration = GRAVITY;
				BoundToWindow(shape);
				shape.Update(dt.AsSeconds());
			}

			if(selectedShape != null)
			{
				// selected shape should ignore gravity
				selectedShape.Acceleration = new Vector2f(0, 0);
			}
		}

		private static void BoundToWindow(Shape shape)
		{
			Vector2f pos = shape.Position;
			Vector2f newPos = pos;

			Vector2f torquePoint = new Vector2f();

			// horizontal bounds
			if(shape.HasVertexOver(0, true, true, ref torquePoint) || shape.HasVertexOver(window.Size.X, true, false, ref torquePoint))
			{
				// apply a force to invert the shape's velocity
				var pointsVelocity = shape.Velocity.X + shape.AngularVelocity * torquePoint.Magnitude();
				Vector2f force = new Vector2f(shape.mass * -2 * pointsVelocity, 0);
				shape.ApplyTorque(force, torquePoint);
			}

			// vertical bounds
			if(shape.HasVertexOver(0, false, true, ref torquePoint) || shape.HasVertexOver(window.Size.Y, false, false, ref torquePoint))
			{
				// apply a force to invert the shape's velocity
				var pointsVelocity = shape.Velocity.Y + shape.AngularVelocity * torquePoint.Magnitude();
				Vector2f force = new Vector2f(0, shape.mass * -2 * pointsVelocity);
				shape.ApplyTorque(force, torquePoint);
			}

			// bound position to window
			Vector2f oppositeVelocity = new Vector2f();

			if(pos.X < 0)
			{
				newPos.X = 0;
				oppositeVelocity.X = -2 * shape.Velocity.X;
			}
			else if(pos.X > window.Size.X)
			{
				newPos.X = window.Size.X;
				oppositeVelocity.X = -2 * shape.Velocity.X;
			}

			if(pos.Y < 0)
			{
				newPos.Y = 0;
				oppositeVelocity.Y = -2 * shape.Velocity.Y;
			}
			else if(pos.Y > window.Size.Y)
			{
				newPos.Y = window.Size.Y;
				oppositeVelocity.Y = -2 * shape.Velocity.Y;
			}

			// apply
			shape.Position = newPos;
			shape.ApplyForce(shape.mass * oppositeVelocity / dt.AsSeconds() * COLLISION_RATIO);
		}

		private static void SetupEvents()
		{
			window.Closed += (s, ev) => window.Close();

			window.LostFocus += (s, ev) => paused = true;
			window.GainedFocus += (s, ev) => paused = false;

			window.KeyReleased += (s, ev) =>
			{
				switch(ev.Code)
				{
					// add shapes
					case Keyboard.Key.Q:
						Shape rect = new Rectangle(SHAPE_SIZE * 4, SHAPE_SIZE);
						rect.Position = (Vector2f)Mouse.GetPosition(window);
						shapes.Add(rect);
						break;

					case Keyboard.Key.W:
						Shape cir = new Circle(SHAPE_SIZE / 2f);
						cir.Position = (Vector2f)Mouse.GetPosition(window);
						shapes.Add(cir);
						break;

					case Keyboard.Key.E:
						Shape tri = new Triangle(SHAPE_SIZE * 2, SHAPE_SIZE);
						tri.Position = (Vector2f)Mouse.GetPosition(window);
						shapes.Add(tri);
						break;
					
					// exit
					case Keyboard.Key.Escape:
						window.Close();
						break;

					default:
						break;
				}
			};

			window.MouseButtonPressed += (s, ev) =>
			{
				switch(ev.Button)
				{
					case Mouse.Button.Left:
						// find which shape the mouse was clicked on (if any)
						Point pt = new Point(new Vector2f(ev.X, ev.Y));

						selectedShape = null;

						foreach(Shape shp in shapes)
						{
							if(new Collision(shp, pt).result)
							{
								selectedShape = shp;
								break;
							}
						}

						break;

					default:
						break;
				}
			};

			window.MouseButtonReleased += (s, ev) =>
			{
				switch(ev.Button)
				{
					case Mouse.Button.Left:
						selectedShape = null;
						break;

					default:
						break;
				}
			};

			window.MouseMoved += (s, ev) =>
			{
				if(selectedShape != null)
				{
					selectedShape.ApplyForce((new Vector2f(ev.X, ev.Y) - selectedShape.Position) * 10000);
				}
			};
		}
	}
}
