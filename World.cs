using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurtainFire4D.Entities;
using CurtainFire4D.Renderer;
using CurtainFireCore;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFire4D
{
    public class World
    {
        private List<Entity> AddEntityList { get; } = new List<Entity>();
        private List<Entity> RemoveEntityList { get; } = new List<Entity>();
        public HashSet<Entity> EntityList { get; } = new HashSet<Entity>();
        public int FrameCount { get; set; }

        public GameCamera Camera { get; }

        private ScheduledTaskManager TaskScheduler { get; } = new ScheduledTaskManager();

        public World(Game game)
        {
            Camera = new GameCamera(this);
            Camera.Spawn();
        }

        public void Init()
        {

        }

        public void AddEntity(Entity entity)
        {
            AddEntityList.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            RemoveEntityList.Add(entity);
        }

        internal void Frame()
        {
            TaskScheduler.Frame();

            AddEntityList.ForEach(e => EntityList.Add(e));
            RemoveEntityList.ForEach(e => EntityList.Remove(e));

            AddEntityList.Clear();
            RemoveEntityList.Clear();

            EntityList.ForEach(e => e.Frame());

            FrameCount++;
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
