using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FPSCounter : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteFont spriteFont;
        Vector2 FPSCounterLocation;
        SpriteBatch m_spritebatch;

        float FPS;
        public FPSCounter(Game game, ref SpriteBatch spriteBatch)
            : base(game)
        {
			FPSCounterLocation = new Vector2(game.GraphicsDevice.Viewport.Width / 2, 50);
            m_spritebatch = spriteBatch;
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            DrawOrder = 100;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteFont = Game.Content.Load<SpriteFont>(@"Fonts\SpriteFont1");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            // The time since Update was called last
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            FPS = 1 / elapsed;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            m_spritebatch.Begin();
            // TODO: Add your drawing code here
            //Shows the amount of updates per second (updates per second)
            m_spritebatch.DrawString(spriteFont, "UPS: " + FPS.ToString(), FPSCounterLocation, Color.White);
            

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            FPS = 1 / elapsed;
            //Shows the number of draw calls per frame (Frames per second)
            m_spritebatch.DrawString(spriteFont, "FPS: " + FPS.ToString(), FPSCounterLocation + new Vector2(0,20), Color.White);


            m_spritebatch.End();
            base.Draw(gameTime);
        }
    }
