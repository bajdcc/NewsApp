namespace NewsApp.Machine.State
{
    public interface IState
    {
        void OnCancel();
        void OnMessage(Message msg);
        void OnStart();
        void OnStateEnter();
        void OnTimer();
    }
}