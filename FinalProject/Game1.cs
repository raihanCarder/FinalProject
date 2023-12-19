using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FinalProject
{
    public class Game1 : Game
    {
        // Final Project
        // Raihan Carder
        List<Rectangle> barriers;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        KeyboardState keyboardState;
        Player stickman;
        Texture2D testingTexture;
        Texture2D wallTexture;
        Screen screen;

        enum Screen
        {
            Intro,
            LevelOne
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1100;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();
            this.Window.Title = "Platformer Game";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            

            screen = Screen.LevelOne; // CHANGE TO INTRO
            base.Initialize();

            barriers = new List<Rectangle>();
            if (screen == Screen.LevelOne)
            {
                stickman = new Player(testingTexture, 10, _graphics.PreferredBackBufferHeight - 35);
                barriers.Add(new Rectangle(0, 495, 1100, 50));
                barriers.Add(new Rectangle(100, 460, 20, 20));
                barriers.Add(new Rectangle(600, 430, 100, 30));
            }     

            //for (int i = 0; i < 1; i++)
            //{
            //    barriers.Add(new Rectangle(0,495,1100,50));
            //}
            //barriers.Add(new Rectangle(100, 460, 20, 20));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            testingTexture = Content.Load<Texture2D>("rectangle");
            wallTexture = Content.Load<Texture2D>("rectangle");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            if (screen == Screen.LevelOne)
            {
                stickman.Update(gameTime, barriers); 
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            stickman.Draw(_spriteBatch);
            foreach (Rectangle barrier in barriers)
                _spriteBatch.Draw(wallTexture, barrier, Color.Black);
            _spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}