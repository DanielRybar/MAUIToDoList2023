using CommunityToolkit.Mvvm.Messaging;
using MAUIToDoList2023.Data;
using MAUIToDoList2023.Interfaces;
using MAUIToDoList2023.Models;
using MAUIToDoList2023.ViewModels;
using System.Diagnostics;

namespace MAUIToDoList2023.Views;

public partial class MainPage : ContentPage
{
	MainViewModel _vm;
    EditViewModel _evm;
	IDataStore<TaskItem> _store;

    public MainPage(MainViewModel vm, IDataStore<TaskItem> store, EditViewModel evm)
    {
		BindingContext = vm;
		_vm = vm;
		_store = store;
        _evm = evm;
		
		InitializeComponent();

        /*
        MessagingCenter.Subscribe<MainViewModel, string>(this, "removeAll", 
            async (sender, msg) => 
            { 
                bool answer = await DisplayAlert("Potvrzení", msg, "Ano", "Ne"); 
                if (answer)
                {
                    await _store.RemoveAllItemsAsync();
                    _vm.UpdateCollection();
                    ((Command)_vm.RemoveAllCommand).ChangeCanExecute();
                }
            });
        */
        
        WeakReferenceMessenger.Default.Register<ConfirmationMessage>(this, async (r, msg) =>
        {
            bool answer = await DisplayAlert("Potvrzení", msg.Value, "Ano", "Ne");
            if (answer)
            {
                await _store.RemoveAllItemsAsync();
                _vm.UpdateCollection();
                _vm.SelectedTask = null;
                ((Command)_vm.RemoveAllCommand).ChangeCanExecute();
            }
        });
    }

    private async void OnFinishChecked(object sender, CheckedChangedEventArgs e)
    {      
        if (sender is CheckBox box && box.AutomationId != null /*&& _vm.FinishTaskCommand.CanExecute(null)*/)
		{
            Debug.WriteLine(box.AutomationId);
            int id = int.Parse(box.AutomationId);
			var task = await _store.GetItemAsync(id);

            if (task != null)
			{
                _vm.SelectedTask = task;
				Debug.WriteLine(_vm.SelectedTask.Title);
                _vm.FinishTaskCommand.Execute(null);
            }
        }
    }

    private async void OnAddButtonPressed(object sender, EventArgs e)
    {
        _vm.SelectedTask = null;
		await Shell.Current.GoToAsync(nameof(AddPage));
    }

    private async void TapGestureRecognizer_Highlight(object sender, TappedEventArgs e)
    {
        if (sender is Grid grid && grid.AutomationId != null)
            _vm.SelectedTask = await _store.GetItemAsync(int.Parse(grid.AutomationId));
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Debug.WriteLine("Tapped on: " + _vm.SelectedTask.Title);
        if (sender is Grid grid && grid.AutomationId != null)
            _vm.SelectedTask = await _store.GetItemAsync(int.Parse(grid.AutomationId));

        _evm.Id = _vm.SelectedTask.TaskId;
        _evm.TaskTitle = _vm.SelectedTask.Title;
        _evm.TaskDescription = _vm.SelectedTask.Description;
        _evm.SelectedDate = _vm.SelectedTask.EndDate;
        _evm.IsDone = _vm.SelectedTask.IsDone;
        _evm.SelectedImportance = _vm.SelectedTask.Importance;
        

        await Shell.Current.GoToAsync(nameof(EditPage));

        _vm.SelectedTask = null;
    }
}

