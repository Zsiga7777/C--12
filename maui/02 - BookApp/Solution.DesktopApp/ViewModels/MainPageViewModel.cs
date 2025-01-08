using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ErrorOr;
using Microsoft.EntityFrameworkCore.Storage;
using Solution.Core.Interfaces;
using Solution.Core.Models;
using Solution.Services;

namespace Solution.DesktopApp.ViewModels;

[ObservableObject]
public partial class MainPageViewModel : BookModel
{
    public IAsyncRelayCommand OnSubmitCommand => new AsyncRelayCommand(OnSubmitAsync);

    public IRelayCommand IdValidationCommand => new RelayCommand(() => Id.Validate());
    public IRelayCommand TitleValidationCommand => new RelayCommand(() => Title.Validate());
    public IRelayCommand WritersValidationCommand => new RelayCommand(() => Writers.Validate());
    public IRelayCommand ReleaseYearValidationCommand => new RelayCommand(() => ReleaseYear.Validate());
    public IRelayCommand PublisherValidationCommand => new RelayCommand(() => Publisher.Validate());

    private readonly IBookService bookService;

    public MainPageViewModel(IBookService bookService): base()
    {
        this.bookService = bookService;
    }

    private async Task OnSubmitAsync()
    {
        if (!IsFormValid)
        {
            return;
        }

        ErrorOr<BookModel> serviceResponse = await bookService.CreateAsync(this);

        string alertMessage = serviceResponse.IsError ? serviceResponse.FirstError.Description : "Book saved!";
        await Application.Current!.MainPage!.DisplayAlert("Alert", alertMessage, "OK");
    }

    private bool IsFormValid => Id.IsValid &&
                                Title.IsValid && 
                                Writers.IsValid &&
                                ReleaseYear.IsValid
                                && Publisher.IsValid;
}
