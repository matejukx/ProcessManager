using System;
using System.ComponentModel;
using System.Diagnostics;

namespace ProcessManager.Models
{
    internal class ProcessModel : INotifyPropertyChanged  
    {
        private string _processName;
        private string _priorityClass;
        private double _totalProcessorTime;
        private long _memory;
        private DateTime _startTime;
        private ProcessThreadCollection _threads;
        private Process _process;


        public int ThreadsNumber => _threads.Count;

        public string GetStartTimeString => StartTime.ToString();
        
        public DateTime StartTime {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        public ProcessThreadCollection Threads
        {
            get { return _threads; }
            set
            {
                _threads = value;
                OnPropertyChanged("Threads");
            }
        }

        public Process Process
        {
            get { return _process; }
            set
            {
                _process = value;
            }
        }

        public string ProcessName {
            get 
            {
                return _processName;
            } 
            set 
            {
                _processName = value;
                OnPropertyChanged("ProcessName");
            } 
        }
        public string PriorityClass
        {
            get
            {
                return _priorityClass;
            }
            set
            {
                _priorityClass = value;
                OnPropertyChanged("PriorityClass");
            }
        }

        public double TotalProcessorTime
        {
            get
            {
                return _totalProcessorTime;
            }
            set
            {
                _totalProcessorTime = value;
                OnPropertyChanged("TotalProcessorTime");
            }
        }

        public long Memory
        {
            get
            {
                return _memory;
            }
            set
            {
                _memory = value;
                OnPropertyChanged("Memory");
            }
        }

        public ProcessModel(Process process)
        {
            ProcessName = process.ProcessName;
            PriorityClass = process.PriorityClass.ToString();
            TotalProcessorTime = process.TotalProcessorTime.TotalSeconds;
            Memory = process.WorkingSet64;
            StartTime = process.StartTime;
            Threads = process.Threads;
            Process = process;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
