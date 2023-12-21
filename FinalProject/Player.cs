using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Player
    {
        Texture2D _texture;
        private List<Texture2D> _walkingTextures;
        private List<Texture2D> _jumpingTextures;
        private Rectangle _location;
        private Vector2 _velocity;
        private bool _hasJumped = false;
        private int _speedX = 3;
        private float _maxSpeed = 1.5f;
        private float _acceleration = 1;
        private SpriteEffects _direction;
        private bool _grounded;

        // Needs To Pull in two Lists in For Animations

        public Player(List<Texture2D> walkingTextures, List<Texture2D> jumpingTextures,  int x, int y) // Actual Player Constructor
        {
            _walkingTextures = walkingTextures; 
            _location = new Rectangle(x, y, 30, 30);
            _velocity = new Vector2();
            _direction = SpriteEffects.None;
            _texture = _walkingTextures[0]; // In update always change Texture to texture wanted.
        }

        public Player(Texture2D texture, int x, int y) // Used for Testing Purposes
        {
            _texture = texture;
            _location = new Rectangle(x, y, 30, 30);
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
            var keyboardstate = Keyboard.GetState();
            _grounded = false;

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
                    }

                    if (_velocity.Y == 0)
                        _grounded = true;

                    //else // Makes it so you stick to bottom of barrier
                    //{
                    //    _velocity.Y = 0;
                    //    _hasJumped = false;
                    //    _location.Y = barrier.Bottom;
                    //}
                }

                if (!this.Collide(barrier) && _velocity.Y == 0)
                {               
                    float i = 1;
                    _velocity.Y -= 0.15f * i;
                }

            }



            if (keyboardstate.IsKeyDown(Keys.D))
            {
                _velocity.X = _speedX;
                if (!_hasJumped && _acceleration <= _maxSpeed)
                {
                    _acceleration += 0.05f;
                }
            }
            else if (keyboardstate.IsKeyDown(Keys.A))
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

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _hasJumped == false)
            {
                _location.Y -= 20;
                _velocity.Y = -5f;
                _hasJumped = true;
            }

            if (_hasJumped)
            {
                float i = 1;
                _velocity.Y += 0.15f * i; // if number higher then will go faster
            }

            if (!_hasJumped && _grounded)
            {
                _velocity.Y = 0f;
            }

            if (!_grounded && !_hasJumped) // Gravity
            {
                float i = 1;
                _velocity.Y += 0.5f * i;
            }

            if (_velocity.X < 0)    // Makes it so I can only you one spritesheet and it'll flip auto
                _direction = SpriteEffects.FlipHorizontally;
            if (_velocity.X > 0)
                _direction = SpriteEffects.None;


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, null, Color.White, 0f, new Vector2(), _direction, 1f);
        }
    }
}
    
 

