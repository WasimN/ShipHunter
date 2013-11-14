using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShipHunter
{
    class Camera
    {
        // Member veriables
        protected float zoom;       // Camera Zoom
        private Matrix tranform;     // Matrix Tranform
        private Vector2 position;    // Camera position
        protected float rotation;   // Maera rotation

        // Constructor
        public Camera()
        {
            zoom = 1.0f;
            rotation = 0.0f;
            position = Vector2.Zero;
        }

        // Accesser's
        public float Zoom { get { return zoom; } set { zoom = value; 
            if (zoom < 0.1f) zoom = 0.1f; if (zoom > 2.0f) zoom = 2f;} }
        public float Roatation { get { return rotation; } set { rotation = value; } }
        public void Move(Vector2 amount) { position += amount; }
        public Vector2 Position { get { return position; } set { position = value; } }

        // Big Bertha
            // I'll give you a dollar if you tell what happened
        public Matrix GetTranformation(GraphicsDevice graphicsDevice, Viewport viewPort)
        {
            tranform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0))
                * Matrix.CreateRotationZ(Roatation) * Matrix.CreateScale(new Vector3(zoom, zoom, 1))
                * Matrix.CreateTranslation(new Vector3(viewPort.Width * 0.5f, viewPort.Height * 0.5f, 0));
            return tranform;
        }

    }
}
