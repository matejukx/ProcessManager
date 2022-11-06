using ProcessManager.Models;
using ProcessManager.Services;
using ProcessManager.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProcessManager.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Processes = new List<ProcessModel>();
            timer.Tick += async (s, e) => {
                await RefreshProcessesAsync();
            };
            timer.Interval = new TimeSpan(0, 0, _interval);
            timer.Start();
            RefreshProcessesAsync();

            KillCommand = new DelegateCommand(KillProcess);
            SetPriorityCommand = new DelegateCommand(SetPriority);
        }

        public ICommand KillCommand
        {
            get;
            set;
        }

        public ICommand SetPriorityCommand
        {
            get;
            set;
        }

        private List<Priority> PrioritiesList = new() { new Priority("Idle"), new Priority("High"), new Priority("Normal") };
        public CollectionView Priorities => new CollectionView(PrioritiesList);
        public string Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }

        public ProcessModel SelectedProcess
        {
            get
            {
                return _selectedProcess;
            }
            set
            {
                _selectedProcess = value;
                OnPropertyChanged("SelectedProcess");
            }
        }

        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                OnFilterChange();
                OnPropertyChanged("Filter");
            }
        }

        public bool IsRefreshEnabled
        {
            get
            {
                return _isRefreshEnabled;
            }
            set
            {
                _isRefreshEnabled = value;
                OnRefreshChecked();
                OnPropertyChanged("IsRefreshEnabled");
            }
        }

        public string Interval
        {
            get
            {
                return _interval.ToString();
            }
            set
            {
                _ = int.TryParse(value, out _interval);
                OnIntervalChange();
                OnPropertyChanged("Interval");
            }
        }

        public List<ProcessModel> Processes { 
            get {
                return _processes;
            } 
            set {
                _processes = value;
                OnPropertyChanged("Processes");
            } }

        public async void KillProcess(object obj)
        {
            if (_selectedProcess is not null)
            {
                _selectedProcess.Process.Kill();
                Processes.Remove(_selectedProcess);
                await RefreshProcessesAsync();
            }
        }

        public async void SetPriority(object obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(_priority) && SelectedProcess is not null)
                {
                    SelectedProcess.Process.PriorityClass = Enum.Parse<System.Diagnostics.ProcessPriorityClass>(_priority);
                    await RefreshProcessesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);   
            }
        }

        private void OnRefreshChecked()
        {
            if (_isRefreshEnabled && timer.IsEnabled)
            {
                return;
            }
            if (_isRefreshEnabled && !timer.IsEnabled)
            {
                timer.Start();
            }
            if (!_isRefreshEnabled)
            {
                timer.Stop();
            }
        }

        private void OnFilterChange()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                return;
            }
            Processes = Processes.Where(p => p.ProcessName.Contains(Filter)).ToList();
        }

        private void OnIntervalChange()
        {
            timer.Stop();
            timer.Interval = new TimeSpan(0, 0, _interval);
            timer.Start();
        }

        private async Task RefreshProcessesAsync()
        {
            Processes = await Task.Run(() => { return ProcessService.GetProcesses(); });
            OnFilterChange();
        }

        private DispatcherTimer timer = new();
        private List<ProcessModel> _processes;
        private string _filter;
        private int _interval = 10;
        private bool _isRefreshEnabled = true;
        private ProcessModel _selectedProcess;
        private string _priority;

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
