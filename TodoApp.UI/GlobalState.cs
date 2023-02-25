namespace TodoApp.UI
{
    public class GlobalState
    {
        public event Action OnChange = default!;
        public readonly List<string> MessegesFirst = new();
        public readonly List<string> MessagesSecond = new();

        public void SendMessageToFirstChat(string message)
        {
            MessegesFirst.Add(message);
            NotifyStateChanged();
        }

        public void SendMessageToSecondChat(string message)
        {
            MessagesSecond.Add(message);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
