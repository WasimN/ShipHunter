using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ShipHunter
{
    class Asteroid : Object
    {

        private static Random seed = new Random();


        public Asteroid(ContentManager content)
            : base (content, "Art/asteroid", Vector2.Zero, new Vector2(8,4), 0, 15 )
        {
            Trajetory = new Vector2(seed.Next(100), seed.Next(15));
            Position = new Vector2(seed.Next(-10000,10000), seed.Next(-10000,10000));
            Scale = seed.Next(100, 500) / 120;

        }
    }
}
