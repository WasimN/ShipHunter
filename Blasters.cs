using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShipHunter
{
    class Blasters : Object
    {
        private bool visible;
        private float firingRate;
        private float firingTimer;

        public bool Visible { get { return visible; } set { visible = value; } }
        public float FiringRate { get { return firingRate; } }
        public float FiringTimer { get { return firingTimer; } set { firingTimer = value; } }

        public Blasters(ContentManager content, Vector2 t_position)
            : base(content, "Art/blasters", t_position, new Vector2(2, 1), 0, 100)
        {
            visible = false;
            firingRate = 100;
        }

        public void Fire(Vector2 start, Vector2 direction, GameTime gameTime)
        {
            firingTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (firingTimer > firingRate)
            {
                visible = true;
                Position = start;
                Trajetory = new Vector2(direction.X, 50);
                Force = new Vector2(direction.X, 0);

            }
        }

        public override void Draw(SpriteBatch t_batch)
        {
            if (visible)
            {
                t_batch.Draw(Texture, Position, SourceRect, Tint, Rotation, Pivot, Scale, SpriteEffects.None, Layer);
            }
        }
    }
}