using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CurtainFireMaker.Renderer;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;

namespace CurtainFireMaker.Entities
{
    public abstract class ShotType : IRenderer
    {
        public string Name { get; }

        public ShotType(string name)
        {
            Name = name;
        }

        public abstract void Render(IEnumerable<Entity> entities);
    }

    public class ShotTypeWavefront : ShotType
    {
        public Wavefront Obj { get; }

        public ShotTypeWavefront(string name, string path) : base(name)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Obj = new Wavefront(new StreamReader(stream));
            }
        }

        public override void Render(IEnumerable<Entity> entities)
        {
            gl.EnableClientState(ArrayCap.VertexArray);
            gl.EnableClientState(ArrayCap.NormalArray);
            gl.EnableClientState(ArrayCap.TextureCoordArray);

            gl.VertexPointer(3, VertexPointerType.Float, 12, Obj.Vertices);
            gl.NormalPointer(NormalPointerType.Float, 12, Obj.Normals);
            gl.TexCoordPointer(2, TexCoordPointerType.Float, 8, Obj.Texcoords);

            foreach (var entity in entities)
            {
                gl.Color4(1.0F, 1.0F, 1.0F, 1.0F);

                gl.PushMatrix();

                gl.MultMatrix((double[])(entity.WorldMat));
                gl.DrawElements(PrimitiveType.Triangles, Obj.Indices.Length, DrawElementsType.UnsignedInt, Obj.Indices);

                gl.PopMatrix();
            }

            gl.DisableClientState(ArrayCap.VertexArray);
            gl.DisableClientState(ArrayCap.NormalArray);
            gl.DisableClientState(ArrayCap.TextureCoordArray);
        }
    }
}
