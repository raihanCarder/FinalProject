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
        private Rectangle _location;
        private int _endingDistance;
        private int _startingDistance;
        private float _speed;
        private bool _horizontalDirection;

        public SpinningBlade(List<Texture2D> bladeTextures, int x, int y, int endingPoint, float speed, int size, bool horizontalDirection) // Default Spinning Blade
        {
            _spinningBladeTextures = bladeTextures;
            _endingDistance = endingPoint;
            _location = new Rectangle(x, y, size, size);
            _speed = speed;
            _horizontalDirection = horizontalDirection;
        }

    }
}
