using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public struct Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }


        public Rectangle DrawRect
        {
            get
            {
                return new Rectangle((int)(Center.X - Radius), (int)(Center.Y - Radius), (int)Radius * 2, (int)Radius * 2);

            }
        }
    }
}
