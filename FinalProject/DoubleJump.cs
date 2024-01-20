using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class DoubleJump // Clouds that player can use to Jump
    {
        private List<Texture2D> _cloudTextures;
        private Texture2D _texture;
        private Rectangle _location;
        private Color _color;
        private Vector2 _position;
        private int _frameCounter;
        private float _animationTimeStamp;
        private float _animationInterval = 0.1f;
        private float _animationTime;

        public DoubleJump(List<Texture2D> textures, Vector2 position, int size) 
        {
            _cloudTextures = textures;
            _frameCounter = 0;
            _texture = textures[_frameCounter];
            _color = Color.White;
            _position = position;
            _location = new Rectangle((int)_position.X, (int)_position.Y, size+10, size);
        }

        public Rectangle Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public void Update(GameTime gameTime, Player stickman)
        {
            // Collision

            if (_location.Intersects(stickman.CollisonRectangle))
            {              
                stickman.isJumping = false;
                stickman.Grounded = true;
            }

            // Animation 

            _animationTime = (float)gameTime.TotalGameTime.TotalSeconds - _animationTimeStamp;
            if (_animationTime > _animationInterval)
            {
                _animationTimeStamp = (float)gameTime.TotalGameTime.TotalSeconds;
                _frameCounter += 1;
                if (_frameCounter >= 23)
                {
                    _frameCounter = 0;
                }
            }

            _texture = _cloudTextures[_frameCounter];

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, _color);
        }
    }
}
