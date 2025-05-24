using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace dev.tsubakimoto.Agentime.ViewModels
{
    public class ClockViewModel : INotifyPropertyChanged
    {
        private readonly IDispatcher? _dispatcher;
        private System.Timers.Timer _timer;
        private DateTime _currentTime;
        private string _selectedTimeZone;
        private bool _isDarkMode = true;
        private ObservableCollection<string> _availableTimeZones = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ClockViewModel()
        {
            _dispatcher = Application.Current?.Dispatcher;
            
            // Set default timezone to Tokyo
            _selectedTimeZone = "Asia/Tokyo";
            
            // Initialize time
            UpdateCurrentTime();
            
            // Set up timer to update every second
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += (s, e) =>
            {
                _dispatcher?.Dispatch(UpdateCurrentTime);
            };
            _timer.Start();
            
            // Initialize timezone list
            InitializeTimeZones();
            
            // Initialize commands
            ToggleThemeCommand = new Command(ToggleTheme);
        }
        
        private void InitializeTimeZones()
        {
            var zones = new List<string>
            {
                "Asia/Tokyo",
                "Asia/Seoul",
                "Asia/Shanghai",
                "Asia/Singapore",
                "Australia/Sydney",
                "Europe/London",
                "Europe/Paris",
                "America/New_York",
                "America/Los_Angeles",
                "Pacific/Auckland"
            };
            
            AvailableTimeZones = new ObservableCollection<string>(zones.OrderBy(z => z));
        }

        private void UpdateCurrentTime()
        {
            var timeZoneInfo = GetTimeZoneInfo();
            CurrentTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }
        
        private TimeZoneInfo GetTimeZoneInfo()
        {
            try
            {
                // Try to find the timezone by ID
                return TimeZoneInfo.FindSystemTimeZoneById(SelectedTimeZone);
            }
            catch
            {
                // If timezone ID is not found, default to local timezone
                return TimeZoneInfo.Local;
            }
        }
        
        private void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            
            // Apply theme (implementation to be handled in the App class)
            if (Application.Current != null)
            {
                Application.Current.UserAppTheme = IsDarkMode ? AppTheme.Dark : AppTheme.Light;
            }
        }

        public DateTime CurrentTime
        {
            get => _currentTime;
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedTimeZone
        {
            get => _selectedTimeZone;
            set
            {
                if (_selectedTimeZone != value)
                {
                    _selectedTimeZone = value;
                    OnPropertyChanged();
                    UpdateCurrentTime();
                }
            }
        }

        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> AvailableTimeZones
        {
            get => _availableTimeZones;
            set
            {
                if (_availableTimeZones != value)
                {
                    _availableTimeZones = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public ICommand ToggleThemeCommand { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}