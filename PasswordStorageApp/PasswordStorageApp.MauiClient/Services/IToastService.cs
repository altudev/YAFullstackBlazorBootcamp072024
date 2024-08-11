namespace PasswordStorageApp.MauiClient.Services
{
    public interface IToastService
    {
        void Success(string message);
        void Success(string message,string title);

        void Warning(string message);
        void Warning(string message, string title);

        void Info(string message);
        void Info(string message, string title);

        void Error(string message);
        void Error(string message, string title);
    }
}
