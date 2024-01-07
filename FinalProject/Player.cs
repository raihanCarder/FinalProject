using Microsoft.Xna.Framework;
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

namespace FinalProject
{
    class Player
    {
        Texture2D _texture;
        private List<Texture2D> _stickmanTextures;

        private KeyboardState _keyboardState;
        private KeyboardState _oldstate;
        private Rectangle _location;
        private Vector2 _velocity;
        private bool _hasJumped = false;
        private bool _isRunning = false;
        private int _speedX = 3;
        private float _maxSpeed = 1.5f;
        private float _acceleration = 1;
        private SpriteEffects _direction;
        private bool _grounded;
        private int frameCounter = 0;
        private float _animationTimeStamp;
        private float _animationInterval = 0.08f;
        private float _animationTime;

        // Animations
        // Standing is 0
        // Jumping 3-19
        // Running 21-39

        public Player(List<Texture2D> stickmanTextures,  int x, int y) // Actual Player Constructor
        {
            _stickmanTextures = stickmanTextures;
            _location = new Rectangle(x, y, 45, 45);
            _velocity = new Vector2();
            _direction = SpriteEffects.None;
            _texture = _stickmanTextures[frameCounter]; // In update always change Texture to texture wanted.
        }

        public Player(Texture2D texture, int x, int y) // Used for Testing Purposes
        {
            _texture = texture;
            _location = new Rectangle(x, y, 45, 45);
            _velocity = new Vector2();
            _direction = SpriteEffects.None;
        }

        public float Yvelocity
        {
            get { return _velocity.Y; }
            set { _velocity.Y = value; }
        }

        public bool isJumping
        {
            get { return _hasJumped; }
            set { _hasJumped = value;}
        }

        public bool Collide(Rectangle item)
        {
            return _location.Intersects(item);
        }
    
        public void Update(GameTime gameTime, List<Rectangle> barriers)
        {

            _keyboardState = Keyboard.GetState();
            KeyboardState newState = Keyboard.GetState();
            _grounded = false;
            _texture = _stickmanTextures[frameCounter];


            // Horizontal movement
            _location.X += (int)_velocity.X * (int)_acceleration;
            foreach (Rectangle barrier in barriers)
                if (this.Collide(barrier))
                    _location.X -= (int)_velocity.X * (int)_acceleration;

            // Vertical movement
            _location.Y += (int)_velocity.Y;

            foreach (Rectangle barrier in barriers)
            {
                if (this.Collide(barrier))
                {
                    if (_velocity.Y > 0) // makes it so you can go thru the floor when platforming
                    {
                        _velocity.Y = 0;
                        _hasJumped = false;
                        _location.Y = barrier.Y - _location.Height;
                        if (_velocity.X != 0)
                        {
                            _isRunning = true;
                            if (frameCounter == 19)
                                frameCounter = 31;
                        }
                    }

                    if (_velocity.Y == 0)
                        _grounded = true;

                }

                if (!this.Collide(barrier) && _velocity.Y == 0)
                {
                    float i = 1;
                    _velocity.Y -= 0.15f * i;
                }

            }

            // Movement Code 

            // Starts running animation

            if (_oldstate.IsKeyUp(Keys.A) && newState.IsKeyDown(Keys.A) && !_hasJumped || _oldstate.IsKeyUp(Keys.D) && newState.IsKeyDown(Keys.D) && !_hasJumped)
            {
                _isRunning = true;
                frameCounter = 31;
            }

            if (_isRunning /*&& _velocity.Y != 0*/)
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
                _velocity.X = _speedX;
                if (!_hasJumped && _acceleration <= _maxSpeed)
                {
                    _acceleration += 0.05f;
                }
            }
            else if (_keyboardState.IsKeyDown(Keys.A))
            {
                _velocity.X = -_speedX;
                if (!_hasJumped && _acceleration <= _maxSpeed)
                {
                    _acceleration += 0.05f;
                }
            }
            else
            {
                _velocity.X = 0;
                _acceleration = 1;
            }

            // Jump Code

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _hasJumped == false)
            {
                _isRunning = false;
                frameCounter = 3; // Start of Jump
                _location.Y -= 20;
                _velocity.Y = -5f;
                _hasJumped = true;
            }

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

           
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, null, Color.White, 0f, new Vector2(), _direction, 1f);
        }
    }
}
    
 

