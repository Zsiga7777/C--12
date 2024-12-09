namespace Solution.DesktopApp.ViewModels;

public partial class MainPageViewModel : MovieModel
{
    public DateTime MaxDateTime => DateTime.Now;

    public IAsyncRelayCommand OnSubmitCommand => new AsyncRelayCommand(OnSubmitAsync);

    public MainPageViewModel() 
    {
        this.Release = DateTime.Now;
    }
     private async Task OnSubmitAsync()
    { 
        
    }
}
