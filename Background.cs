using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace ShipHunter
{
    class Background : Sprite
    {
        // Private members
        private Vector2 size;

        // System objects
        protected GraphicsDeviceManager graphics;

        // Accesser's
        public Vector2 Size { get { return size; } set { size = value; } }

        public Background(ContentManager content, GraphicsDeviceManager t_graphics, string file)
            : base(content, file, new Vector2(0, 0))
        {
            graphics = t_graphics;
            Layer = 0;
            size = new Vector2(Texture.Width * 20, Texture.Height * 20);
        }

        public void BGDraw(SpriteBatch t_batch)
        {
            for (int i = -Texture.Width * 20; i < Texture.Width * 20; )
            {
                for (int j = -Texture.Height * 15; j < Texture.Height * 15; )
                {
                    t_batch.Draw(Texture, new Vector2(i, j), SourceRect, Tint, Rotation, Pivot,
                        Scale, SpriteEffects.None, 0);
                    j += Texture.Height;
                }
                i += Texture.Width;
            }
        }

    }
}
