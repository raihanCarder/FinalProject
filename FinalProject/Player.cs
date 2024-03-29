﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FinalProject
{
    public class Player
    {
        Texture2D _texture;
        private List<Texture2D> _stickmanTextures;

        private KeyboardState _keyboardState;
        private KeyboardState _oldstate;
        private Rectangle _location;
        private Rectangle _collisionRectangle; // Update this rectangle with location rectangle except offset it then make all collision 
        private Vector2 _velocity;
        private bool _hasJumped = false;
        private bool _isRunning = false;
        private int _speedX = 4;
        private float _acceleration = 1.05f;
        private SpriteEffects _direction;
        private bool _grounded;
        private int frameCounter = 0;
        private float _animationTimeStamp;
        private float _animationInterval = 0.06f;
        private float _animationTime;
        private Vector2 _spawnPoint;
        private int _deaths;

        

        // Animations
        // Standing is 0
        // Jumping 3-19
        // Running 21-39

        public Player(List<Texture2D> stickmanTextures,  int x, int y) // Actual Player Constructor
        {
            _spawnPoint = new Vector2(x, y);
            _stickmanTextures = stickmanTextures;
            _location = new Rectangle(x, y, 45, 45);
            _collisionRectangle = new Rectangle(x + 15, y+5, 15, 45-10);
            _velocity = new Vector2();
            _direction = SpriteEffects.None;
            _texture = _stickmanTextures[frameCounter]; // In update always change Texture to texture wanted.
            _deaths = 0;
        }

        public Rectangle CollisonRectangle // Used for Collision
        {
            get { return _collisionRectangle; }
            set { _collisionRectangle = value; }
        }
        public int XLocation
        {
            get { return _collisionRectangle.X; }
            set { _collisionRectangle.X = value; }
        }
        public int YLocation
        {
            get { return _collisionRectangle.Y; }
            set { _collisionRectangle.Y = value; }
        }

        public Vector2 SpawnPoint
        {
            get { return _spawnPoint; }
            set { _spawnPoint = value; }
        }
        public int DeathCount
        {
            get { return _deaths; }
            set { _deaths = value; }
        }
        public float Yvelocity
        {
            get { return _velocity.Y; }
            set { _velocity.Y = value; }
        }
        public float Xvelocity
        {
            get { return _velocity.X; }
            set { _velocity.X = value; }
        }

        public bool isJumping
        {
            get { return _hasJumped; }
            set { _hasJumped = value;}
        }

        public bool CollisionCollide(Rectangle item)
        {
            return _collisionRectangle.Intersects(item);
        }

        public bool Grounded
        {
            get { return _grounded; }
            set { _grounded = value; }
        }

        public void Update(GameTime gameTime, List<Rectangle> barriers)
        {

            _keyboardState = Keyboard.GetState();
            KeyboardState newState = Keyboard.GetState();
            //_grounded = false;
            _texture = _stickmanTextures[frameCounter];
            _location.X = _collisionRectangle.X - 15;
            _location.Y = _collisionRectangle.Y - 10;
            // Horizontal movement
            _collisionRectangle.X += (int)_velocity.X;

            foreach (Rectangle barrier in barriers)
                if (this.CollisionCollide(barrier))
                    _collisionRectangle.X -= (int)_velocity.X * (int)_acceleration;


            // Vertical Movement
            _collisionRectangle.Y += (int)_velocity.Y;

            foreach (Rectangle barrier in barriers)
            {
                if (this.CollisionCollide(barrier))
                {
                    if (_velocity.Y > 0) // makes it so you can go thru the floor when platforming
                    {
                        _velocity.Y = 0;
                        _hasJumped = false;

                        //_collisionRectangle.Y = barrier.Y - _collisionRectangle.Height;

                        if (_velocity.X != 0)
                        {
                            _isRunning = true;
                            if (frameCounter == 19)
                                frameCounter = 31;

                        }
                    }

                    if (_velocity.Y == 0)
                        _collisionRectangle.Y = barrier.Y - _collisionRectangle.Height;


                    if (_velocity.Y == 0)
                    {
                        _grounded = true;
                    }



                }

                // Gravity

                if (!this.CollisionCollide(barrier) && _velocity.Y == 0)
                {
                    float i = 1;
                    _velocity.Y -= 0.15f * i;
                }




            }

            // Collision Rectangle Code

            _location.X = _collisionRectangle.X - 15;
            _location.Y = _collisionRectangle.Y - 8;

            // Movement Code 

            // Starts running animation

            if (_oldstate.IsKeyUp(Keys.A) && newState.IsKeyDown(Keys.A) && !_hasJumped || _oldstate.IsKeyUp(Keys.D) && newState.IsKeyDown(Keys.D) && !_hasJumped)
            {
                _isRunning = true;
                frameCounter = 31;
            }

            if (_isRunning)
            {
                _animationTime = (float)gameTime.TotalGameTime.TotalSeconds - _animationTimeStamp;
                if (_animationTime > _animationInterval)
                {
                    _animationTimeStamp = (float)gameTime.TotalGameTime.TotalSeconds;
                    frameCounter += 1;
                    if (frameCounter >= 39)
                    {
                        frameCounter = 31;
                    }
                }

            }

            _oldstate = newState;

            if (_keyboardState.IsKeyDown(Keys.D))
            {
                _velocity.X = _speedX * (int)_acceleration;
            }
            else if (_keyboardState.IsKeyDown(Keys.A))
            {
                _velocity.X = -_speedX * (int)_acceleration;
            }
            else
            {
                _velocity.X = 0;
            }

            // Jump Code


            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _hasJumped == false && _grounded)
            { 
                _isRunning = false;
                frameCounter = 3; // Start of Jump           
                _collisionRectangle.Y -= 20;
                _velocity.Y = -5f;
                _hasJumped = true;
            }
            _grounded = false;
            // Jumping code with Animation


            if (_hasJumped)
            {
                if (frameCounter < 19)
                    frameCounter++;
                float i = 1;
                _velocity.Y += 0.15f * i; // if number higher then will go faster
            }

            // Makes sure that when grounded and Not Moving Player stays still

            if (!_hasJumped && _grounded && !_isRunning)
            {
                _velocity.Y = 0f;
                frameCounter = 0;
            }
            else if (_velocity.X == 0 && _isRunning)
                frameCounter = 0;

            // Gravity

            
            if (!_grounded && !_hasJumped)
            {
                float i = 1;
                _velocity.Y += 0.5f * i;
            }

            // Flips Spritesheet 


            if (_velocity.X < 0)    // Makes it so I can only you one spritesheet and it'll flip auto
            {
                _direction = SpriteEffects.FlipHorizontally; 
            }
            if (_velocity.X > 0)
            {
                _direction = SpriteEffects.None;
            }

            // When fallen off map it'll make you respawn

            if (_location.Y > 520)
            {             
                _collisionRectangle.X = (int)SpawnPoint.X;
                _collisionRectangle.Y = (int)SpawnPoint.Y;
                _deaths++;
            }

            // Makes frame 0 if Standing Still

            if (!_grounded && _velocity.X == 0 && !_hasJumped)
            {
                frameCounter = 0;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, null, Color.White, 0f, new Vector2(), _direction, 1f);
        }
    }
}
    
 

