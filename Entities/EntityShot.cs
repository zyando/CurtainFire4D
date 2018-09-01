using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecMath;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;
using CurtainFireMaker.Renderer;

namespace CurtainFireMaker.Entities
{
    public class EntityShot : Entity
    {
        public Vector3 LookAtVec { get; set; }

        public Vector3 Velocity { get; set; }
        public Vector3 Upward { get; set; } = Vector3.UnitY;

        public Matrix4 Scale { get; set; } = Matrix4.Identity;

        public ShotType ShotType { get; }

        public override IRenderer Renderer => ShotType;

        public EntityShot(World world, ShotType type) : base(world)
        {
            ShotType = type;
        }

        public override void Frame()
        {
            if (Velocity != Vector3.Zero)
            {
                Rot = Matrix3.LookAt(Velocity, Upward);
            }
            Pos += Velocity;
            base.Frame();
        }
    }
}
