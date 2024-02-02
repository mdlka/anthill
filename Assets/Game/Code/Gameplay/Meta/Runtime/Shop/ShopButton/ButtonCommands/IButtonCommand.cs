namespace YellowSquad.Anthill.Meta
{
    internal interface IButtonCommand
    {
        bool CanExecute { get; }
        void Execute();
    }
}