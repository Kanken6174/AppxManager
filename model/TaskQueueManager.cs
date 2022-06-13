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

        public static void StopAll()
        {
            Queue.Clear();
        }

        public static void StartAsync()
        {
            try
            {
                Parallel.ForEach(Queue.Chunk(10),
                currentElement =>
                {
                    Task.Run(() =>
                    {
                        foreach (Task task in currentElement)
                        {
                            if (task.Status != TaskStatus.Running)
                                task.RunSynchronously();
                        }
                    });
                });
            }
            catch(NullReferenceException ex)
            {
                return;
            }
        }
    }
}