using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ShipHunter
{
    class GameMouse : Object
    {
        private MouseState mouse;

        public MouseState getMouse { get { return mouse; } set { mouse = value; } }

        public GameMouse(ContentManager content, string file, Vector2 t_numFrames)
            : base(content, file, Vector2.Zero, t_numFrames, 0, 0)
        {
            //SetRect();
            Pivot = new Vector2(SourceRect.Width/3, SourceRect.Height/1);
            Layer = 1;

        }

        public void Update(InputObject ship, Viewport viewPort, GameTime time, Camera camera)
        {
            RegisterHitBox();
            mouse = Mouse.GetState();
            
            Position = new Vector2(mouse.X + ship.Position.X - (viewPort.Width / 2), 
                mouse.Y + ship.Position.Y - (viewPort.Height / 2 ));
            
            Rotation += time.ElapsedGameTime.Milliseconds / 110f;
            AnimateSprite(time, 0);
            
        }


    }
}
