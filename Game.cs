using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurtainFireMaker;
using CurtainFireMaker.Renderer;
using OpenTK;
using System.Drawing;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;

namespace CurtainFireMaker
{
    public class Game : GameWindow
    {
        public WorldRenderer Renderer { get; }

        public World World { get; }

        public Game() : base(960, 540, new GraphicsMode(32, 24, 0, 8), "CurtainFireMaker", GameWindowFlags.FixedWindow)
        {
            World = new World(this);
            Renderer = new WorldRenderer(World, ClientSize);

            Renderer.Camera.Pos = Vector3.UnitZ * 20;

            var type = new Entities.ShotTypeWavefront("", @"C:\Tool\CG\MikuMikuMoving64_v1282\CurtainFireMaker\Resource\Wavefront\ico.obj");

            float pi = (float)Math.PI;

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    var shot = new Entities.EntityShot(World, type)
                    {
                        Velocity = VecMath.Vector3.UnitY * VecMath.Matrix3.RotationX(pi / 6 * j) * VecMath.Matrix3.RotationY(pi / 6 * i) * 0.1F,
                    };
                    Console.WriteLine(shot.Velocity);
                    shot.Spawn();
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            World.Init();
            World.Frame();

            Renderer.Init();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            World.Frame();
            Renderer.Render();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnDisposed(EventArgs e)
        {
            //  TextureGenerater.Clear();

            base.OnDisposed(e);
        }
    }
}
