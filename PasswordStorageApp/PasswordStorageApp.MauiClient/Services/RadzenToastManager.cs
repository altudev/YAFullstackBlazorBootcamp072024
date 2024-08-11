using Radzen;

namespace PasswordStorageApp.MauiClient.Services
{
    public class RadzenToastManager:IToastService
    {
        private readonly NotificationService _notificationService;

        public RadzenToastManager(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void Success(string message)
        {
            _notificationService.Notify(NotificationSeverity.Success, detail:message,duration:10000);
        }

        public void Success(string message, string title)
        {
            _notificationService.Notify(NotificationSeverity.Success, summary:title, message, duration: 10000);
        }

        public void Warning(string message)
        {
            _notificationService.Notify(NotificationSeverity.Warning, detail:message, duration: 10000);
        }

        public void Warning(string message, string title)
        {
            _notificationService.Notify(NotificationSeverity.Warning, summary:title, message, duration: 10000);
        }

        public void Info(string message)
        {
            _notificationService.Notify(NotificationSeverity.Info, detail:message, duration: 10000);
        }

        public void Info(string message, string title)
        {
            _notificationService.Notify(NotificationSeverity.Info, summary:title, message, duration: 10000);
        }

        public void Error(string message)
        {
            _notificationService.Notify(NotificationSeverity.Error, detail:message, duration: 10000);
        }

        public void Error(string message, string title)
        {
          _notificationService.Notify(NotificationSeverity.Error, summary:title, message, duration: 10000);
        }
    }
}
