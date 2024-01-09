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

        //public Circle(int x, int y, float radius)
        //    : this()
        //{
        //    Center = center;
        //    Center.X = x;
        //    Radius = radius;
        //}


        public bool Intersects(Rectangle rectangle)
        {
            int X = (int)Center.X;
            int Y = (int)Center.Y;
            // the first thing we want to know is if any of the corners intersect
            var corners = new[]
            {
            new Point(rectangle.Left, rectangle.Top),
            new Point(rectangle.Right, rectangle.Top),
            new Point(rectangle.Right, rectangle.Bottom),
            new Point(rectangle.Left, rectangle.Bottom)
            };

            foreach (var corner in corners)
            {
                if (ContainsPoint(corner))
                    return true;
            }

            // next we want to know if the left, top, right or bottom edges overlap
            if (X - Radius > rectangle.Right || X + Radius < rectangle.Left)
                return false;

            if (Y - Radius > rectangle.Bottom || Y + Radius < rectangle.Top)
                return false;

            return true;
        }

        private bool ContainsPoint(Point corner)
        {
            throw new NotImplementedException();
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
