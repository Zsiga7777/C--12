using Solution.Core.Interfaces;
using Solution.Services;

namespace Solution.DesktopApp.Configurations;

public static class ConfigureDI
{
	public static MauiAppBuilder UseDIConfiguration(this MauiAppBuilder builder)
	{
		builder.Services.AddTransient<MainPageViewModel>();

        builder.Services.AddTransient<IRunService, RunService>();

		return builder;
	}
}
