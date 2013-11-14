using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ShipHunter
{
    public class Debug
    {
        private SpriteBatch batch;
        private List<Vector2> position;
        private List<string> output;
        private SpriteFont font;

        public List<Vector2> Position { get { return position; } }

        public Debug(ContentManager content, SpriteBatch t_batch, float t_posx, float t_posy)
        {
            batch = t_batch;
            position = new List<Vector2>();
            font = content.Load<SpriteFont>("Debug");
            output = new List<string>();
        }

        public void PushBack(string text, float x, float y)
        {
            output.Add(text);
            position.Add(new Vector2(x, y));
        }

        public void Draw()
        {
            for (int i = 0; i < output.Count; i++)
            {
                //batch.DrawString(font, (i+1) + ": " + output[i], position[i], Color.LimeGreen);
                batch.DrawString(font, (i + 1) + ": " + output[i], position[i], Color.LimeGreen, 0f, Vector2.Zero, 1, SpriteEffects.None, .99f);
            }

            output.Clear();
            position.Clear();
        }
    }
}
