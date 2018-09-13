using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurtainFireCore;
using CurtainFire4D.Renderer;
using IronPython.Runtime;
using IronPython.Runtime.Operations;
using VecMath;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;

namespace CurtainFire4D.Entities
{
    public abstract class Entity
    {
        public World World { get; }

        public virtual Matrix4 WorldMat { get; protected set; }
        public Vector3 WorldPos => WorldMat.Translation;
        public Matrix3 WorldRot => WorldMat;

        public abstract IRenderer Renderer { get; }

        public virtual Matrix4 LocalMat { get; protected set; }

        public virtual Vector3 Pos { get; set; }

        public virtual Quaternion Rot { get; set; } = Quaternion.Identity;

        public virtual Entity Parent { get; protected set; }

        private ScheduledTaskManager TaskScheduler { get; } = new ScheduledTaskManager();

        public int LifeSpan { get; set; }
        public int FrameCount { get; set; }

        public Entity(World world, Entity parentEntity = null)
        {
            World = world;
            Parent = parentEntity;
        }

        public virtual void Frame()
        {
            FrameCount++;

            LocalMat = new Matrix4(Rot, Pos);
            WorldMat = Parent != null ? LocalMat * Parent.WorldMat : LocalMat;
        }

        public virtual void PreRender()
        {
            gl.MultMatrix((double[])WorldMat);
        }

        public void Spawn()
        {
            World.AddEntity(this);
        }

        public void Remove()
        {
            World.RemoveEntity(this);
        }

        private void AddTask(ScheduledTask task)
        {
            TaskScheduler.AddTask(task);
        }

        private void AddTask(PythonFunction task, Func<int, int> interval, int executeTimes, int waitTime, bool withArg = false)
        {
            if (withArg)
            {
                AddTask(new ScheduledTask(t => PythonCalls.Call(task, t), interval, executeTimes, waitTime));
            }
            else
            {
                AddTask(new ScheduledTask(t => PythonCalls.Call(task), interval, executeTimes, waitTime));
            }
        }

        public void AddTask(PythonFunction task, PythonFunction interval, int executeTimes, int waitTime, bool withArg = false)
        {
            AddTask(task, i => (int)PythonCalls.Call(interval, i), executeTimes, waitTime, withArg);
        }

        public void AddTask(PythonFunction task, int interval, int executeTimes, int waitTime, bool withArg = false)
        {
            AddTask(task, i => interval, executeTimes, waitTime, withArg);
        }
    }
}
