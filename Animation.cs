using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShipHunter
{
    class Animation : Sprite
    {
        //private members
        private Vector2 currentFrame;   //position of current frame in spritesheet
        private Vector2 numFrames;      //the number of (rows, columns)
        private int frameCounter;       //tracks time for animation
        private int switchFrame;        //time in between frames

        //Get/Set for privates
        public Vector2 CurrentFrame { get { return currentFrame; } set { currentFrame = value; } }
        public Vector2 NumFrames { get { return numFrames; } set { currentFrame = value; } }
        public int SwitchFrame { get { return switchFrame; } set { switchFrame = value; } }
        public int FrameWidth { get { return (Texture.Width / (int)numFrames.X); } }
        public int FrameHeight { get { return (Texture.Height / (int)numFrames.Y); } }


        //Overload TOR and call base TOR of Sprite
        public Animation(ContentManager content, string file, Vector2 t_position, Vector2 t_numFrames) 
            : base(content, file, t_position)
        {
            currentFrame = new Vector2(0, 0);
            numFrames = t_numFrames;
            frameCounter = 0;
        }

        public void UpdatePosition(GameTime t_time, Viewport t_viewPort)
        {
            Position = new Vector2(Position.X + t_time.ElapsedGameTime.Milliseconds, Position.Y + t_time.ElapsedGameTime.Milliseconds);

            if (Position.Y > Texture.Height + t_viewPort.Height)
                Position = new Vector2(Position.X, 0);
            //Top of the screen
            else if (Position.Y < (0 - Texture.Height))
                Position = new Vector2(Position.X, Texture.Height + t_viewPort.Height);
            //Right side of the screen
            else if (Position.X > Texture.Width + t_viewPort.Width)
                Position = new Vector2(0, Position.Y);
            //Left side of the screen
            else if (Position.X < 0)
                Position = new Vector2(Texture.Width + t_viewPort.Width, Position.Y);
        }

        //Runs through the sprite, passing 0 to t_switch does not animate
        public void AnimateSprite(GameTime t_timer, int t_switch)
        {

            switchFrame = t_switch;
            frameCounter += t_timer.ElapsedGameTime.Milliseconds;
            if (frameCounter >= switchFrame && SwitchFrame != 0)
            {
                frameCounter = 0;
                currentFrame.X += FrameWidth;
                if (currentFrame.X >= Texture.Width)
                {
                    currentFrame.X = 0;
                    currentFrame.Y += FrameHeight;
                    if (currentFrame.Y >= Texture.Height - FrameHeight)
                    {
                        currentFrame.Y = 0;
                        currentFrame.X = 0;
                    }
                }
            }
            SourceRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y, FrameWidth, FrameHeight);
            Pivot = new Vector2(FrameWidth / 2, FrameHeight / 2);
        }
    }
}
