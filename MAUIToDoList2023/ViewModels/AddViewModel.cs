using CommunityToolkit.Mvvm.ComponentModel;
using MAUIToDoList2023.Interfaces;
using MAUIToDoList2023.Models;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace MAUIToDoList2023.ViewModels
{
    public partial class AddViewModel : /*ObservableObject*/ ObservableValidator
    {
        private readonly IDataStore<TaskItem> _store;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Název je povinný")]
        [MaxLength(10, ErrorMessage = "Maximální délka názvu je 10 znaků")]
        //[NotifyCanExecuteChangedFor(nameof(AddCommand))]
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

        public AddViewModel(IDataStore<TaskItem> store, MainViewModel mainViewModel)
        {
            _store = store;
            
            AddCommand = new Command(
                async () =>
                {
                    ValidateAllProperties();
                    if (HasErrors)
                        ErrorMsg = String.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                    else
                    {
                        var task = new TaskItem
                        {
                            Title = TaskTitle,
                            Description = TaskDescription,
                            IsDone = IsDone,
                            EndDate = SelectedDate,
                            Importance = SelectedImportance
                        };
                        await _store.AddItemAsync(task);

                        mainViewModel.UpdateCollection();
                        await Shell.Current.GoToAsync("..");
                        ReturnToInitialState();
                    }
                },
                () => true /*!String.IsNullOrWhiteSpace(TaskTitle)*/
            );
        }

        public ICommand AddCommand { get; private set; }

        public List<Importance> ImportanceList
        {
            get => Enum.GetValues(typeof(Importance)).Cast<Importance>().ToList();
        }

        partial void OnTaskTitleChanged(string value) => ((Command)AddCommand).ChangeCanExecute();
    }
}