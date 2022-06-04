using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppxManager.model
{
    public static class TaskQueueManager
    {
        public static List<Task> Queue = new List<Task>();

        public static void StartAsync()
        {
            Parallel.ForEach(Queue.Chunk(35),
            currentElement =>
            {
                Task.Run(() =>
                {
                    foreach (Task task in currentElement)
                    {
                        if(task.Status != TaskStatus.Running)
                            task.RunSynchronously();
                    }
                });
            });
        }
    }
}