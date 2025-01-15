using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ErrorOr;
using Solution.Core.Interfaces;
using Solution.Core.Models;

namespace Solution.DesktopApp.ViewModels;
[ObservableObject]
public partial class MainPageViewModel : RunModel
{
    public DateTime MaxDate => DateTime.Now;

    [ObservableProperty]
    private double datePickerWidth;

    public IAsyncRelayCommand OnSubmitCommand => new AsyncRelayCommand(OnsubmitAsync);

    public IRelayCommand DistanceValidationCommand => new RelayCommand(() => Distance.Validate());

    public IRelayCommand WeightValidationCommand => new RelayCommand(() => Weight.Validate());
    public IRelayCommand RunningTimeValidationCommand => new RelayCommand(() => RunningTime.Validate());

    private readonly IRunService runService;

    public MainPageViewModel(IRunService runService): base()
    {
    this.runService = runService;
        this.Date.Value = DateTime.Now;
    }

    private async Task OnsubmitAsync()
    {
        if (!IsFormValid)
        { 
            return;
        }

        ErrorOr<RunModel> serviceResponse = await runService.CreateAsync(this);

        string alertMessage = serviceResponse.IsError ? serviceResponse.FirstError.Description : "Run saved.";
        await Application.Current!.MainPage!.DisplayAlert("Alert", alertMessage, "ok");
    }

    private bool IsFormValid  => Distance.IsValid && RunningTime.IsValid && Weight.IsValid;
}
