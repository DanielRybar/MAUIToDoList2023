using MAUIToDoList2023.Data;
using MAUIToDoList2023.Interfaces;
using MAUIToDoList2023.Models;
using MAUIToDoList2023.Services;
using MAUIToDoList2023.ViewModels;
using MAUIToDoList2023.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MAUIToDoList2023;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .RegisterViewModels()
            .RegisterViews()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddDbContext<TaskDbContext>(options =>
        {
            //options.UseSqlite(@"Data Source=tasks.sqlite");
            string path = Path.Combine(FileSystem.AppDataDirectory, "tasks.sqlite");
            options.UseSqlite("Data Source=" + path);
            Debug.WriteLine(path);
            options.LogTo((param) => { Debug.WriteLine(param); }, new[]
            { 
                //CoreEventId.ContextInitialized,
                RelationalEventId.CommandExecuted
            });
            options.EnableSensitiveDataLogging();
        });

        builder.Services.AddScoped<IDataStore<TaskItem>, TaskStore>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<AddViewModel>();
        builder.Services.AddSingleton<EditViewModel>();
        // ...
        return builder;
    }
    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<AddPage>();
        builder.Services.AddSingleton<EditPage>();
        // ...
        return builder;
    }
}
