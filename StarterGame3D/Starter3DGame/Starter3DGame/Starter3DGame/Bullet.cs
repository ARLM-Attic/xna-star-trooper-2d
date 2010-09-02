using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Starter3DGame
{
    struct Bullet
    {
        public Vector3 Position;
        public Vector3 Direction;
        public float Speed;
        public bool isActive;

		public void Update(GameTime gameTime)
        {
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Direction * Speed *
                        GameConstants.BulletSpeedAdjustment * delta;
            if (Position.X > GameConstants.PlayfieldSizeX ||
                Position.X < -GameConstants.PlayfieldSizeX ||
                Position.Y > GameConstants.PlayfieldSizeY ||
                Position.Y < -GameConstants.PlayfieldSizeY)
                isActive = false;
        }
    }

}
