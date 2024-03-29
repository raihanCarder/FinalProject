﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class EndLevelDoor // End Point of a Level That player must Reach
    {
        private List<Texture2D> _doorTextures;
        private Rectangle _location;
        private Rectangle _collisionRectangle;
        private Texture2D _texture;
        private int _frameCounter;
        private float _animationTimeStamp;
        private float _animationInterval = 0.3f;
        private float _animationTime;
        private bool _advance;
        private float _endingTime;

        public EndLevelDoor(List<Texture2D> textures, Vector2 position, int size)
        {
          _doorTextures = textures;
          _texture = textures[0];
          _location = new Rectangle((int)position.X, (int)position.Y, size, size+10);
          _collisionRectangle = new Rectangle((int)position.X, (int)position.Y, size/2, size);
          _frameCounter = 0;
        }

        public void Update(GameTime gameTime, Player stickman)
        {
            // Animation 

            if (_collisionRectangle.Intersects(stickman.CollisonRectangle) && _frameCounter <=3)
            {
                _animationTime = (float)gameTime.TotalGameTime.TotalSeconds - _animationTimeStamp;
                if (_animationTime > _animationInterval)
                {
                    _animationTimeStamp = (float)gameTime.TotalGameTime.TotalSeconds;
                    _frameCounter += 1;
                }

                if (_frameCounter<3)
                _texture = _doorTextures[_frameCounter];
            }

            if (_frameCounter == 4 && _collisionRectangle.Intersects(stickman.CollisonRectangle))
            {
                _endingTime = (float)gameTime.TotalGameTime.TotalSeconds;
                if (_endingTime > 3)
                {
                    _advance = true;
                }
            }


        }
        public bool AdvanceLevel
        {
            get { return _advance; }
            set { _advance = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}
