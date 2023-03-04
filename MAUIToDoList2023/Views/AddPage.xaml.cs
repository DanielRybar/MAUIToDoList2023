using MAUIToDoList2023.ViewModels;

namespace MAUIToDoList2023.Views;

public partial class AddPage : ContentPage
{
    AddViewModel _vm;

    public AddPage(AddViewModel vm)
    {
        BindingContext = vm;
        _vm = vm;
        
        InitializeComponent();
	}

    private async void OnCancelButtonPressed(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
        _vm.ReturnToInitialState();
    }
}