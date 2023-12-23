using DesktopApp.Command;
using System;

namespace DesktopApp.Service
{
    public class NavigationService : ObservableObject
    {
        public delegate void ViewChangedEventHandler(object sender, Type newViewType);
        public event ViewChangedEventHandler ViewChanged;
        private readonly Func<Type, ViewModelBase> _viewModelFactory;
        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get => _currentView;
            private set
            {
                
                _currentView = value;

                
                OnPropertyChanged();

                OnViewChanged(value.GetType());
            }
        }

        public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        private void OnViewChanged(Type viewType)
        {
            ViewChanged?.Invoke(this, viewType);
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;

            MessagingService.Instance.DeselectAll();
        }
    }
}