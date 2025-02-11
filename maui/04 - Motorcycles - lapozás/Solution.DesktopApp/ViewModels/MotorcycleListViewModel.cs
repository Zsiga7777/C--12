using Microsoft.EntityFrameworkCore;
using Solution.Core.Interfaces;
using System.Collections.ObjectModel;

namespace Solution.DesktopApp.ViewModels;

[ObservableObject]
public partial class MotorcycleListViewModel(IMotorcycleService motorcycleService)
{

    #region life cycle commands
    public IAsyncRelayCommand AppearingCommand => new AsyncRelayCommand(OnAppearingAsync);
    public IAsyncRelayCommand DisappearingCommand => new AsyncRelayCommand(OnDisappearingAsync);
    #endregion

    public IAsyncRelayCommand LoadPreviousPageCommand => new AsyncRelayCommand(LoadPreviousPageAsync);

    public IAsyncRelayCommand LoadNextPageCommand => new AsyncRelayCommand(LoadNextPageAsync);


    [ObservableProperty]
    private ObservableCollection<MotorcycleModel> motorcycles;

    private int page = 0;
    private async Task OnAppearingAsync()
    {
        await LoadMotorcycles();
    }

    private async Task LoadPreviousPageAsync()
    {
        if (page <= 0)
        {
            page = 0;
        }
        else
        {
            page--;
        }
        await LoadMotorcycles();
    }

    private async Task LoadNextPageAsync()
    {
        int maxNumberOfPages =await motorcycleService.GetMaximumNumberOfPagesAsync();


        if (page >= maxNumberOfPages -1)
        {
            page = maxNumberOfPages - 1;
        }
        else
        {
            page++;
        }
        await LoadMotorcycles();
    }

    private async Task OnDisappearingAsync()
    { }

    private async Task LoadMotorcycles()
    {
        var result = await motorcycleService.GetPageAsync(page);

        if (result.IsError)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Motorcycles not loaded!", "OK");
            return;
        }

        Motorcycles = new ObservableCollection<MotorcycleModel>(result.Value);
    }

}
