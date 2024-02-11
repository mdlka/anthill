namespace YellowSquad.Anthill.Meta
{
    public interface IButtonCommand
    {
        bool CanExecute { get; }
        void Execute();
    }
}