using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects;
using System.Maths;
using System.Rendering;
using System.Rendering.Forms;
using System.Compilers.Shaders;

namespace Testing.Common
{
    public class ShadowMapScene : Scene
    {
        TextureBuffer shadowMap;
        TextureBuffer image;

        IModel teapot, plane, cube;

        public override void Initialize(System.Rendering.Forms.IControlRenderDevice render)
        {
            shadowMap = TextureBuffer.Empty<CustomPixels.ScaledRed>(1024, 1024);
            shadowMap = shadowMap.Allocate(render);

            image = render.Services.LoadTexture("Resources/Tulips_Small.jpg").Allocate(render);

            teapot = Models.Teapot.Allocate(render);
            plane = Models.PlaneXZ.Allocate(render);
            cube = Models.Cube.Allocate(render);
        }

        void DrawScene(Matrix4x4 view, Matrix4x4 projection, bool shadowMap, IControlRenderDevice render)
        {
            render.Draw(() =>
            {
                render.Draw(teapot
                        , Materials.White.Shinness.Shinness.Glossy
                        , shadowMap ? Effect.None : Shading.DiffuseMap(image)
                        );
                render.Draw(teapot, Transforming.Translate(-2, 0, 0)
                        , Materials.White.Shinness.Shinness.Glossy
                    //, shadowMap ? Effect.None : Shading.DiffuseMap(image)
                        );
                render.Draw(teapot, Transforming.Translate(2, 0, 0)
                        , Materials.White.Shinness.Shinness.Glossy
                    //, shadowMap ? Effect.None : Shading.DiffuseMap(image)
                        );

                render.Draw(teapot, Transforming.Translate(0, 0, -2)
                        , Materials.White.Shinness.Shinness.Glossy
                    //, shadowMap ? Effect.None : Shading.DiffuseMap(image)
                        );
                render.Draw(teapot, Transforming.Translate(0, 0, 2)
                        , Materials.White.Shinness.Shinness.Glossy
                    //, shadowMap ? Effect.None : Shading.DiffuseMap(image)
                        );
                render.Draw(plane,
                    Transforming.Translate (0, -1f,0)
                        //, shadowMap ? Effect.None : Materials.White
                        );
            },
                shadowMap ? Effect.None : Lighting.PointLight(LightPosition, new Vector3(1, 1, 1)),
                (Viewing)view,
                (Projecting)projection,
                Buffering.Clear(shadowMap ? new Vector4(1,1,1,1) : new Vector4(0.2f, 0.2f, 0.2f, 0.2f)),
                Buffering.ClearDepth());
        }

        Vector3 LightPosition = new Vector3(4, 4, 4);

        public override void Draw(System.Rendering.Forms.IControlRenderDevice render)
        {
            ITextureRenderDevice textureRender = render as ITextureRenderDevice;

            LightPosition = (Vector3)GMath.mul(new Vector4(3, 3, 3, 1), Matrices.RotateY(Environment.TickCount / 1000f));

            Matrix4x4 camera = Matrices.LookAtLH(new Vector3(-4, 5, 6), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            Matrix4x4 projection = Matrices.PerspectiveFovLH(GMath.PiOver4, render.ImageHeight / (float)render.ImageWidth, 1, 100);
            Matrix4x4 shadowMapView = Matrices.LookAtLH(LightPosition, new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            Matrix4x4 shadowMapProjection = Matrices.PerspectiveFovLH(GMath.Pi/3f, 1f, 0.1f, 100);

            Matrix4x4 cameraInverse = camera.Inverse;

            render.BeginScene();

            textureRender.BeginScene(shadowMap);

            textureRender.Draw(() => DrawScene(shadowMapView, shadowMapProjection, true, render),
                new PixelShaderEffect<PositionData, ColorData>(In =>
                {
                    var p = GMath.mul(new Vector4(In.Position, 1), shadowMapProjection);
                    return new ColorData { Color = new Vector4(p.Z / p.W, 0, 0, 1) };
                }) { AppendMode = AppendMode.Append },
                Shading.VertexTransform, Shading.None);

            textureRender.EndScene();

            Sampler2D shadowMapSampler = new Sampler2D(shadowMap) { Addresses = new Addressing2D { U = TextureAddress.Border, V = TextureAddress.Border }, Filters = new Filtering { MipMapping = TextureFilterType.Linear, Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear } };

            render.Draw(() => DrawScene(camera, projection, false, render), //Shading.DiffuseMap (shadowMap),
                new PixelShaderEffect<SurfaceData, ColorsData>(In =>
                {
                    var P = GMath.mul(new Vector4(In.Position, 1), GMath.mul(cameraInverse, shadowMapView));
                    var L = GMath.mul(P, shadowMapProjection);
                    
                    var coordinates = new Vector2(L.X / L.W * 0.5f + 0.5f, 0.5f - 0.5f * L.Y / L.W);
                    var LitPos = shadowMapSampler.Sample(coordinates).X;

                    var distance = L.Z / L.W - LitPos;

                    float shadowFactor = 0;
                    if (distance <= 0.0001f)
                        shadowFactor = 1;

                    return new ColorsData
                    {
                        Diffuse = new Vector4(shadowFactor, shadowFactor, shadowFactor, 1) * In.Diffuse,
                        Specular = new Vector3  (1,1,1) * In.Specular
                    };
                }) { AppendMode = AppendMode.Prepend },
                Shading.Phong
                );

            render.EndScene();
        }
    }
}
