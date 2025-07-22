namespace Frontend.Components.Alert;

public class AlertService
{
    public event Action<AlertMessage>? OnShow;
    public event Action? OnHide;

    public void ShowAlert(string message, string type)
    {
        OnShow?.Invoke(new AlertMessage { Message = message, Type = type });
    }

    public void HideAlert()
    {
        OnHide?.Invoke();
    }
}