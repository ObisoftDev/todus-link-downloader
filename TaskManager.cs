using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TDM
{
    public class TaskManager
    {
        public static List<Thread> Tareas = new List<Thread>();

        public static void Run(Action action)
        {
            Thread t = new Thread(new ThreadStart(action));
            Tareas.Add(t);
            t.Start();
        }
        public static void AbortAll()
        {
            Tareas.ForEach(t=> { t?.Abort();});
            Tareas.Clear();
        }
    }
}
