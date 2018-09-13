using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;

namespace CurtainFire4D.Renderer
{
    public static class TextureGenerator
    {
        private static Dictionary<string, int> TextureMap { get; } = new Dictionary<string, int>();

        public static int Generate(string path)
        {
            if (!TextureMap.ContainsKey(path))
            {
                gl.GenTextures(1, out int texture);

                gl.BindTexture(TextureTarget.Texture2D, texture);

                gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                using (Bitmap file = new Bitmap(path))
                {
                    BitmapData data = file.LockBits(new Rectangle(0, 0, file.Width, file.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    gl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                    file.UnlockBits(data);
                }

                gl.BindTexture(TextureTarget.Texture2D, 0);

                TextureMap[path] = texture;
            }
            return TextureMap[path];
        }

        public static void Clear()
        {
            foreach (int texture in TextureMap.Values)
            {
                gl.DeleteTexture(texture);
            }
            TextureMap.Clear();
        }
    }
}
