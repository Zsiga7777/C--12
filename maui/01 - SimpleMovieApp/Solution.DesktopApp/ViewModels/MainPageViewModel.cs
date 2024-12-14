namespace Solution.DesktopApp.ViewModels;

public partial class MainPageViewModel : MovieModel
{
    public DateTime MaxDateTime => DateTime.Now;

    [ObservableProperty]
    private double datePickerWidth;
    public IAsyncRelayCommand OnSubmitCommand => new AsyncRelayCommand(OnSubmitAsync);

    private readonly IMovieService movieService;
    public MainPageViewModel(IMovieService movieService) 
    {
        this.Release = DateTime.Now;
        this.movieService = movieService;
    }
     private async Task OnSubmitAsync()
    {
        await movieService.CreateAsync(this);
    }
}
