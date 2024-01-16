using Microsoft.Xna.Framework;
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
        private int _framecounter;
        private Vector2 _position;

        public EndLevelDoor(List<Texture2D> textures, Vector2 position, int size)
        {
          _doorTextures = textures;
          _texture = textures[0];
          _position = position;
          _location = new Rectangle((int)position.X, (int)position.Y, size, size);
          _collisionRectangle = new Rectangle((int)position.X, (int)position.Y, size/2, size);
          _framecounter = 0;
        }

        public void Update(GameTime gameTime, Player stickman)
        {


        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}
