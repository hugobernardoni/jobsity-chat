namespace JobSity.API.Services.Abstract
{
    public interface IChatService
    {
        bool IsCommand(string message);

        string GetValidCommandMessage(string message);
    }
}
