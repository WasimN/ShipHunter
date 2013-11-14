using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ShipHunter
{
    class Sprite
    {
        // private members
        private Texture2D texture;      // stores the image
        private Rectangle sourceRect;   // used for sprite sheets
        private Vector2 position;       // position to draw on screen
        private Color tint;             // leave white to prevent tinting
        private float rotation;         // rotation in radians
        private Vector2 pivot;          // point around which the sprite rotates
        private Single scale;           // scale 
        private Single layer;           // layering used in 3D
        

        // Get/Set for private members
        public Vector2 Position { get { return position; } set { position = value; } }
        public float Rotation { get { return rotation; } set { rotation = value; } }
        public Vector2 Pivot { get { return pivot; } set { pivot = value; } }
        public Single Scale { get { return scale; } set { scale = value; } }
        public Rectangle SourceRect { get { return sourceRect; } set { sourceRect = value; } }
        public Texture2D Texture { get { return texture; } }
        public Color Tint { get { return tint; } set { tint = value; } }
        public Single Layer { get { return layer; } set { layer = value; } }
        

        // Overload TOR
        public Sprite(ContentManager content, string file, Vector2 t_position)
        {
            texture = content.Load<Texture2D>(file);
            position = t_position;
            tint = Color.White;
            rotation = 0;
            scale = 1;
            layer = 1;
            SetRect(texture);
            pivot = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        // Load sourceRect with the dimentions of the texture
        private void SetRect(Texture2D t_image)
        {
            sourceRect.X = 0;
            sourceRect.Y = 0;
            sourceRect.Width = t_image.Width;
            sourceRect.Height = t_image.Height;
        }

        

        // Draw the Sprite
        public virtual void Draw(SpriteBatch t_batch)
        {
            // Position is the position on the screen the object will be draw
            // the Pivot not only is the point around which the object rotates it is also 
            // the 0,0 position of the object.  In order to draw a rect around the sprite
            // subtract the pivot from the position, thus giving you the upper left hand 
            // corner of the sprite. Then simply make the width / height equal to the sorceRect

            t_batch.Draw(texture, position, sourceRect, tint, rotation, pivot,
                scale, SpriteEffects.None, layer);
        }


    }
}
