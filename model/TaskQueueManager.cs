using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppxManager.model
{
    /// <summary>
    /// This static class helps with managing parrallel tasks, it is mainly used to get each appx's details such as the logo, ect...
    /// </summary>
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