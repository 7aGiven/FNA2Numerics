using Microsoft.Build.Framework;
using System;

namespace FNA2NumericsTask
{
    public class FNA2NumericsTask : ITask
    {
        public IBuildEngine BuildEngine { get; set; }
        ITaskHost ITask.HostObject { get; set; }
        public ITaskItem[] InputAssemblies { get; set; }

        bool ITask.Execute()
        {
            foreach (ITaskItem taskItem in InputAssemblies)
            {
                try
                {
                    FNA2Numerics.FNA2Numerics.Process(taskItem.GetMetadata("FullPath"));
                    BuildEngine.LogMessageEvent(new BuildMessageEventArgs("FNA2NumericsTask:" + taskItem.GetMetadata("FullPath"), "helpKeyword", "senderName", MessageImportance.High));
                }
                catch (BadImageFormatException)
                {

                }
            }
            return true;
        }
    }
}
