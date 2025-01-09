using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SlotMachine.Services;
using SlotMachine.ViewModels;
using SlotMachine.ViewModels.SlotViewModels;
using SlotMachine.Views;
using Syncfusion.Maui.Core.Hosting;
#if ANDROID
using Android.Content.Res;
using Android.Graphics;
#endif

namespace SlotMachine
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .UseSkiaSharp();

            builder.Services.AddSingleton<SlotViewModel>();
            builder.Services.AddSingleton<GameService>();

            builder.Services.AddSingleton<AchievementManager>();
            builder.Services.AddSingleton<AchievementsViewModel>();
            builder.Services.AddTransient<AchievementView>();

            builder.Services.AddSingleton<StatisticsViewModel>();
            builder.Services.AddSingleton<StatisticsView>();

            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<ShopViewModel>();
            builder.Services.AddSingleton<ShopView>();

            builder.Services.AddSingleton<PaymentView>();
            builder.Services.AddSingleton<PaymentViewModel>();

            builder.Services.AddSingleton<ThemeView>();
            builder.Services.AddSingleton<ThemeViewModel>();

            builder.Services.AddSingleton<PaymentView>();
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList =
                    ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
