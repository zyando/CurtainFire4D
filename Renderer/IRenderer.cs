using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurtainFireMaker.Entities;

namespace CurtainFireMaker.Renderer
{
    public interface IRenderer
    {
        void Render(IEnumerable<Entity> entities);
    }

    public class NoneRenderer : IRenderer
    {
        public static NoneRenderer Instance { get; } = new NoneRenderer();

        public void Render(IEnumerable<Entity> entities)
        {
        }
    }
}
