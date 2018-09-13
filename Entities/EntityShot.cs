using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecMath;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;
using CurtainFire4D.Renderer;

namespace CurtainFire4D.Entities
{
    public class EntityShot : Entity
    {
        public Vector3 LookAtVec { get; set; }

        public Vector3 Velocity { get; set; }
        public Vector3 Upward { get; set; } = Vector3.UnitY;

        public Func<EntityShot, Quaternion> GetRot { get; set; } = e => e.Velocity != Vector3.Zero ? (Quaternion)Matrix3.LookAt(e.Velocity, e.Upward) : e.Rot;

        public Matrix4 Scale { get; set; } = Matrix4.Identity;

        public Vector4 Color { get; set; } = new Vector4(1, 1, 1, 1);

        public ShotType ShotType { get; }
        public override IRenderer Renderer => ShotType;

        public EntityShot(World world, string typeName, int color)
       : this(world, typeName, color, Matrix4.Identity) { }

        public EntityShot(World world, string typeName, int color, float scale)
        : this(world, typeName, color, new Matrix3(scale)) { }

        public EntityShot(World world, string typeName, int color, Vector3 scale)
        : this(world, typeName, color, new Matrix3(scale)) { }

        public EntityShot(World world, string typeName, int color, Matrix3 scale)
        : this(world, typeName, color, (Matrix4)scale) { }

        public EntityShot(World world, string typeName, int color, Matrix4 scale) : this(world, ShotType.TypeDictionary[typeName], color, scale) { }

        public EntityShot(World world, ShotType type, int color, Matrix4 scale) : base(world)
        {
            ShotType = type;
            Color = new Vector4((color >> 16 & 0x000000FF) / 255.0F, (color >> 8 & 0x000000FF) / 255.0F, (color >> 0 & 0x000000FF) / 255.0F, 1);

            ShotType.InitEntity(this);
        }

        public override void Frame()
        {
            Rot = GetRot(this);
            Pos += Velocity;
            base.Frame();
        }

        public override void PreRender()
        {
            base.PreRender();

            gl.MultMatrix((double[])Scale);
            gl.Color4(Color.GLVec());
        }
    }
}
