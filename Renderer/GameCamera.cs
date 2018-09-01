using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CurtainFireMaker.Entities;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;
using VecMath;

namespace CurtainFireMaker.Renderer
{
    public class GameCamera : Entity
    {
        public Vector3 Upward { get; set; } = Vector3.UnitY;

        public override IRenderer Renderer => NoneRenderer.Instance;

        public float Fovy = (float)Math.PI * 0.25F;
        public float ZNear = 0.05F;
        public float ZFar = 1024.0F;

        public GameCamera(World world) : base(world)
        {
        }

        public void LoadProjectionMatrix(int width, int height)
        {
            var matrix = OpenTK.Matrix4.CreatePerspectiveFieldOfView(Fovy, width / (float)height, ZNear, ZFar);
            gl.LoadMatrix(ref matrix);
        }

        public void LoadModelViewMatrix()
        {
            var matrix = OpenTK.Matrix4.LookAt((OpenTK.Vector3)Pos, (OpenTK.Vector3)(Pos - Vector3.UnitZ * Rot), (OpenTK.Vector3)Upward);
            gl.LoadMatrix(ref matrix);
        }

        public void Update()
        {
        }
    }
}
