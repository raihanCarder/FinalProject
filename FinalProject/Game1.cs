﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Numerics;
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
        Texture2D stickmanSpritesheet;
        Texture2D spawnPoint;
        Texture2D endPoint;
        Texture2D circleHitboxTexture; // Shows Circular Hitbox
        Circle hitboxCircle;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Player stickman;
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
            hitboxCircle = new Circle(new Microsoft.Xna.Framework.Vector2(500,350), 50f);
            stickmanTextures = new List<Texture2D>();
            screen = Screen.LevelOne; // CHANGE TO INTRO
            base.Initialize();
            stickman = new Player(stickmanTextures, 10, 10); // Testing Sprite
            barriers = new List<Rectangle>();
            if (screen == Screen.LevelOne)
            {         
                barriers.Add(new Rectangle(0, 495, 1100, 50));
                barriers.Add(new Rectangle(100, 460, 20, 20));
                barriers.Add(new Rectangle(600, 400, 100, 20));
            }
      
        }

        protected override void LoadContent()
        {
            circleHitboxTexture = Content.Load<Texture2D>("circle");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            wallTexture = Content.Load<Texture2D>("rectangle");
            stickmanSpritesheet = Content.Load<Texture2D>("BlackStickmanRight");

            int width = stickmanSpritesheet.Width / 8;
            int height = stickmanSpritesheet.Height / 5;

            // Jumping Textures

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

           

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
          

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            if (screen == Screen.LevelOne)
            {
                stickman.Update(gameTime, barriers); 

                if (hitboxCircle.Intersects(stickman.Location))
                {
                    
                }


            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            stickman.Draw(_spriteBatch);

            foreach (Rectangle barrier in barriers)
                _spriteBatch.Draw(wallTexture, barrier, Color.Black);

            _spriteBatch.Draw(circleHitboxTexture,hitboxCircle.DrawRect, Color.Black);



            _spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}