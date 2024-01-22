using Microsoft.Xna.Framework;
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
        List<Texture2D> stickmanDancingTextures;
        List<Texture2D> levelSelectLevelTextures;
        int stickmanDancingFrame = 0;      
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
        SpriteFont titleFont, deathCounterText;
        bool playingGame = false;
        Rectangle playRect, controlsRect, creditsRect, levelSelectRect;
        Texture2D playTexture1, playTexture2, controlsTexture1, controlsTexture2, creditsTexture1, creditsTexture2, levelSelectTexture1,levelSelectTexture2, levelSelectTexture, playTexture, creditsTexture, controlsTexture;
        Vector2 mouseLocation;
        int level, totalDeaths;
        private float _animationTimeStamp;
        private float _animationInterval = 0.08f;
        private float _animationTime;
        bool levelSelect = false;


        enum Screen
        {
            Intro,
            Controls,
            Credits,
            LevelSelect,
            LevelOne,
            LevelTwo,
            LevelThree
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1100;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();
            this.Window.Title = "Stickman";
          
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Button Rectangles

            playRect = new Rectangle(100,90,200,70);
            controlsRect = new Rectangle(100, 390, 200, 70);
            creditsRect = new Rectangle(100, 290, 200, 70);
            levelSelectRect = new Rectangle(100, 190, 200,70);

            // Texture 2D lists

            stickmanTextures = new List<Texture2D>();
            spinningBladeTextures = new List<Texture2D>();
            doubleJumpTextures = new List<Texture2D>();
            doorTextures = new List<Texture2D>();
            stickmanDancingTextures = new List<Texture2D>();
            levelSelectLevelTextures = new List<Texture2D> ();

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
            deathCounterText = Content.Load<SpriteFont>("DeathCounter");
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

            // Dancing Stickman Textures

            for (int i = 0; i < 76; i++)
            {
                stickmanDancingTextures.Add(Content.Load<Texture2D>($"{i}"));
            }

            // Level Select Imags

            levelSelectLevelTextures.Add(Content.Load<Texture2D>("LevelOneFinishedImage"));
            levelSelectLevelTextures.Add(Content.Load<Texture2D>("LevelTwoFinishedImage"));

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
                DancingStickman(gameTime);

                // Level Select Button

                if (mouseState.LeftButton == ButtonState.Pressed && levelSelectRect.Contains(mouseLocation)) 
                {
                    screen = Screen.LevelSelect;
                    levelSelect = true;
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

                // Controls Button

                if (mouseState.LeftButton == ButtonState.Pressed && controlsRect.Contains(mouseLocation)) 
                {
                    screen = Screen.Controls;
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

                // Credits Button

                if (mouseState.LeftButton == ButtonState.Pressed && creditsRect.Contains(mouseLocation))
                {
                    screen = Screen.Credits;
                    creditsTexture = creditsTexture2;

                }
                else if (creditsRect.Contains(mouseLocation))
                {
                    creditsTexture = creditsTexture1;

                }
                else
                {
                    creditsTexture = creditsTexture2;
                }

                // Play Button

                if (mouseState.LeftButton == ButtonState.Pressed && playRect.Contains(mouseLocation))
                {
                    LevelOne();
                    playTexture = playTexture2;
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
                    totalDeaths = stickman.DeathCount;
                    ClearLevel();
                    LevelTwo();
                }
            }
            else if (screen == Screen.LevelTwo)
            {
                if (endingDoors[0].AdvanceLevel)
                {
                    totalDeaths += stickman.DeathCount;
                    ClearLevel();
                    LevelThree();
                }
            }
            else if (screen == Screen.LevelThree)
            {
                if (endingDoors[0].AdvanceLevel)
                {
                    screen = Screen.Intro;
                    ClearLevel();
                    playingGame = false;

                }


            }
            
            // Lets you Return to Lobby

            if (keyboardState.IsKeyDown(Keys.R) && screen != Screen.Intro)
            {
                if (playingGame)
                {
                    totalDeaths = 0;
                    ClearLevel();
                    playingGame = false;
                    levelSelect = false;
                }
                screen = Screen.Intro;
            }

            // All Code Needed to Update level and Play Game

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
                BorderTextures();
                _spriteBatch.DrawString(titleFont, "Stickman", new Vector2(440, 10), Color.Black);
                _spriteBatch.Draw(levelSelectTexture, levelSelectRect, Color.White);
                _spriteBatch.Draw(playTexture, playRect, Color.White);
                _spriteBatch.Draw(controlsTexture, controlsRect, Color.White);
                _spriteBatch.Draw(creditsTexture, creditsRect, Color.White);
                _spriteBatch.Draw(stickmanDancingTextures[stickmanDancingFrame], new Rectangle(650, 85, 400, 385), Color.White);
                
            }
            else if (screen == Screen.LevelSelect)
            {
                BorderTextures();
                _spriteBatch.Draw(levelSelectLevelTextures[0], new Rectangle(40,100,300,200), Color.White);
                _spriteBatch.Draw(levelSelectLevelTextures[1], new Rectangle(380, 100, 300, 200), Color.White);
            }
            else if (screen == Screen.Controls)
            {
                BorderTextures();
            }
            else if (screen == Screen.Credits)
            {
                BorderTextures();
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

                if (level == 1)
                {
                    _spriteBatch.DrawString(deathCounterText, $"Total Deaths: {stickman.DeathCount} ", new Vector2(30, 10), Color.Black);
                }
                else if (level == 2)
                {
                    _spriteBatch.DrawString(deathCounterText, $"Total Deaths: {stickman.DeathCount + totalDeaths}", new Vector2(50, 470), Color.Black);
                }
                else if (level == 3)
                {
                    _spriteBatch.DrawString(deathCounterText, $"Total Deaths: {stickman.DeathCount + totalDeaths}", new Vector2(430, 450), Color.Black);
                }

                stickman.Draw(_spriteBatch);
            }

            _spriteBatch.DrawString(cordinates, $"{xPosition}, {yPosition}", new Vector2(100, 30), Color.Black);

            _spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void ClearLevel()
        {
            barriers.Clear();
            spinningBlades.Clear();
            endingDoors.Clear();
            doubleJumps.Clear();
        }

        public void BorderTextures() // This is only used to make interface look better in the screens that aren't levels.
        {
            _spriteBatch.Draw(wallTexture, new Rectangle(0, 0, 1100, 20), Color.Black);
            _spriteBatch.Draw(wallTexture, new Rectangle(0, 0, 20, 500), Color.Black);
            _spriteBatch.Draw(wallTexture, new Rectangle(1080, 0, 20, 500), Color.Black);
            _spriteBatch.Draw(wallTexture, new Rectangle(0, 480, 1100, 20), Color.Black);
        }

        public void LevelOne()
        {
            level = 1;
            screen = Screen.LevelOne;
            playingGame = true;
            stickman = new Player(stickmanTextures, 24, 400);
            stickman.SpawnPoint = new Vector2(24, 400);

            barriers.Add(new Rectangle(0, 0, 20, 500));
            barriers.Add(new Rectangle(1080, 0, 20, 500));
            barriers.Add(new Rectangle(0, 450, 317, 20));
            barriers.Add(new Rectangle(480, 450, 550, 20));
            barriers.Add(new Rectangle(20, 300, 1080, 20));
            barriers.Add(new Rectangle(20, 118, 100, 20));
            barriers.Add(new Rectangle(400, 118, 20, 20));
            barriers.Add(new Rectangle(540, 118, 220, 20));
            barriers.Add(new Rectangle(1000, 118, 100, 20));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(1030, 373), 30));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(20, 200), 30));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(100, 410), 300, 0, 50, true));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(20, 230), 1010, 8, 70, true));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(50, 355), 300, 3, 50, true));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(480, 400), 978, 5, 50, true));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(630, 0), 90, 2, 40, false));
            endingDoors.Add(new EndLevelDoor(doorTextures, new Vector2(1010, 50), 70));
        }

        public void LevelTwo()
        {
            level = 2;
            screen = Screen.LevelTwo;
            playingGame = true;
            stickman = new Player(stickmanTextures, 24, 152);
            stickman.SpawnPoint = new Vector2(24, 152);
            barriers.Add(new Rectangle(1080, 0, 20, 500));
            barriers.Add(new Rectangle(0, 202, 100, 20));
            barriers.Add(new Rectangle(600, 200, 400, 20));
            barriers.Add(new Rectangle(675, 350, 425, 20));
            barriers.Add(new Rectangle(600, 200, 20, 300));
            barriers.Add(new Rectangle(600, 490, 500, 20));
            barriers.Add(new Rectangle(200, 0, 20, 200));
            barriers.Add(new Rectangle(0, 0, 20, 500));
            barriers.Add(new Rectangle(340, 319, 20, 181));
            barriers.Add(new Rectangle(480, 0, 20, 90));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(100, 300), 300, 2, 50, true)); // Indicates horizontal Direction
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(150, 0), 300, 3, 50, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(100, 0), 300, 3, 50, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(430, 230), 500, 0, 50, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(200, 190), 500, 0, 20, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(340, 310), 500, 0, 20, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(480, 80), 500, 0, 20, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(585, 0), 500, 6, 50, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(690, 0), 500, 3, 50, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(790, 0), 500, 8, 50, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(890, 0), 500, 2, 50, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(990, 0), 500, 9, 50, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(590, 190), 1060, 3, 40, true));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(590, 340), 1060, 2, 40, true));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(590, 480), 1060, 1, 40, true));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(400, 230), 30));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(187, 300), 30));
            endingDoors.Add(new EndLevelDoor(doorTextures, new Vector2(1020, 420), 70));
        }

        public void LevelThree()
        {
            level = 3;
            screen = Screen.LevelThree;
            playingGame = true;
            stickman = new Player(stickmanTextures, 24, 430);
            stickman.SpawnPoint = new Vector2(24, 430); // Og is 24
            barriers.Add(new Rectangle(0, 480, 1100, 20));
            barriers.Add(new Rectangle(0, 0, 1100, 20));
            barriers.Add(new Rectangle(0, 0, 20, 500));
            barriers.Add(new Rectangle(1080, 0, 20, 500));
            barriers.Add(new Rectangle(20, 205, 45, 10));
            barriers.Add(new Rectangle(20, 125, 45, 10));
            barriers.Add(new Rectangle(500, 164, 100, 10));
            barriers.Add(new Rectangle(500, 410, 100, 10));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(95, 400), 30));
            endingDoors.Add(new EndLevelDoor(doorTextures, new Vector2(520, 340), 70));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(20, 275), 30));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(85, 200), 30));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(265, 390), 30));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(320, 390), 30));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(450, 300), 30));
            doubleJumps.Add(new DoubleJump(doubleJumpTextures, new Vector2(320, 220), 30));       
            // Moving Ones
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(60, 174), 460, 3, 20, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(200, 20), 460, 4, 20, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(220, 20), 460, 4, 20, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(180, 20), 460, 4, 20, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(300, 20), 460, 8, 20, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(330, 20), 385, 10, 30, false));
            spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(460, 20), 385, 10, 30, false));



            for (int i = 140; i < 470; i+=10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(130, i), 1060, 0, 20, true));
            }

            for (int i = 355; i < 400; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(80, i), 1060, 0, 20, true));
            }
            for (int i = 20; i < 90; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(i, 355), 1060, 0, 20, true));
            }
            for (int i = 60; i < 260; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(i, 260), 1060, 0, 20, true)); 
            }
            for (int i = 20; i < 1070; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(i, 20), 1060, 0, 20, true));
            }
            for (int i = 130; i < 270; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(60, i), 1060, 0, 20, true));
            }
            for (int i = 20; i < 420; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(300, i), 1060, 0, 20, true));
            }
            for (int i = 180; i < 350; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(i, 410), 1060, 0, 20, true));
            }
            for (int i = 160; i < 410; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(490, i), 1060, 0, 20, true));
            }
            for (int i = 400; i < 500; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(i, 410), 1060, 0, 20, true));
            }
            for (int i = 410; i < 470; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(400, i), 1060, 0, 20, true));
            }
            for (int i = 100; i < 170; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(585, i), 1060, 0, 20, true));
            }
            for (int i = 595; i < 1070; i += 10)
            {
                spinningBlades.Add(new SpinningBlade(spinningBladeTextures, new Vector2(i, 405), 1060, 0, 20, true));
            }
        }

        public void DancingStickman(GameTime gameTime)
        {
            _animationTime = (float)gameTime.TotalGameTime.TotalSeconds - _animationTimeStamp;
            if (_animationTime > _animationInterval)
            {
                _animationTimeStamp = (float)gameTime.TotalGameTime.TotalSeconds;
                 stickmanDancingFrame += 1;
                if (stickmanDancingFrame >= 76)
                {
                    stickmanDancingFrame = 0;
                }
            }
        }

    }
}