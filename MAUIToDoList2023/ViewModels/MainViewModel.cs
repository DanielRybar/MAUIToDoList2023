using CommunityToolkit.Mvvm.Messaging;
using MAUIToDoList2023.Data;
using MAUIToDoList2023.Interfaces;
using MAUIToDoList2023.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MAUIToDoList2023.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public async void UpdateCollection()
        {
            var newList = await _context.TaskItems.OrderBy(x => x.IsDone).ThenBy(x => x.EndDate).ToListAsync();
            TaskItems = new ObservableCollection<TaskItem>(newList);
        }

        private readonly TaskDbContext _context;
        private readonly IDataStore<TaskItem> _store;

        private ObservableCollection<TaskItem> _taskItems = new();
        private TaskItem _selectedTask;

        public MainViewModel(TaskDbContext context, IDataStore<TaskItem> store)
        {
            _context = context;
            _store = store;

            UpdateCollection();

            RemoveAllCommand = new Command(
                /*async*/ () =>
                {
                    //MessagingCenter.Send(this, "removeAll", "Opravdu chcete odstranit všechny úkoly?");
                    WeakReferenceMessenger.Default.Send(new ConfirmationMessage("Opravdu chcete odstranit všechny úkoly?"));

                    /*
                    await _store.RemoveAllItemsAsync();

                    UpdateCollection();
                    ((Command)RemoveAllCommand).ChangeCanExecute();
                    */
                },
                () => TaskItems.Count > 0
            );

            FinishTaskCommand = new Command(
                async () =>
                {
                    int id = SelectedTask.TaskId;
                    var finishedTasks = await _context.TaskItems.Where(x => x.IsDone).ToListAsync();
                    var unfinishedTasks = await _context.TaskItems.Where(x => !x.IsDone).ToListAsync();

                    var taskItem = await _store.GetItemAsync(id);
                    if (taskItem != null)
                    {
                        if (finishedTasks.Contains(taskItem)) taskItem.IsDone = false;
                        else if (unfinishedTasks.Contains(taskItem)) taskItem.IsDone = true;
                    }
                    await _context.SaveChangesAsync();

                    MainThread.BeginInvokeOnMainThread(
                    () =>
                    {
                        TaskItems.Clear();
                        UpdateCollection();
                    });
                },
                () => SelectedTask != null
            );

            RemoveCommand = new Command(
                async () =>
                {
                    int id = SelectedTask.TaskId;
                    var task = await _store.GetItemAsync(id);
                    if (task != null)
                    {
                        await _store.DeleteItemAsync(task);
                    }

                    SelectedTask = null;
                    UpdateCollection();
                    ((Command)RemoveAllCommand).ChangeCanExecute();
                },
                () => SelectedTask != null
            );
        }

        public ICommand RemoveAllCommand { get; private set; }
        public ICommand FinishTaskCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }

        public ObservableCollection<TaskItem> TaskItems
        {
            get { return _taskItems; }
            set
            {
                _taskItems = value;
                //Dispatcher.GetForCurrentThread().Dispatch(() => NotifyPropertyChanged(nameof(TaskItems)));
                NotifyPropertyChanged();
                //((Command)RemoveAllCommand).ChangeCanExecute();
            }
        }

        public TaskItem SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                NotifyPropertyChanged();
                ((Command)FinishTaskCommand).ChangeCanExecute();
                ((Command)RemoveCommand).ChangeCanExecute();
            }
        }
    }
}