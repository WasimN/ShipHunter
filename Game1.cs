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

namespace ShipHunter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //System Objects
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viewport viewPort;
        Random random;

        //Game Objects
        GameMouse mouse;
        Camera camera;
        Debug debug;
        InputObject ship;
        Background backGround;
        List<Asteroid> asteroid;

        List<Blasters> blasters;


        public Game1()
        {
            random = new Random();
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = (int)1080;
            graphics.PreferredBackBufferWidth = (int)1920;
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);  

            // TODO: Add your initialization logic here
            mouse = new GameMouse(Content, "Art/mice", new Vector2(3,1)); 
            camera = new Camera();
            debug = new Debug(Content, spriteBatch, 0, 0);
            ship = new InputObject(Content, "Art/spaceShip", new Vector2(0, 0), new Vector2(0, 0), 6.0f, 30);
            backGround = new Background(Content, graphics, "Art/space");
            asteroid = new List<Asteroid>();

            blasters = new List<Blasters>();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            viewPort = graphics.GraphicsDevice.Viewport;
            
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (Mouse.GetState().ScrollWheelValue > mouse.getMouse.ScrollWheelValue)
                camera.Zoom += .05f;
            else if (Mouse.GetState().ScrollWheelValue < mouse.getMouse.ScrollWheelValue)
                camera.Zoom -= .05f;

            // TODO: Add your update logic here
            ship.UpdateInput(Keyboard.GetState(), gameTime, viewPort, 0, backGround, mouse, camera);

            camera.Position = new Vector2(ship.Position.X, ship.Position.Y);
            
            mouse.Update(ship, viewPort, gameTime, camera);
            for (int i = asteroid.Count; i < 500; i++)
                asteroid.Add(new Asteroid(Content));
            for (int i = 0; i < asteroid.Count; i++)
                asteroid[i].Update(gameTime, viewPort, 30, backGround);


            for (int i = 0; i < blasters.Count; i++)
            {
                blasters[i].Update(gameTime, viewPort, 20, backGround);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                bool create = true;

                for (int i = 0; i < blasters.Count; i++)
                {
                    if (!blasters[i].Visible)
                    {
                        create = false;
                        blasters[i].Fire(ship.Position, ship.Force, gameTime);

                        break;
                    }

                    //blasters[i].Visible = false;
                }

                if (create)
                {
                    Blasters blaster = new Blasters(Content, ship.Position);
                    blaster.Fire(ship.Position, ship.Force, gameTime);
                    blasters.Add(blaster);
                }
            }

            //if (ship.BoxCollision(enemyShip).CollisionFlag)
            //{
            //    if (ship.CheckPerPixelCollision(enemyShip))
            //        pixelCollision = true;
            //    else
            //        pixelCollision = false;
            //}
            //else
            //    pixelCollision = false;


            // Debugger statements go here 
            debug.PushBack("Num asteroids: " + asteroid.Count.ToString(), ship.Position.X - 140, ship.Position.Y - 70);
            debug.PushBack("Num blasters: " + blasters.Count.ToString(), ship.Position.X - 140, ship.Position.Y - 60);

            base.Update(gameTime);
            viewPort = graphics.GraphicsDevice.Viewport;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
                //all sprite are drawn in the order they are listed
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, 
                camera.GetTranformation(GraphicsDevice, viewPort));

            mouse.Draw(spriteBatch);
            ship.Draw(spriteBatch);

            for (int i = 0; i < blasters.Count; i++)
            {
                blasters[i].Draw(spriteBatch);
            }

            debug.Draw();

            backGround.BGDraw(spriteBatch);

            for (int i = 0; i < asteroid.Count; i++)
                asteroid[i].Draw(spriteBatch);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
