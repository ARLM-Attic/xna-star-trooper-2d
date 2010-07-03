﻿#region Using directives

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace TestProject
{
    public class Background: Sprite
    {
        public Background()
        {

        }

        public Background(Texture2D Texture)
            : base(Texture)
        { }

        protected Background(Background background): base(background)
        {

        }

        public override Object Clone()
        {
            return new Background(this);
        }

        public override void Update()
        {
            Vector2 NewLocation = Location;
            if (NewLocation.Y == StarTrooperGame.BackBufferHeight)
            {
                NewLocation.Y = -StarTrooperGame.BackBufferHeight;
                Location = NewLocation;
            }
        }
    }
}
