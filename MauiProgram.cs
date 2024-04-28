﻿using Microsoft.Extensions.Logging;

namespace WrathBlueprintTree;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
			//builder.Services.AddTransient<MainPage>();
			builder.Services.AddTransient<BlueprintObject>();
			builder.Services.AddTransient<BpFile>();
			builder.Services.AddTransient<TreeBuilder>();
			//builder.Services.AddTransient<BlueprintModels>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
