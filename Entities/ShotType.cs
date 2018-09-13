using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CurtainFire4D.Renderer;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;
using VecMath;

namespace CurtainFire4D.Entities
{
    public abstract class ShotType : IRenderer
    {
        public static Dictionary<string, ShotType> TypeDictionary { get; } = new Dictionary<string, ShotType>();

        public static void Register(ShotType[] types)
        {
            foreach (var type in types)
            {
                TypeDictionary[type.Name] = type;
            }
        }

        public string Name { get; }

        public ShotType(string name)
        {
            Name = name;
        }

        public virtual void InitEntity(EntityShot entity) { }

        public abstract void Render(IEnumerable<Entity> entities);
    }

    public class ShotTypeWavefront : ShotType
    {
        public Wavefront Obj { get; }

        public ShotTypeWavefront(string name, string path, float scale) : base(name)
        {
            path = Path.IsPathRooted(path) ? path : "Resource\\" + path;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Obj = new Wavefront(new StreamReader(stream));
            }
            Obj.Scale(scale);
        }

        protected virtual void RenderEntity(Entity entity)
        {
            gl.Color4(1.0F, 1.0F, 1.0F, 1.0F);

            gl.PushMatrix();

            entity.PreRender();
            gl.DrawElements(PrimitiveType.Triangles, Obj.Indices.Length, DrawElementsType.UnsignedInt, Obj.Indices);

            gl.PopMatrix();
        }

        public override void Render(IEnumerable<Entity> entities)
        {
            gl.EnableClientState(ArrayCap.VertexArray);
            gl.EnableClientState(ArrayCap.NormalArray);
            gl.EnableClientState(ArrayCap.TextureCoordArray);

            gl.VertexPointer(3, VertexPointerType.Float, 12, Obj.Vertices);
            gl.NormalPointer(NormalPointerType.Float, 12, Obj.Normals);
            gl.TexCoordPointer(2, TexCoordPointerType.Float, 8, Obj.Texcoords);

            entities.ForEach(RenderEntity);

            gl.DisableClientState(ArrayCap.VertexArray);
            gl.DisableClientState(ArrayCap.NormalArray);
            gl.DisableClientState(ArrayCap.TextureCoordArray);
        }
    }

    public class ShotTypeBillboard : ShotType
    {
        public float Scale { get; }
        public int Texture { get; }

        public ShotTypeBillboard(string name, string texturePath, float scale) : base(name)
        {
            Scale = scale;
            Texture = TextureGenerator.Generate(texturePath);
        }

        public override void InitEntity(EntityShot entity)
        {
            entity.GetRot = e => Matrix3.LookAt((e.WorldPos - e.World.Camera.WorldPos), e.Upward);
        }

        protected virtual void RenderEntity(Entity entity)
        {
            gl.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            gl.PushMatrix();

            entity.PreRender();

            gl.Begin(PrimitiveType.Quads);
            gl.TexCoord2(1, 0);
            gl.Vertex3(Scale, Scale, 0);
            gl.TexCoord2(1, 1);
            gl.Vertex3(Scale, -Scale, 0);
            gl.TexCoord2(0, 1);
            gl.Vertex3(-Scale, -Scale, 0);
            gl.TexCoord2(0, 0);
            gl.Vertex3(-Scale, Scale, 0);
            gl.End();

            gl.PopMatrix();
        }

        public override void Render(IEnumerable<Entity> entities)
        {
            gl.BindTexture(TextureTarget.Texture2D, Texture);

            entities.ForEach(RenderEntity);

            gl.BindTexture(TextureTarget.Texture2D, -1);
        }
    }
}
