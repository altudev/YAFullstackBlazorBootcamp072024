using Radzen;

namespace PasswordStorageApp.BlazorClient.Services
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
            _notificationService.Notify(NotificationSeverity.Success, detail:message);
        }

        public void Success(string message, string title)
        {
            _notificationService.Notify(NotificationSeverity.Success, summary:title, message);
        }

        public void Warning(string message)
        {
            _notificationService.Notify(NotificationSeverity.Warning, detail:message);
        }

        public void Warning(string message, string title)
        {
            _notificationService.Notify(NotificationSeverity.Warning, summary:title, message);
        }

        public void Info(string message)
        {
            _notificationService.Notify(NotificationSeverity.Info, detail:message);
        }

        public void Info(string message, string title)
        {
            _notificationService.Notify(NotificationSeverity.Info, summary:title, message);
        }

        public void Error(string message)
        {
            _notificationService.Notify(NotificationSeverity.Error, detail:message);
        }

        public void Error(string message, string title)
        {
          _notificationService.Notify(NotificationSeverity.Error, summary:title, message);
        }
    }
}
