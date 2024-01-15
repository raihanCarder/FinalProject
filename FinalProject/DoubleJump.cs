﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class DoubleJump
    {
        List<Texture2D> cloudTextures;
        private Texture2D _texture;
        private Rectangle _location;
        private Color _color;
        private Vector2 _position;

        public DoubleJump(List<Texture2D> textures, Vector2 position, int size) // Actual Player Constructor
        {
            cloudTextures = textures;

            _texture = textures[0];
            _color = Color.White;
            _position = position;
            _location = new Rectangle((int)_position.X, (int)_position.Y, size+10, size);

        }

        public void Update(GameTime gameTime, Player stickman)
        {
            if (_location.Intersects(stickman.CollisonRectangle))
            {
                stickman.isJumping = false;
            }        
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, _color);
        }
    }
}
