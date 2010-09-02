using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Starter3DGame
{
    class Ship
    {
        public Model Model;
        public Matrix[] Transforms;
        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);
		
        //Position of the model in world space
        public Vector3 Position = Vector3.Zero;
		public Vector2 Target = Vector2.Zero;
		//offset due to Model
		float offset = MathHelper.PiOver2;
		public float Scale = 0.005f;



        public Matrix TransformMatrix
        {
            get
            {
                return RotationMatrix * Matrix.CreateTranslation(Position);
            }
        }

        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity = Vector3.Zero;
        private const float VelocityScale = 5.0f; //amplifies controller speed input
        
        public bool isActive = true;

        private float rotation = MathHelper.PiOver4;
        public float Rotation
        {
            get { return rotation; }
            set
            {
				value = WrapAngle(value);
                if (rotation != value)
                {
                    rotation = value;
                    RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2) *
                        Matrix.CreateRotationZ(rotation);
                }
            }
        }

		public void Update(GameTime gameTime)
        {
            HandleInput();

            // Add velocity to the current position.
            Position.X += Velocity.X;
			Position.Y += Velocity.Y;
			Position.Z += Velocity.Z;

            // Bleed off velocity over time.
            Velocity *= 0.95f;

        }

        void HandleInput()
        {
			TouchCollection touches = Input.touches;
			foreach (TouchLocation t in touches)
            {
                switch (t.State)
                {
                    case TouchLocationState.Pressed:
                        break;
                    case TouchLocationState.Moved:
						Target = Game1.TransformtoScreenSpace(t.Position);
						ApplyThrust();
                        break;
                    case TouchLocationState.Released:
						Game1.EngineInstance.Stop();
                        break;
                    default:
                        break;
                }
            }
			if(Target != Vector2.Zero) Rotation = TurnToFace(new Vector2(Position.X, Position.Y), Target, rotation, 0.1f, offset);

			
        }

		private void ApplyThrust()
		{
			// Finally, add this vector to our velocity.
			Velocity += RotationMatrix.Forward * VelocityScale;
			if (Game1.EngineInstance.State != Microsoft.Xna.Framework.Audio.SoundState.Playing) Game1.EngineInstance.Play();
		}

		/// <summary>
		/// Calculates the angle that an object should face, given its position, its
		/// target's position, its current angle, and its maximum turning speed.
		/// </summary>
		private static float TurnToFace(Vector2 position, Vector2 faceThis,
			float currentAngle, float turnSpeed)
		{
			return TurnToFace(position, faceThis, currentAngle, turnSpeed, 0);
		}

		/// <summary>
		/// Calculates the angle that an object should face, given its position, its
		/// target's position, its current angle, and its maximum turning speed.
		/// </summary>
		private static float TurnToFace(Vector2 position, Vector2 faceThis,
			float currentAngle, float turnSpeed, float offSet)
		{
			// consider this diagram:
			//         C 
			//        /|
			//      /  |
			//    /    | y
			//  / o    |
			// S--------
			//     x
			// 
			// where S is the position of the spot light, C is the position of the cat,
			// and "o" is the angle that the spot light should be facing in order to 
			// point at the cat. we need to know what o is. using trig, we know that
			//      tan(theta)       = opposite / adjacent
			//      tan(o)           = y / x
			// if we take the arctan of both sides of this equation...
			//      arctan( tan(o) ) = arctan( y / x )
			//      o                = arctan( y / x )
			// so, we can use x and y to find o, our "desiredAngle."
			// x and y are just the differences in position between the two objects.
			float x = faceThis.X - position.X;
			float y = faceThis.Y - position.Y;

			// we'll use the Atan2 function. Atan will calculates the arc tangent of 
			// y / x for us, and has the added benefit that it will use the signs of x
			// and y to determine what cartesian quadrant to put the result in.
			// http://msdn2.microsoft.com/en-us/library/system.math.atan2.aspx
			float desiredAngle = (float)Math.Atan2(y, x);
			desiredAngle -= offSet;
			// so now we know where we WANT to be facing, and where we ARE facing...
			// if we weren't constrained by turnSpeed, this would be easy: we'd just 
			// return desiredAngle.
			// instead, we have to calculate how much we WANT to turn, and then make
			// sure that's not more than turnSpeed.

			// first, figure out how much we want to turn, using WrapAngle to get our
			// result from -Pi to Pi ( -180 degrees to 180 degrees )
			float difference = WrapAngle(desiredAngle - currentAngle);


			// clamp that between -turnSpeed and turnSpeed.
			difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

			// so, the closest we can get to our target is currentAngle + difference.
			// return that, using WrapAngle again.
			return WrapAngle(currentAngle + difference);
		}

		/// <summary>
		/// Returns the angle expressed in radians between -Pi and Pi.
		/// </summary>
		private static float WrapAngle(float radians)
		{
			while (radians < -MathHelper.Pi)
			{
				radians += MathHelper.TwoPi;
			}
			while (radians > MathHelper.Pi)
			{
				radians -= MathHelper.TwoPi;
			}
			return radians;
		}





    }
}
