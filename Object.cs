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
    class Object : Animation
    {
        #region Private Members
        //private members
        private Vector2 trajectory;         // track speed and direction of travel
        private Vector2 force;              // track magnetude and direction of forward force
        private Vector2 strafe;             // track magnetude and direction of side force
        private float maxVelocity;          // limit total accumulated velocity
        private float turnRate;             // limit the rate at which the ship can turn
        private float accelerationRate;     // set the rate at which the opject picks up speed
        private float delimiter;            // used to calculate the rate at which force can be applyed to the object
        //Collision members
        private Rectangle hitBox;           // hitbox for collision
        private double distance;
        private bool collisionFlag;         // shows collision
        #endregion

        #region Public
        //Access specifiers
        public Vector2 Trajetory { get { return trajectory; } set { trajectory = value; } }
        public Vector2 Force { get { return force; } set { force = value; } }
        public Vector2 Strafe { get { return strafe; } set { strafe = value; } }
        public float TurnRate { get { return turnRate; } }
        public float AccelerationRate { get { return accelerationRate; } }
        public Rectangle HitBox { get { return hitBox; } }
        public bool CollisionFlag { get { return collisionFlag; } }
        public float MaxVelocety { set { maxVelocity = value; } }
        //PixelPerfect
        public Matrix Transform { get; private set; }
        public Color[] TextureData { get; private set; }
        #endregion

        //TOR Super-over-loaded beyond recognition
        public Object(ContentManager content, string file, Vector2 t_position, Vector2 t_numFrames, float t_turnRate, int t_maxVelocity)
            : base(content, file, t_position, t_numFrames)
        {
            turnRate = t_turnRate;
            maxVelocity = t_maxVelocity;
            trajectory = new Vector2(0, 0);
            force = new Vector2(0, 0);
            strafe = new Vector2(0, 0);
            accelerationRate = 10.0f;
            delimiter = 50;
            Pivot = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Layer = .7f;

            //Used for Pixel Collision
            TextureData = new Color[Texture.Width * Texture.Height];
            Texture.GetData(TextureData);
            CalculateMatrix();
        }

        #region PixelCollsion Data
        private void CalculateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Pivot, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(1f) *
                Matrix.CreateTranslation(new Vector3(Position, 0));
        }
        public bool CheckPerPixelCollision(Object b)
        {
            /*
             * We need to make the transform matrix that goes from A's transform
             * to B's transform, because they most likely have different effects
             * like scale, rotation, position, and more.
             */
            Matrix atob = this.Transform * Matrix.Invert(b.Transform);

            /*
             * Our main loop checks every single column and row of A's texture.
             * We need to duplicate this for B.  However, since B has a different 
             * transform, we need to transform a normal from A to B in order
             * to do the proper checking in B's world
             */
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, atob);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, atob);

            /*
             * 0x0 in A will be different in B since we have different
             * transform matrices.  We need to calculate the correct one
             * for B's start by using the atob transform we created.
             */
            Vector2 iBPos = Vector2.Transform(Vector2.Zero, atob);

            //Start from 0x0 on A, and the similar coordinates in B
            for (int deltax = 0; deltax < this.Texture.Width; deltax++)
            {
                Vector2 bpos = iBPos;
                for (int deltay = 0; deltay < this.Texture.Height; deltay++)
                {
                    int bx = (int)bpos.X;
                    int by = (int)bpos.Y;

                    /*
                     * The values need to be within the texture dimensions.
                     * Otherwise, the program will throw an ArrayIndexOutOfBounds
                     * exception
                     */
                    if (bx >= 0 && bx < b.Texture.Width && by >= 0 &&
                        by < b.Texture.Height)
                    {
                        //CHANGE THE '> 150' TO '!= 0' WHEN YOU USE THIS CODE
                        //IF YOU HAVE FULLY TRANSPARENT PIXELS!
                        if (this.TextureData[deltax + deltay * this.Texture.Width].A > 150
                            && b.TextureData[bx + by * b.Texture.Width].A > 150)
                            return true;
                    }

                    /*
                     * We are looping through every single pixel in column deltax
                     * for the A texture.  We need to increment the same thing 
                     * for B's texture.
                     */
                    bpos += stepY;
                }
                iBPos += stepX;
            }

            return false;
        }
        #endregion

        #region BoxCollision Data
        // Register the hit box
        public void RegisterHitBox()
        {
            //Transform the rectangle to the largest possible hitBox
            //that surrounds the object completely 
            //(For moving/rotating objects)
            hitBox = new Rectangle(0, 0, SourceRect.Width, SourceRect.Height);
            Vector2 leftTop = Vector2.Transform(new Vector2(hitBox.Left, hitBox.Top), Transform);
            Vector2 rightTop = Vector2.Transform(new Vector2(hitBox.Right, hitBox.Top), Transform);
            Vector2 leftBottom = Vector2.Transform(new Vector2(hitBox.Left, hitBox.Bottom), Transform);
            Vector2 rightBottom = Vector2.Transform(new Vector2(hitBox.Right, hitBox.Bottom), Transform);

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                              Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            hitBox = new Rectangle((int)Position.X, (int)Position.Y,
                (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        // Takes the object that it can collide with and returns itself
        public Object BoxCollision(Object obj)
        {
            if (hitBox.Intersects(obj.hitBox))
                collisionFlag = true;
            else
                collisionFlag = false;
            return this;

        }
        #endregion

        public Object DistanceCollision(Object obj)
        {
            distance = Math.Sqrt(Math.Pow(this.Position.X - obj.Position.X, 2) + Math.Pow(this.Position.Y - obj.Position.Y, 2));
            if (distance < this.SourceRect.Width / 2 + obj.SourceRect.Width / 2)
                collisionFlag = true;
            else
                collisionFlag = false;
            return this;

        }

        #region Physics Data
        //Apply the velocety and turn speed constraints so that the object does not get
        //out of control
        protected void ApplyConstraints()
        {
            //Set max force constraint
            if (force.Y >= maxVelocity / delimiter)
                force.Y = maxVelocity / delimiter;
            else if (force.Y <= -maxVelocity / delimiter)
                force.Y = -maxVelocity / delimiter;

            //Set max Strafe constrint
            if (strafe.Y >= maxVelocity / delimiter)
                strafe.Y = maxVelocity / delimiter;
            else if (strafe.Y <= -maxVelocity / delimiter)
                strafe.Y = -maxVelocity / delimiter;

            //Set max trajectory constraint
            if (trajectory.Y >= maxVelocity)
                trajectory.Y = maxVelocity;
            if (trajectory.Y <= -maxVelocity)
                trajectory.Y = -maxVelocity;

            //keep rotation between 0 & 2Pi
            if (trajectory.X > 2 * Math.PI)
                trajectory.X = trajectory.X - (2 * (float)Math.PI);
            else if (trajectory.X < 0)
                trajectory.X = trajectory.X + (2 * (float)Math.PI);
            if (force.X > 2 * Math.PI)
                force.X = force.X - (2 * (float)Math.PI);
            else if (force.X < 0)
                force.X = force.X + (2 * (float)Math.PI);
        }


        //recalculate the position, add two physics vectors together, only used in Update()
        protected void CalculatePosition(Viewport t_viewPort, Background backGround)
        {
            //Positional values conatining polar (x, y);
            Vector2 t_start, t_end;
            double pos_x, pos_y;

            //Save the starting location
            t_start = Position;

            //Apply force to the existing trajectory
            pos_x = (Math.Cos(force.X) * force.Y) + (Math.Cos(strafe.X) * strafe.Y) + Position.X;
            pos_y = (Math.Sin(force.X) * force.Y) + (Math.Sin(strafe.X) * strafe.Y) + Position.Y;

            //if the ship is moving
            if (trajectory.Y != 0)
            {
                //Continue to add velocety to the tragectory 
                pos_x += (Math.Cos(trajectory.X) * trajectory.Y);
                pos_y += (Math.Sin(trajectory.X) * trajectory.Y);
            }

            //Update the position at which to draw the sprite
            Position = new Vector2((float)pos_x, (float)pos_y);

            //subtract the starting position form the ending position so that the result
            //can be used to calculate the new angle of tragectory
            t_end = Position - t_start;

            //When calculating pythageruims therum the result is always positive therefor
            //the values must be check prior to the calculation to determine if the object 
            //is moving in a positive direction or negative and the +/- signs get manually 
            //applyed to the tragectory. 

            //Check that the velocety is positive
            if (t_end.X == Math.Abs(t_end.X))
                //since the direction of travel is positive leave pythageriums positive
                trajectory.Y = (float)Math.Sqrt(Math.Pow(t_end.X, 2) + Math.Pow(t_end.Y, 2));
            //if for negative velocety
            else if (t_end.X == -Math.Abs(t_end.X))
                //since the direction of travel is negative manualy apply a negative to pythageriums
                trajectory.Y = -(float)Math.Sqrt(Math.Pow(t_end.X, 2) + Math.Pow(t_end.Y, 2));
            //finally calculate the new angle of travel using the tangent of the differance of 
            //the starting position and the ending position
            trajectory.X = (float)Math.Atan(t_end.Y / t_end.X);

            //This section is used to loop the object on the screen if said object
            //is to leave the screen redraw it on the opposite side that it left from


            //Bottom of the screen
            if (Position.Y > backGround.Texture.Height * 10)
                Position = new Vector2(Position.X, -backGround.Texture.Height * 10);
            //Top of the screen
            else if (Position.Y < -backGround.Texture.Height * 10)
                Position = new Vector2(Position.X, backGround.Texture.Height * 10);
            //Right side of the screen
            else if (Position.X > backGround.Texture.Width * 15)
                Position = new Vector2(-backGround.Texture.Width * 15, Position.Y);
            //Left side of the screen
            else if (Position.X < -backGround.Texture.Width * 15)
                Position = new Vector2(backGround.Texture.Width * 15, Position.Y);
        }
        #endregion

        //central processing for the entire class, pass 0 to t_switchFrame if Object does not animate
        public void Update(GameTime t_time, Viewport t_viewPort, int t_switchFrame, Background backGround)
        {
            CalculatePosition(t_viewPort, backGround);
            ApplyConstraints();
            RegisterHitBox();
            CalculateMatrix();


            //if the object needs to animate on a timer do not pass 0 to t_switchFrame
            if (t_switchFrame != 0)
                AnimateSprite(t_time, t_switchFrame);

            Rotation = force.X;

        }
    }
}
