﻿using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace PhysicsPlayground
{
	internal class Game
	{
		public const float GRAVITY = 100f;
		public const float COLLISION_RATIO = 0.75f;
		public const float SHAPE_SIZE = 20f;
		public static readonly Time dt = Time.FromSeconds(1f / 120f);

		private static List<Shape> shapes;
		private static Shape selectedShape = null;

		private static RenderWindow window;
		private static Time lag = Time.FromSeconds(0);
		private static bool paused = false;

		internal static void Main(string[] args)
		{
			shapes = new List<Shape>();

			window = new RenderWindow(new VideoMode(800, 600), "Physics Playground", Styles.Titlebar | Styles.Close);
			window.SetVerticalSyncEnabled(true);
			window.SetKeyRepeatEnabled(false);
			SetupEvents();

			Clock clock = new Clock();
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
			foreach(Shape s in shapes)
			{
				// run collision handling
				foreach(Shape o in shapes)
				{
					if(o != s)
					{
						Vector2f projection = new Vector2f();

						if(new Collision(s, o, ref projection).result)
						{
							// move shapes out of each other
							s.Position -= projection;
							o.Position += projection;

							// take a little "heat" energy
							projection *= COLLISION_RATIO;

							// apply a force along collision axis to each shape
							s.ApplyForce(-projection * o.mass);
							o.ApplyForce(projection * s.mass);
						}
					}
				}

				// update velocity and position
				s.Velocity += s.Acceleration * dt.AsSeconds();
				s.Position += s.Velocity * dt.AsSeconds();

				BoundToWindow(s);

				// zero out all acceleration (relative to gravity)
				s.Acceleration = new Vector2f(0, GRAVITY);
			}

			if(selectedShape != null)
			{
				// selected shape should ignore gravity and any movement
				selectedShape.Acceleration = new Vector2f(0, 0);
				selectedShape.Velocity = new Vector2f(0, 0);
			}
		}

		private static void BoundToWindow(Shape s)
		{
			Vector2f pos = s.Position;
			Vector2f newPos = pos;

			// bound position to window
			if(pos.X < 0)
				newPos.X = 0;
			else if(pos.X > window.Size.X)
				newPos.X = window.Size.X;

			if(pos.Y < 0)
				newPos.Y = 0;
			else if(pos.Y > window.Size.Y)
				newPos.Y = window.Size.Y;

			// if position changed, then a window boundary was hit
			// invert velocity direction and take some "heat" energy
			Vector2f vel = s.Velocity;
			if(newPos.X != pos.X)
				vel.X *= -COLLISION_RATIO;

			if(newPos.Y != pos.Y)
				vel.Y *= -COLLISION_RATIO;

			// apply
			s.Position = newPos;
			s.Velocity = vel;
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
						Shape rect = new Rectangle(SHAPE_SIZE, SHAPE_SIZE);
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
							Vector2f projection = new Vector2f();

							if(new Collision(shp, pt, ref projection).result)
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
					selectedShape.Position = new Vector2f(ev.X, ev.Y);
				}
			};
		}
	}
}
