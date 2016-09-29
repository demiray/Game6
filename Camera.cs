using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _3D_Game
{

    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        //Camera matrices
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        // Camera vectors
        public Vector3 cameraPosition { get; protected set; }
        Vector3 cameraDirection;
        Vector3 cameraUp;

        //float speed = 3;

        MouseState prevMouseState;

        // Max yaw/pitch variables
        float totalYaw = MathHelper.PiOver4 / 2;
        float currentYaw = 0;
        float totalPitch = MathHelper.PiOver4 / 2;
        float currentPitch = 0;

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Build camera view matrix
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();

            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height,
                1, 3000);
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

            // Set mouse position and do initial get state
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2,
            Game.Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            //// Move forward/backward
            //if (Keyboard.GetState().IsKeyDown(Keys.W))
            //    cameraPosition += cameraDirection * speed;
            //if (Keyboard.GetState().IsKeyDown(Keys.S))
            //    cameraPosition -= cameraDirection * speed;

            //// Move side to side
            //if (Keyboard.GetState().IsKeyDown(Keys.A))
            //    cameraPosition += Vector3.Cross(cameraUp, cameraDirection) * speed;
            //if (Keyboard.GetState().IsKeyDown(Keys.D))
            //    cameraPosition -= Vector3.Cross(cameraUp, cameraDirection) * speed;


            //// Yaw rotation
            //cameraDirection = Vector3.Transform(cameraDirection,
            //Matrix.CreateFromAxisAngle(cameraUp, (-MathHelper.PiOver4 / 150) *
            //(Mouse.GetState().X - prevMouseState.X)));

            // Yaw rotation
            float yawAngle = (-MathHelper.PiOver4 / 150) *
            (Mouse.GetState().X - prevMouseState.X);
            if (Math.Abs(currentYaw + yawAngle) < totalYaw)
            {
                cameraDirection = Vector3.Transform(cameraDirection,
                Matrix.CreateFromAxisAngle(cameraUp, yawAngle));
                currentYaw += yawAngle;
            }

            //// Roll rotation
            //if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            //{
            //    cameraUp = Vector3.Transform(cameraUp,
            //    Matrix.CreateFromAxisAngle(cameraDirection,
            //    MathHelper.PiOver4 / 45));
            //}
            //if (Mouse.GetState().RightButton == ButtonState.Pressed)
            //{
            //    cameraUp = Vector3.Transform(cameraUp,
            //    Matrix.CreateFromAxisAngle(cameraDirection, -MathHelper.PiOver4 / 45));
            //}

            //// Pitch rotation
            //cameraDirection = Vector3.Transform(cameraDirection,
            //Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection),
            //(MathHelper.PiOver4 / 100) *
            //(Mouse.GetState().Y - prevMouseState.Y)));

            //cameraUp = Vector3.Transform(cameraUp,
            //Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection),
            //(MathHelper.PiOver4 / 100) *
            //(Mouse.GetState().Y - prevMouseState.Y)));

            // Pitch rotation
            float pitchAngle = (MathHelper.PiOver4 / 150) *
            (Mouse.GetState().Y - prevMouseState.Y);
            if (Math.Abs(currentPitch + pitchAngle) < totalPitch)
            {
                cameraDirection = Vector3.Transform(cameraDirection,
                Matrix.CreateFromAxisAngle(
                Vector3.Cross(cameraUp, cameraDirection),
                pitchAngle));
                currentPitch += pitchAngle;
            }

            // Reset prevMouseState
            prevMouseState = Mouse.GetState();

            // Recreate the camera view matrix
            CreateLookAt();

            base.Update(gameTime);
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition,
            cameraPosition + cameraDirection, cameraUp);
        }
    }
}