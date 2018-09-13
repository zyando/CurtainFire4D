using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using CurtainFireCore;
using CurtainFire4D.Renderer;
using OpenTK;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;
using AForge.Video.VFW;

namespace CurtainFire4D
{
    public class Game : GameWindow
    {
        public static string ConfigScriptPath => "config.py";

        public static Size RenderingSize => new Size(960, 540);

        public PythonExecutor Executor { get; } = new PythonExecutor();
        public WorldRenderer Renderer { get; }
        public World World { get; }
        private AVIWriter AVIWriter { get; }

        public string ScriptPath { get; }

        public dynamic ScriptDynamic { get; set; }

        public Game(string scriptPath) : base(RenderingSize.Width - 16, RenderingSize.Height - 39, new GraphicsMode(32, 24, 0, 8), "CurtainFireMaker", GameWindowFlags.FixedWindow)
        {
            World = new World(this);
            Renderer = new WorldRenderer(World, RenderingSize);
            AVIWriter = new AVIWriter()
            {
                Codec = "DIB ",
                FrameRate = 30,
            };
            AVIWriter.Open("export.avi", RenderingSize.Width, RenderingSize.Height);

            ScriptPath = scriptPath;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Executor.Init();
            World.Init();
            Renderer.Init();

            try
            {
                ScriptDynamic = Executor.Engine.ExecuteFile(ConfigScriptPath, Executor.RootScope);
                Entities.ShotType.Register(ScriptDynamic.init_shottype());

                Executor.SetGlobalVariable(("WORLD", World), ("STARTFRAME", 0), ("ENDFRAME", 1200));
                Executor.Engine.ExecuteFile(ScriptPath, Executor.CreateScope());
            }
            catch (Exception ex)
            {
                using (StreamWriter error_sw = new StreamWriter("error.log", false, Encoding.UTF8))
                {
                    try { error_sw.WriteLine(Executor.FormatException(ex)); } catch { }
                    error_sw.WriteLine(ex.ToString());
                }
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            World.Frame();
            Renderer.Render();
          //  CaptureScreen();

            SwapBuffers();
        }

        private void CaptureScreen()
        {
            var bmp = new Bitmap(RenderingSize.Width, RenderingSize.Height);
            var data = bmp.LockBits(new Rectangle(Point.Empty, RenderingSize), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            gl.ReadBuffer(ReadBufferMode.Front);
            gl.ReadPixels(0, 0, RenderingSize.Width, RenderingSize.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);
            AVIWriter.AddFrame(bmp);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnDisposed(EventArgs e)
        {
            //  TextureGenerater.Clear();

            AVIWriter.Close();

            base.OnDisposed(e);
        }
    }
}
