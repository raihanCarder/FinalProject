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
        private int _startingDistance;// Maybe Make Vector 2 Like Other Class
        private bool _goingRight;

        public SpinningBlade(List<Texture2D> bladeTextures, int x, int y, int endingPoint, float speed, int size, bool horizontalDirection, bool goingRight) // Default Spinning Blade
        {
            _spinningBladeTextures = bladeTextures;
            _endingDistance = endingPoint;
            _location = new Rectangle(x, y, size, size);
            _startingDistance = x;
            _velocity = new Vector2();
            _texture = bladeTextures[0];
            _goingRight = goingRight;
            //if (horizontalDirection)
            //{
            //    _velocity.X = speed;
            //}
            //else if (!horizontalDirection)
            //{
            //    _velocity.Y = speed;
            //}
            _velocity.X = speed;

        }
        public void Update(GameTime gameTime)
        {
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
