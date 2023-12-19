﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Player
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _velocity;
        private bool _hasJumped = false;
        private int _speedX = 3;
        private float _maxSpeed = 1.5f;
        private float _acceleration = 1;


        public Player(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 30, 30);
            _velocity = new Vector2();
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

            // Horizontal movement
            _location.X += (int)_velocity.X * (int)_acceleration;
            foreach (Rectangle barrier in barriers)
                if (this.Collide(barrier))
                    _location.X -= (int)_velocity.X * (int)_acceleration;

            // Vertical movement
            _location.Y += (int)_velocity.Y;
            foreach (Rectangle barrier in barriers)
                if (this.Collide(barrier))
                {
                    if (_velocity.Y > 0)
                    {

                        _velocity.Y = 0;
                        _hasJumped = false;
                        _location.Y = barrier.Y - _location.Height;
                    }
                    else
                    {
                        _velocity.Y = 0;
                        _hasJumped = false;
                        _location.Y = barrier.Bottom;
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

            if (_hasJumped == true)
            {
                float i = 1;
                _velocity.Y += 0.15f * i; // if number higher then will go faster
            }

            if (_hasJumped == false)
            {
                _velocity.Y = 0f;
            }
        
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}
    
 
