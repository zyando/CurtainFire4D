using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurtainFire4D
{
    public static class VectorExtensions
    {
        public static OpenTK.Vector3 GLVec(this VecMath.Vector3 v)
        {
            return new OpenTK.Vector3(v.x, v.y, v.z);
        }

        public static OpenTK.Vector4 GLVec(this VecMath.Vector4 v)
        {
            return new OpenTK.Vector4(v.x, v.y, v.z, v.w);
        }
    }
}
