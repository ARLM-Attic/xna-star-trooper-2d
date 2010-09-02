using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Starter3DGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // The aspect ratio determines how to scale 3d to 2d projection.
        float aspectRatio;

        //Camera_View information
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.CameraHeight);
        static Matrix projectionMatrix;
        static Matrix viewMatrix;

        SpriteFont Font;
        Random random = new Random();

        //Player Ship
        Ship ship = new Ship();

        //Asteroid Template
        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
        Matrix[] AsteroidsTransforms;
        Model AsteroidModel;

        //Bullet Template
        Model BulletModel;
        Texture2D BulletTexture;
        Matrix[] bulletTransforms;
        Bullet[] bulletList = new Bullet[GameConstants.NumBullets];

		//Background
		Texture2D stars;
		
		public static int PreferredBackBufferWidth;
		public static int PreferredBackBufferHeight;

		public static Viewport viewport;

		public static SoundEffect Engine;
		public static SoundEffectInstance EngineInstance;

		public static SoundEffect Shot;
		public static SoundEffect ExplosionShip;
		public static SoundEffect ExplosionAsteroid;
		public static SoundEffect HyperSpace;

		public static SoundEffect Music;
		public static SoundEffectInstance MusicInstance;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
			

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            //Added
            graphics.IsFullScreen = true;
            InitializeDownscaleGraphics();
            //InitializeFullScaleGraphics();

			
            
        }


        /// <summary>
        /// Helper method to the initialize the game to be a portrait game.
        /// </summary>
        private void InitializeDownscaleGraphics()
        {
            graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.LandscapeLeft;
            PreferredBackBufferWidth = 240;
            PreferredBackBufferHeight = 400;
            
        }

        /// <summary>
        /// Helper method to initialize the game to be a landscape game.
        /// </summary>
        private void InitializeFullScaleGraphics()
        {
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.Portrait;
            PreferredBackBufferWidth = 800;
            PreferredBackBufferHeight = 480;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
			graphics.PreferredBackBufferWidth = PreferredBackBufferWidth;
			graphics.PreferredBackBufferHeight = PreferredBackBufferHeight;
			viewport = graphics.GraphicsDevice.Viewport;
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: Add your initialization logic here
            ResetAsteroids();
			this.Components.Add(new FPSCounter(this, ref spriteBatch));

            base.Initialize();
        }

        private void ResetAsteroids()
        {
            float xStart;
            float yStart;
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                asteroidList[i] = new Asteroid();
                if (random.Next(2) == 0)
                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)GameConstants.PlayfieldSizeX;
                }
                yStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeY;
                asteroidList[i].Position = new Vector3(xStart, yStart, 0.0f);
                double angle = random.NextDouble() * 2 * Math.PI;
                asteroidList[i].Direction.X = -(float)Math.Sin(angle);
                asteroidList[i].Direction.Y = (float)Math.Cos(angle);
                asteroidList[i].Speed = GameConstants.AsteroidMinSpeed +
                   (float)random.NextDouble() * GameConstants.AsteroidMaxSpeed;
				asteroidList[i].Rotation.X = (float)random.NextDouble();
				asteroidList[i].Rotation.Y = (float)random.NextDouble();
				asteroidList[i].Rotation.Z = (float)random.NextDouble();
                asteroidList[i].isActive = true;
            }
        }

        private Matrix[] SetupEffectDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
					effect.PreferPerPixelLighting = false;
               }
            }
            return absoluteTransforms;
        }

        private Model SetupModelTexture(Model myModel, Texture2D myTexture)
        {

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = true;
                    effect.Texture = myTexture;
                }
            }
            return myModel;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {


            // Load Font
            Font = Content.Load<SpriteFont>(@"Fonts\SpriteFont1");
            
            //Setup Camera
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f), aspectRatio,
                1,
                GameConstants.CameraHeight + 10.0f);

            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);

            //Load Models
            ship.Model = Content.Load<Model>(@"Models\p1_wedge");
            ship.Transforms = SetupEffectDefaults(ship.Model);
			
            AsteroidModel = Content.Load<Model>(@"Models\asteroid");
            AsteroidsTransforms = SetupEffectDefaults(AsteroidModel);

            BulletModel = Content.Load<Model>(@"Models\pea_proj");
            BulletTexture = Content.Load<Texture2D>(@"Textures\pea_proj");
            bulletTransforms = SetupEffectDefaults(BulletModel);
            BulletModel = SetupModelTexture(BulletModel, BulletTexture);

            //Load Background
            stars = Content.Load<Texture2D>(@"Textures\B1_stars");

			//Load Audio
			Engine = Content.Load<SoundEffect>(@"Audio\EngineSound"); 
			EngineInstance = Engine.CreateInstance();
			EngineInstance.IsLooped = true;

			Shot = Content.Load<SoundEffect>(@"Audio\EngineSound"); 
			ExplosionShip = Content.Load<SoundEffect>(@"Audio\EngineSound"); 
			ExplosionAsteroid = Content.Load<SoundEffect>(@"Audio\EngineSound");
			HyperSpace = Content.Load<SoundEffect>(@"Audio\EngineSound"); 

			Music = Content.Load<SoundEffect>(@"Audio\EngineSound"); 
			MusicInstance = Music.CreateInstance();
			MusicInstance.IsLooped = true;




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
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
			Input.Update();
			ship.Update(gameTime);

            // TODO: Add your update logic here
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
				asteroidList[i].Update(gameTime);
            }

            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                if (bulletList[i].isActive)
                {
                    bulletList[i].Update(gameTime);
                }
            }



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(stars, GraphicsDevice.Viewport.TitleSafeArea, Color.White);
            spriteBatch.DrawString(Font, "Hello World", new Vector2(10, 10), Color.Red);
            spriteBatch.End();

            if (ship.isActive)
            {
                DrawModel(ship.Model, ship.TransformMatrix, ship.Transforms, ship.Scale);
            }
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (asteroidList[i].isActive)
                {
					DrawModel(AsteroidModel, asteroidList[i].TransformMatrix, AsteroidsTransforms, asteroidList[i].Scale);
                }
            }
            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                if (bulletList[i].isActive)
                {
                    Matrix bulletTransform =
                      Matrix.CreateTranslation(bulletList[i].Position);
                    DrawModel(BulletModel, bulletTransform, bulletTransforms);
                }
            }



            base.Draw(gameTime);
        }

		public static void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
		{
			DrawModel(model,modelTransform, absoluteBoneTransforms,1f);
		}

        public static void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms, float Scale)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform *  Matrix.CreateScale(Scale);
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }

		public static Vector2 TransformtoScreenSpace(Vector2 position)
		{
			//Create a 3D vector from our 2D touch
			Vector3 point = new Vector3(position, 0f);

			// use Viewport.Unproject to tell what those two screen space positions
			// would be in world space. we'll need the projection matrix and view
			// matrix, which we have saved as member variables. We also need a world
			// matrix, which can just be identity.
			Vector3 target = Game1.viewport.Unproject(point,
				projectionMatrix, viewMatrix, Matrix.Identity);

			position.X = target.X;
			position.Y = target.Y;

			return position;
		}

		// CalculateCursorRay Calculates a world space ray starting at the camera's
		// "eye" and pointing in the direction of the cursor. Viewport.Unproject is used
		// to accomplish this. see the accompanying documentation for more explanation
		// of the math behind this function.
		public Ray CalculateCursorRay(Vector2 Target)
		{
			// create 2 positions in screenspace using the cursor position. 0 is as
			// close as possible to the camera, 1 is as far away as possible.
			Vector3 nearSource = new Vector3(Target, 0f);
			Vector3 farSource = new Vector3(Target, 1f);

			// use Viewport.Unproject to tell what those two screen space positions
			// would be in world space. we'll need the projection matrix and view
			// matrix, which we have saved as member variables. We also need a world
			// matrix, which can just be identity.
			Vector3 nearPoint = Game1.viewport.Unproject(nearSource,
				projectionMatrix, viewMatrix, Matrix.Identity);

			Vector3 farPoint = Game1.viewport.Unproject(farSource,
				projectionMatrix, viewMatrix, Matrix.Identity);

			// find the direction vector that goes from the nearPoint to the farPoint
			// and normalize it....
			Vector3 direction = farPoint - nearPoint;
			direction.Normalize();

			// and then create a new ray using nearPoint as the source.
			return new Ray(nearPoint, direction);
		}
    }
}
