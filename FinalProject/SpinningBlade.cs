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

namespace FinalProject
{
    class SpinningBlade
    {
        private List<Texture2D> _spinningBladeTextures;
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _velocity;
        private int _endingDistance;
        private float _startingDistance;// Maybe Make Vector 2 Like Other Class
        private float _speed;
        private bool _goingRight;
        private bool _horizontalDirection;

        public SpinningBlade(List<Texture2D> bladeTextures, Vector2 spawnPoint, int endingPoint, float speed, int size, bool horizontalDirection, bool goingRight) // Default Spinning Blade
        {
            _spinningBladeTextures = bladeTextures;
            _endingDistance = endingPoint;
            _location = new Rectangle((int)spawnPoint.X, (int)spawnPoint.Y, size, size);
            _startingDistance = spawnPoint.X;
            _velocity = new Vector2();
            _texture = bladeTextures[0];
            _goingRight = goingRight;
            _horizontalDirection = horizontalDirection;
            _speed = speed;
        }
        public void Update(GameTime gameTime)
        {
            if (_horizontalDirection)
            {
                _velocity.X = _speed;
            }
            else if (!_horizontalDirection)
            {
                _velocity.Y = _speed;
            }

            _location.X += (int)_velocity.X;
            _location.Y += (int)_velocity.Y;

            if (_location.X > _endingDistance && _goingRight)
            {
                _velocity.X *= -1;
                _goingRight = false;
            }
            else if (_location.X < _startingDistance && !_goingRight)
            {
                _velocity *= -1;
                _goingRight = true;
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
