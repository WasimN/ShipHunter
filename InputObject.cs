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
    class InputObject : Object
    {
        public InputObject(ContentManager content, string file, Vector2 t_position, Vector2 t_numFrames, float t_turnRate, int t_maxVelocity)
            : base(content, file, t_position, t_numFrames, t_turnRate, t_maxVelocity)
        {
            Layer = .9f;
        }

        // Proccess keyboard input
        private void ProccessInput(KeyboardState t_keyState, GameTime t_time, Viewport t_viewPort, GameMouse mouse, Camera camera)
        {
            // Protected KeyBoard Input Code
            ///////////////////////////////////////////////////////////////
            // Roation  Q / E
            if (t_keyState.IsKeyDown(Keys.Q))
                Force = new Vector2(Force.X - (TurnRate *
                    (float)t_time.ElapsedGameTime.TotalSeconds), Force.Y);
            if (t_keyState.IsKeyDown(Keys.E))
                Force = new Vector2(Force.X + (TurnRate *
                    (float)t_time.ElapsedGameTime.TotalSeconds), Force.Y);
            ////////////////////////////////////////////////////////////////

            // mouse input code
            ////////////////////////////////////////////////////////////////
            Vector2 direction = new Vector2();
            direction = mouse.Position - Position;
            Force = new Vector2((float)Math.Atan2(direction.Y,direction.X), Force.Y);
            ///////////////////////////////////////////////////////////////

            // Strafe A / D
            if (t_keyState.IsKeyDown(Keys.A))
            {
                Strafe = new Vector2((float)(Force.X + Math.PI / 2),
                    Strafe.Y - AccelerationRate * (float)t_time.ElapsedGameTime.TotalSeconds);
            }
            else if (t_keyState.IsKeyDown(Keys.D))
            {
                Strafe = new Vector2((float)(Force.X + Math.PI / 2),
                    Strafe.Y + AccelerationRate * (float)t_time.ElapsedGameTime.TotalSeconds);
            }
            else
                Strafe = new Vector2(Strafe.X, 0);

            // Accelerate forward W
            if (t_keyState.IsKeyDown(Keys.W))
                Force = new Vector2(Force.X,
                    Force.Y + AccelerationRate * (float)t_time.ElapsedGameTime.TotalSeconds);
            else
                Force = new Vector2(Force.X, 0);

            // Brake S
            if (t_keyState.IsKeyDown(Keys.S))
                Trajetory = new Vector2(Trajetory.X,
                     Trajetory.Y - (float)(1.90f * Trajetory.Y * t_time.ElapsedGameTime.TotalSeconds));
        }

        public void UpdateInput(KeyboardState t_keyState, GameTime t_time, Viewport t_viewPort, int t_switchFrame,
            Background backGround, GameMouse mouse, Camera camera)
        {
            ProccessInput(t_keyState, t_time, t_viewPort, mouse, camera);
            //CalculatePosition(t_viewPort, backGround);
            //ApplyConstraints();
            //RegisterHitBox();
            this.Update(t_time, t_viewPort, t_switchFrame, backGround);

            //if the object needs to animate on a timer do not pass 0 to t_switchFrame
            if (t_switchFrame != 0)
                AnimateSprite(t_time, t_switchFrame);

            Rotation = Force.X;
        }
    }
}
