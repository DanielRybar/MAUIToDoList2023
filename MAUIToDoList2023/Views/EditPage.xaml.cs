using MAUIToDoList2023.ViewModels;

namespace MAUIToDoList2023.Views;

public partial class EditPage : ContentPage
{
    EditViewModel _vm;

    public EditPage(EditViewModel vm)
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