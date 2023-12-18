using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Player
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _velocity;
        private Vector2 _speed;
        private bool _hasJumped = false;
        private int _speedX = 3;


        public Player(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 30, 30);
            _velocity = new Vector2();
        }
        public float Hspeed
        {
            get { return _speed.X; }
            set { _speed.X = value; }
        }
        
        public float Vspeed
        {
            get { return _speed.Y; }
            set { _speed.Y = value; }
        }
        public bool Collide(Rectangle item)
        {
            return _location.Intersects(item);
        }
        public void UndoMove()
        {
            _location.X += (int)_velocity.X;
            _location.Y += (int)_velocity.X;
        }

        public void Update(GameTime gameTime)
        {
            var keyboardstate = Keyboard.GetState();

            _location.X += (int)_velocity.X;
            _location.Y += (int)_velocity.Y;
            if (keyboardstate.IsKeyDown(Keys.D))
                _velocity.X = _speedX;
            else if (keyboardstate.IsKeyDown(Keys.A))
                _velocity.X = -_speedX;
            else
                _velocity.X = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _hasJumped == false)
            {
                _location.Y -= 10;
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
    
 

