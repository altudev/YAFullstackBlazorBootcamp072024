using Blazored.LocalStorage;

namespace ChatGPTClone.WasmClient.Services;

public class ThemeService
{
    private readonly ILocalStorageService _localStorageService;

    public ThemeService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public event Action OnChange;

    public string CurrentTheme { get; private set; } = "light";

    public async Task InitializeThemeAsync()
    {
        var theme = await _localStorageService.GetItemAsync<string>("theme");

        if (string.IsNullOrEmpty(theme))
        {
            await _localStorageService.SetItemAsync("theme", "light");
        }

        CurrentTheme = theme ?? "light";

        NotifyThemeChanged();
    }

    public async Task SetThemeAsync(string theme)
    {
        await _localStorageService.SetItemAsync("theme", theme);

        CurrentTheme = theme;

        NotifyThemeChanged();
    }

    private void NotifyThemeChanged() => OnChange?.Invoke();
}
