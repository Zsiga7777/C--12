using ErrorOr;
using Microsoft.UI.Text;
using Solution.Core.Interfaces;
using Solution.Services;

namespace Solution.DesktopApp.ViewModels;

[ObservableObject]
public partial class CreateOrEditMotorcycleViewModel(AppDbContext dbContext, IMotorcycleService motorcycleService) : MotorcycleModel(), IQueryAttributable
{
    #region life cycle commands
    public IAsyncRelayCommand AppearingCommand => new AsyncRelayCommand(OnAppearingAsync);
    public IAsyncRelayCommand DisappearingCommand => new AsyncRelayCommand(OnDisappearingAsync);
    #endregion

    #region Validation commands
    public IRelayCommand ManufacturerIndexChangedCommand => new RelayCommand(() => this.Manufacturer.Validate());

    public IRelayCommand CylindersIndexChangedCommand => new RelayCommand(() => this.NumberOfCylinders.Validate());

    public IRelayCommand ModelValidationCommand => new RelayCommand(() => this.Model.Validate());

    public IRelayCommand CubicValidationCommand => new RelayCommand(() => this.Cubic.Validate());

    public IRelayCommand ReleaseYearValidationCommand => new RelayCommand(() => this.ReleaseYear.Validate());
    #endregion

    public IAsyncRelayCommand SaveCommand => new AsyncRelayCommand(OnSaveAsync);

    [ObservableProperty]
    private ICollection<ManufacturerModel> manufacturers;

    [ObservableProperty]
    private ICollection<uint> cylinders = [1, 2, 3, 4, 6, 8];

    private async Task OnAppearingAsync()
    {
       await LoadManufacturers();
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {

        bool hasValue = query.TryGetValue("Motorcycle", out object result);

        if (!hasValue)
        {
            return;
        }
        MotorcycleModel motorcycle = result as MotorcycleModel;


        this.Manufacturer.Value = motorcycle.Manufacturer.Value;
        this.Model.Value = motorcycle.Model.Value;
        this.ReleaseYear.Value = motorcycle.ReleaseYear.Value;
        this.Cubic.Value = motorcycle.Cubic.Value;
        this.NumberOfCylinders.Value = motorcycle.NumberOfCylinders.Value;
    }
    private async Task OnDisappearingAsync()
    { }

    private async Task OnSaveAsync()
    {
        if (!IsFormValid())
        {
            return;
        }
       var serviceResponse = await motorcycleService.CreateAsync(this);

        string alertMessage = serviceResponse.IsError ? serviceResponse.FirstError.Description : "Motorcycle saved!";
        string title = serviceResponse.IsError ? "Error" : "Information";

        if (!serviceResponse.IsError)
        { 
            ClearForm();
        }

        await Application.Current!.MainPage!.DisplayAlert(title, alertMessage, "OK");
    }

    private async Task LoadManufacturers()
    {
       
            Manufacturers = await dbContext.Manufacturers.AsNoTracking()
                                                            .OrderBy(x => x.Name)
                                                           .Select(x => new ManufacturerModel(x))
                                                           .ToListAsync();
      
    }
    private void ClearForm()
    {
        this.Manufacturer.Value = null;
        this.Model.Value = null;
        this.Cubic.Value = null;
        this.ReleaseYear.Value = null;
        this.NumberOfCylinders.Value = null;
    }
    private bool IsFormValid()
    { 
        this.Manufacturer.Validate();
        this.Model.Validate();
        this.Cubic.Validate();
        this.ReleaseYear.Validate();
        this.NumberOfCylinders.Validate();

        return this.Manufacturer?.IsValid ?? false &&
            this.Model.IsValid &&
            this.Cubic.IsValid &&
            this.ReleaseYear.IsValid &&
            this.NumberOfCylinders.IsValid;
    }
}
