using Microsoft.Xna.Framework;
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

namespace FinalProject
{
    public class SpinningBlade
    {
        private List<Texture2D> _spinningBladeTextures;
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _velocity;
        private int _endingDistance;
        private float _startingDistance;// Maybe Make Vector 2 Like Other Class
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
            _startingDistance = spawnPoint.X;
            _velocity = new Vector2();
            _texture = bladeTextures[_frameCounter];
            _horizontalDirection = horizontalDirection;
            _velocity.X = speed; // if wanted to reverse directions for start make this negative speed;
            _velocity.Y = speed;
            
        }
        public void Update(GameTime gameTime, Player stickman)
        {

            // Movement

            if (_horizontalDirection)
                _location.X += (int)_velocity.X;
            else
                _location.Y += (int)_velocity.Y;

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

            if (_horizontalDirection) {
                if (_location.X >= _endingDistance)
                {
                    _velocity.X *= -1;
                }
                else if (_location.X <= _startingDistance)
                {
                    _velocity.X *= -1;
                }
            }
            else
            {
                if (_location.Y >= _endingDistance)
                {
                    _velocity.Y *= -1;
                }
                else if (_location.Y <= _startingDistance)
                {
                    _velocity.Y *= -1;
                }
            }

            // Intersects Player Code

            if (stickman.Frame == 0 || stickman.Frame >=3 && stickman.Frame<=19)
            {
                if (_location.Intersects(stickman.CollisonRectangle))
                {
                    stickman.XLocation = 10;
                }
            }
            else
            {
                if (_location.Intersects(stickman.Location))
                {
                    stickman.XLocation = 10;
                }
            }


        }

        public bool Collide(Rectangle item)
        {
            return _location.Intersects(item);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}
