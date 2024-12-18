

namespace Solution.DesktopApp.ViewModels;

[ObservableObject]
public partial class MainPageViewModel : MovieModel
{
    public DateTime MaxDateTime => DateTime.Now;

    [ObservableProperty]
    private double datePickerWidth;
    public IAsyncRelayCommand OnSubmitCommand => new AsyncRelayCommand(OnSubmitAsync);

    public IRelayCommand TitleValidationCommand => new RelayCommand(() => Title.Validate());
    public IRelayCommand LengthValidationCommand => new RelayCommand(() => Length.Validate());

    private readonly IMovieService movieService;
    public MainPageViewModel(IMovieService movieService) : base()
    {
        this.Release.Value = DateTime.Now;
        this.movieService = movieService;
    }
     private async Task OnSubmitAsync()
    {
        if (!IsFormValid)
        {
            return;
        }
        ErrorOr<MovieModel> serviceResponse = await movieService.CreateAsync(this);
        if (serviceResponse.IsError)
        { 
            await DisplayAlert("Alert", "You have ")
        }
    }

    private bool IsFormValid => Title.IsValid && Length.IsValid;
}
