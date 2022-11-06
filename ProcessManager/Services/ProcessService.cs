using ProcessManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessManager.Services
{
    internal static class ProcessService
    {
        public static List<ProcessModel> GetProcesses()
        {
            {
                List<ProcessModel> processes = new List<ProcessModel>();
                foreach (var process in System.Diagnostics.Process.GetProcesses())
                {
                    try
                    {
                        ProcessModel processModel = new ProcessModel(process);
                        processes.Add(processModel);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                return processes;
            }
        }
    }
}
