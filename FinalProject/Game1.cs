using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace FinalProject
{
    public class Game1 : Game
    {
        // Final Project
        // Raihan Carder
        List<Rectangle> barriers;
        List<Texture2D> stickmanTextures;
        List<Texture2D> spinningBladeTextures;
        List<SpinningBlade> spinningBlades;
        List<DoubleJump> doubleJumps;
        List<EndLevelDoor> endingDoors;
        List<Texture2D> doubleJumpTextures;
        List<Texture2D> doorTextures;
        Texture2D stickmanSpritesheet;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Player stickman;
        Texture2D wallTexture;
        Screen screen;
        MouseState mouseState;
        int xPosition, yPosition;
        SpriteFont cordinates;
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

            // Texture 2D lists

            stickmanTextures = new List<Texture2D>();
            spinningBladeTextures = new List<Texture2D>();
            doubleJumpTextures = new List<Texture2D>();
            doorTextures = new List<Texture2D>();

            // Screen

            screen = Screen.LevelOne; // CHANGE TO INTRO
            base.Initialize();
          
            // Player
            stickman = new Player(stickmanTextures, 24, 400); // Testing Sprite

            // Initialize all other Lists

            barriers = new List<Rectangle>();
            spinningBlades = new List<SpinningBlade>();
            doubleJumps = new List<DoubleJump>();
            endingDoors = new List<EndLevelDoor>();

            if (screen == Screen.Intro)
            {


            }
            if (screen == Screen.LevelOne)
            {
                stickman.SpawnPoint = new Vector2(24, 400);

                barriers.Add(new Rectangle(0, 450, 700, 20));            
                barriers.Add(new Rectangle(600, 400, 100, 20));
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(150, 300), 300, 2, 50, true)); // How to add Blades
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(150, 300), 500, 2, 50, false));
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(500, 400), 500, 0, 50, false));
                doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(400, 400), 30));
                doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(600, 340), 50));
                endingDoors.Add(new EndLevelDoor(doorTextures, new Vector2(1000, 420),70));
            }
      
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            wallTexture = Content.Load<Texture2D>("rectangle");
            stickmanSpritesheet = Content.Load<Texture2D>("BlackStickmanRight");
            cordinates = Content.Load<SpriteFont>("Cordinates"); // Delete later 


            int width = stickmanSpritesheet.Width / 8;
            int height = stickmanSpritesheet.Height / 5;

            // Stickman Textures

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    
                    Rectangle sourceRect = new Rectangle(x * width, y * height, width, height);
                    Texture2D cropTexture = new Texture2D(GraphicsDevice, width, height);
                    Color[] data = new Color[width * height];
                    stickmanSpritesheet.GetData(0, sourceRect, data, 0, data.Length);
                    cropTexture.SetData(data);
                    if (stickmanTextures.Count < 40)
                    {
                        stickmanTextures.Add(cropTexture);
                    }        
                }
            }

            // Spinning Blade Texture

            for (int i = 0; i < 3; i++)
            {
                spinningBladeTextures.Add(Content.Load<Texture2D>($"frameSpinningBlade{i}"));
            }

            // Cloud Textures

            for (int i = 0; i < 23; i++)
            {
                doubleJumpTextures.Add(Content.Load<Texture2D>($"frame_{i}_delay-0.2s"));
            }

            // Door Textures;

            for (int i = 1; i < 5; i++)
            {
                doorTextures.Add(Content.Load<Texture2D>($"CroppedDoor-imageonline.co-63895-{i}"));
            }


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState(); // Delete Later 
            xPosition = mouseState.X;
            yPosition = mouseState.Y;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            if (screen == Screen.LevelOne)
            {
                stickman.Update(gameTime, barriers);
                
                foreach (SpinningBlade spinningBlade in spinningBlades)
                    spinningBlade.Update(gameTime, stickman);

                foreach (DoubleJump doubleJump in doubleJumps)
                    doubleJump.Update(gameTime, stickman);

                foreach (EndLevelDoor endDoors in endingDoors)
                    endDoors.Update(gameTime, stickman);
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            // Drawing 

            foreach (DoubleJump doubleJump in doubleJumps)
                doubleJump.Draw(_spriteBatch);

            foreach (EndLevelDoor endDoors in endingDoors)
                endDoors.Draw(_spriteBatch);

            foreach (Rectangle barrier in barriers)
                _spriteBatch.Draw(wallTexture, barrier, Color.Black);

            foreach (SpinningBlade spinningBlade in spinningBlades)
                spinningBlade.Draw(_spriteBatch);

            stickman.Draw(_spriteBatch);


            _spriteBatch.DrawString(cordinates, $"{xPosition}, {yPosition}", new Vector2(10, 10), Color.Black);         

            _spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}