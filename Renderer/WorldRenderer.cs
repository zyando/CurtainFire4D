using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;

namespace CurtainFireMaker.Renderer
{
    public class WorldRenderer
    {
        public World World { get; }

        public GameCamera Camera { get; }
        public Size Size { get; }

        public WorldRenderer(World world, Size size)
        {
            World = world;
            Size = size;

            Camera = new GameCamera(World);
            Camera.Spawn();
        }

        public void Init()
        {
            gl.ClearColor(Color.Black);

            gl.Enable(EnableCap.Texture2D);
            // gl.Enable(EnableCap.Lighting);
            // gl.Enable(EnableCap.Light0);

            gl.Enable(EnableCap.Multisample);
            gl.Enable(EnableCap.SampleAlphaToCoverage);
            gl.SampleCoverage(0.5F, false);

            gl.Enable(EnableCap.DepthTest);
            gl.DepthFunc(DepthFunction.Lequal);

            gl.Enable(EnableCap.Blend);
            gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            gl.Enable(EnableCap.PolygonOffsetFill);

            SetProjectionMatrix();
        }

        public void Render()
        {
            Camera.Update();

            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetModelViewMatrix();

            gl.ShadeModel(ShadingModel.Smooth);

            foreach (var entities in World.EntityList.ToLookup(e => e.Renderer))
            {
                entities.Key.Render(entities);
            }
        }

        private void SetProjectionMatrix()
        {
            gl.Viewport(0, 0, Size.Width, Size.Height);

            gl.MatrixMode(MatrixMode.Projection);
            Camera.LoadProjectionMatrix(Size.Width, Size.Height);
        }

        private void SetModelViewMatrix()
        {
            gl.MatrixMode(MatrixMode.Modelview);
            Camera.LoadModelViewMatrix();
        }
    }
}
