using CommunityToolkit.Mvvm.ComponentModel;
using MAUIToDoList2023.Interfaces;
using MAUIToDoList2023.Models;
using System.Windows.Input;

namespace MAUIToDoList2023.ViewModels
{
    public partial class EditViewModel : ObservableValidator
    {
        private readonly IDataStore<TaskItem> _store;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _taskTitle = String.Empty;

        [ObservableProperty]
        private string _taskDescription = String.Empty;

        [ObservableProperty]
        private bool _isDone = false;

        [ObservableProperty]
        private DateTime _selectedDate = DateTime.Now;

        [ObservableProperty]
        private Importance _selectedImportance = Importance.Medium;

        // validace
        [ObservableProperty]
        private string _errorMsg = String.Empty;
        
        // vynulování proměnných
        public void ReturnToInitialState()
        {
            TaskTitle = String.Empty;
            TaskDescription = String.Empty;
            IsDone = false;
            SelectedDate = DateTime.Now;
            SelectedImportance = Importance.Medium;

            ErrorMsg = String.Empty;
        }
        public EditViewModel(IDataStore<TaskItem> store, MainViewModel mainViewModel)
        {
            _store = store;

            
            EditCommand = new Command(
                async () =>
                {
                    ValidateAllProperties();
                    if (HasErrors)
                        ErrorMsg = String.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                    else
                    {
                        var task = await _store.GetItemAsync(Id);
                        if (task != null)
                        {
                            
                        }
 
                        mainViewModel.UpdateCollection();

                        await Shell.Current.GoToAsync("..");
                        ReturnToInitialState();
                    }
                },
                () => true
            );
            
        }

        public ICommand EditCommand { get; private set; }

        public List<Importance> ImportanceList
        {
            get => Enum.GetValues(typeof(Importance)).Cast<Importance>().ToList();
        }

        //partial void OnTaskTitleChanged(string value) => ((Command)EditCommand).ChangeCanExecute();
    }
}
