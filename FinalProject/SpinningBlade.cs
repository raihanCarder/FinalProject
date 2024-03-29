﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FinalProject // YUH
{
    public class SpinningBlade
    {
        private List<Texture2D> _spinningBladeTextures;
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _velocity;
        private Vector2 _spawnPoint;
        private int _endingDistance;
        private float _startingDistanceX;
        private float _startingDistanceY;
        private bool _horizontalDirection;
        private int _frameCounter = 0;
        private float _animationTimeStamp;
        private float _animationInterval = 0.05f;
        private float _animationTime;


        public SpinningBlade(List<Texture2D> bladeTextures, Vector2 spawnPoint, int endingPoint, float speed, int size, bool horizontalDirection) // Default Spinning Blade
        {
            _spinningBladeTextures = bladeTextures;
            _endingDistance = endingPoint;
            _location = new Rectangle((int)spawnPoint.X, (int)spawnPoint.Y, size, size);
            _spawnPoint = new Vector2(spawnPoint.X, spawnPoint.Y);     
            _velocity = new Vector2();
            _texture = bladeTextures[_frameCounter];
            _horizontalDirection = horizontalDirection;
            _velocity.X = speed;
            _velocity.Y = speed;
            
        }       

        public void Update(GameTime gameTime, Player stickman)
        {

            // Movement

            if (_horizontalDirection)
            {
                _startingDistanceX = _spawnPoint.X;
                _location.X += (int)_velocity.X;
            }
            else
            {
                _startingDistanceY = _spawnPoint.Y;
                _location.Y += (int)_velocity.Y;
            }

            // Animation

            _animationTime = (float)gameTime.TotalGameTime.TotalSeconds - _animationTimeStamp;
            if (_animationTime > _animationInterval)
            {
                _animationTimeStamp = (float)gameTime.TotalGameTime.TotalSeconds;
                _frameCounter += 1;
                if (_frameCounter >= 3)
                {
                    _frameCounter = 0;
                }
            }
            _texture = _spinningBladeTextures[_frameCounter];

            // Making Blade go back and forth

            if (_horizontalDirection) 
            {
                if (_location.X > _endingDistance)
                {
                    _velocity.X *= -1;
                }
                else if (_location.X < _startingDistanceX)
                {
                    _velocity.X *= -1;
                }
            }
            else if (!_horizontalDirection)
            {
                if (_location.Y > _endingDistance)
                {
                    _velocity.Y *= -1;
                }
                else if (_location.Y < _startingDistanceY)
                {
                    _velocity.Y *= -1;
                }
            }

            // Intersects Player Code

            if (_location.Intersects(stickman.CollisonRectangle))
            {
                stickman.DeathCount++;
                stickman.isJumping = false;
                stickman.Yvelocity = 0;
                stickman.Xvelocity = 0;
                stickman.XLocation = (int)stickman.SpawnPoint.X;
                stickman.YLocation = (int)stickman.SpawnPoint.Y;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}
