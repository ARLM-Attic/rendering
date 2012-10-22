using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maths
{
    public sealed class Ray
    {
        public static Ray LookingTo(Vector3 point)
        {
            return Ray.FromPosDir(point, Vectors.Front);
        }

        public static Ray LookingToScreen(int x, int y, int screenWidth, int screenHeight, Matrix4x4 projection, Matrix4x4 view)
        {
            Vector3 beforeViewport = new Vector3(
                2 * x / (float)screenWidth - 1,
                1 - 2 * y / (float)screenHeight, 0);

            return LookingToScreen(beforeViewport, projection, view);
        }

        public static Ray LookingToScreen(Vector3 point, Matrix4x4 projection, Matrix4x4 view)
        {
            if (projection.IsSingular || view.IsSingular) return null;

            Matrix4x4 invP = projection.Inverse;

            Matrix4x4 invV = view.Inverse;

            Vector4 to_ = new Vector4(point.X, point.Y, 1, 1);
            Vector4 from_ = new Vector4(point.X, point.Y, -1, 1);

            Vector4 screenOn3D = GMath.mul (GMath.mul (to_, invP) , invV);
            Vector4 cameraOn3D = GMath.mul (GMath.mul (from_ , invP) , invV);
            return Ray.FromPosTo(new Vector3(screenOn3D.X/screenOn3D.W, screenOn3D.Y/screenOn3D.W, screenOn3D.Z/screenOn3D.W),
                new Vector3(cameraOn3D.X/cameraOn3D.W, cameraOn3D.Y/cameraOn3D.W, cameraOn3D.Z/cameraOn3D.W));
        }
        
        public readonly Vector3 Position, Direction;
        
        public static Ray FromPosDir(Vector3 position, Vector3 direction)
        {
            return new Ray(position, direction);
        }
        public static Ray FromPosTo(Vector3 position, Vector3 destination)
        {
            return new Ray(position, GMath.normalize(destination - position));
        }
        public Ray(Vector3 position, Vector3 direction)
        {
            this.Position = position;
            this.Direction = direction;
        }

        public Ray Transform(Matrix4x4 app)
        {
            Vector4 pos = GMath.mul (new Vector4(Position, 1), app);
            Vector4 to = GMath.mul (new Vector4((Position + Direction), 1) , app);

            pos *= 1 / pos.W;
            to *= 1 / to.W;

            return Ray.FromPosTo(new Vector3(pos.X, pos.Y, pos.Z), new Vector3(to.X, to.Y, to.Z));
        }
    }
}
