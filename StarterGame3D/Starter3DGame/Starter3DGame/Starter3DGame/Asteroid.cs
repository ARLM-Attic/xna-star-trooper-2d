using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Starter3DGame
{
    class Asteroid
    {
        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);
		//public Matrix TransformMatrix;
        public Vector3 Position;
        public Vector3 Direction;
		public Vector3 Rotation;
		public float Speed;
		public float Scale = 0.005f;

        public bool isActive;

		public Matrix TransformMatrix
		{
			get
			{
				return RotationMatrix * Matrix.CreateTranslation(Position);
			}
		}

		public void Update(GameTime gameTime)
        {
			float time = (float)gameTime.TotalGameTime.TotalSeconds;

			RotationMatrix = Matrix.CreateRotationX(Rotation.X * time) *
							  Matrix.CreateRotationY(Rotation.X * time ) *
							  Matrix.CreateRotationZ(Rotation.Z * time );

			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Direction * Speed *
                        GameConstants.AsteroidSpeedAdjustment * delta;
			//TransformMatrix = Matrix.CreateScale(Scale) * RotationMatrix * Matrix.CreateTranslation(Position);

            if (Position.X > GameConstants.PlayfieldSizeX)
                Position.X -= 2 * GameConstants.PlayfieldSizeX;
            if (Position.X < -GameConstants.PlayfieldSizeX)
                Position.X += 2 * GameConstants.PlayfieldSizeX;
            if (Position.Y > GameConstants.PlayfieldSizeY)
                Position.Y -= 2 * GameConstants.PlayfieldSizeY;
            if (Position.Y < -GameConstants.PlayfieldSizeY)
                Position.Y += 2 * GameConstants.PlayfieldSizeY;
        }

    }
}
