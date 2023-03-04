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
	IDataStore<TaskItem> _store;
	
	public MainPage(MainViewModel vm, IDataStore<TaskItem> store)
	{
		BindingContext = vm;
		_vm = vm;
		_store = store;
		
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
		await Shell.Current.GoToAsync(nameof(AddPage));
    }
}

