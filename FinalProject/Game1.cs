﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        KeyboardState keyboardState;
        int xPosition, yPosition;
        SpriteFont cordinates;
        SpriteFont titleFont;
        bool playingGame = false;
        Rectangle playRect, controlsRect, creditsRect, levelSelectRect;
        Texture2D playTexture1, playTexture2, controlsTexture1, controlsTexture2, creditsTexture1, creditsTexture2, levelSelectTexture1,levelSelectTexture2, levelSelectTexture, playTexture, creditsTexture, controlsTexture;
        Vector2 mouseLocation;


        enum Screen
        {
            Intro,
            Controls,
            Credits,
            LevelSelect,
            LevelOne,
            LevelTwo
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

            // Button Rectangles

            playRect = new Rectangle(100,100,200,70);
            controlsRect = new Rectangle(400, 100, 70, 40);
            creditsRect = new Rectangle(600, 100, 70, 40);
            levelSelectRect = new Rectangle(800, 100, 200,70);


            // Texture 2D lists

            stickmanTextures = new List<Texture2D>();
            spinningBladeTextures = new List<Texture2D>();
            doubleJumpTextures = new List<Texture2D>();
            doorTextures = new List<Texture2D>();

            // Screen

            screen = Screen.Intro; // CHANGE TO INTRO
            base.Initialize();

            // Initialize all other Lists

            barriers = new List<Rectangle>();
            spinningBlades = new List<SpinningBlade>();
            doubleJumps = new List<DoubleJump>();
            endingDoors = new List<EndLevelDoor>();
     
        }

        protected override void LoadContent()
        {
            // Loading Content
            controlsTexture1 = Content.Load<Texture2D>("button_controls");
            controlsTexture2 = Content.Load<Texture2D>("button_controls (1)");
            levelSelectTexture1 = Content.Load<Texture2D>("button_level-select");
            levelSelectTexture2 = Content.Load<Texture2D>("button_level-select (1)");
            playTexture1 = Content.Load<Texture2D>("button_play");
            playTexture2 = Content.Load<Texture2D>("button_play (1)");
            creditsTexture1 = Content.Load<Texture2D>("button_credits");
            creditsTexture2 = Content.Load<Texture2D>("button_credits (1)");
            titleFont = Content.Load<SpriteFont>("Title");
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
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState(); // Delete Later 
            mouseLocation = new Vector2(mouseState.X, mouseState.Y);
            xPosition = mouseState.X;
            yPosition = mouseState.Y;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            if (screen == Screen.Intro)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && levelSelectRect.Contains(mouseLocation)) // Level Select Button
                {
                    screen = Screen.LevelSelect;
                    levelSelectTexture = levelSelectTexture2;

                }
                else if (levelSelectRect.Contains(mouseLocation))
                {
                    levelSelectTexture = levelSelectTexture1;
                }
                else
                {
                    levelSelectTexture = levelSelectTexture2;
                }

                if (mouseState.LeftButton == ButtonState.Pressed && controlsRect.Contains(mouseLocation)) // Controls Button
                {
                    screen = Screen.LevelSelect;
                    controlsTexture = controlsTexture2;

                }
                else if (controlsRect.Contains(mouseLocation))
                {
                    controlsTexture = controlsTexture1;
                }
                else
                {
                    controlsTexture = controlsTexture2;
                }

                if (mouseState.LeftButton == ButtonState.Pressed && creditsRect.Contains(mouseLocation))
                {
                    screen = Screen.Credits;
                    creditsTexture = creditsTexture2;

                }
                else if (controlsRect.Contains(mouseLocation))
                {
                    creditsTexture = creditsTexture1;

                }
                else
                {
                    creditsTexture = creditsTexture2;
                }

                if (mouseState.LeftButton == ButtonState.Pressed && playRect.Contains(mouseLocation)) // Play Button
                {
                    screen = Screen.LevelOne;
                    playTexture = playTexture2;

                    // Initializes Level One Code

                    playingGame = true;
                    stickman = new Player(stickmanTextures, 24, 400);

                    stickman.SpawnPoint = new Vector2(24, 400);
                    barriers.Add(new Rectangle(0, 450, 700, 20));
                    barriers.Add(new Rectangle(600, 380, 100, 20));
                    spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(150, 300), 300, 2, 50, true)); // How to add Blades
                    spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(150, 300), 500, 2, 50, false));
                    spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(500, 400), 500, 0, 50, false));
                    doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(400, 400), 30));
                    doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(600, 340), 50));
                    endingDoors.Add(new EndLevelDoor(doorTextures, new Vector2(350, 375), 70));

                }
                else if (playRect.Contains(mouseLocation))
                {
                    playTexture = playTexture1;
                }
                else
                {
                    playTexture = playTexture2;
                }
                

            }
            else if (screen == Screen.LevelOne)
            {
                if (endingDoors[0].AdvanceLevel)
                {
                    screen = Screen.LevelTwo;
                    barriers.Clear();
                    spinningBlades.Clear();
                    endingDoors.Clear();
                    doubleJumps.Clear();

                    // New level Code make Sure to Move Stickman to new Spawnpoint;

                    barriers.Add(new Rectangle(0, 450, 700, 20));
                    barriers.Add(new Rectangle(600, 400, 100, 20));
                }
            }

            if (playingGame)
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

            if (screen == Screen.Intro)
            {
                _spriteBatch.DrawString(titleFont, "Stickman", new Vector2(400, 10), Color.Black);
                _spriteBatch.Draw(levelSelectTexture, levelSelectRect, Color.White);
                _spriteBatch.Draw(playTexture, playRect, Color.White);
                _spriteBatch.Draw(controlsTexture, controlsRect, Color.White);
                _spriteBatch.Draw(creditsTexture, creditsRect, Color.White);
            }
            else if (screen == Screen.LevelOne)
            {
               
            }
            else if (screen == Screen.LevelTwo)
            {

            }

            if (playingGame)
            {
                foreach (EndLevelDoor endDoors in endingDoors)
                    endDoors.Draw(_spriteBatch);
                foreach (DoubleJump doubleJump in doubleJumps)
                    doubleJump.Draw(_spriteBatch);
                foreach (Rectangle barrier in barriers)
                   _spriteBatch.Draw(wallTexture, barrier, Color.Black);
                foreach (SpinningBlade spinningBlade in spinningBlades)
                   spinningBlade.Draw(_spriteBatch);

                    stickman.Draw(_spriteBatch);
            }


            _spriteBatch.DrawString(cordinates, $"{xPosition}, {yPosition}", new Vector2(10, 10), Color.Black);         

            _spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}