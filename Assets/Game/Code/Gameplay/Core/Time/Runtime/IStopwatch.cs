namespace YellowSquad.Anthill.Core.GameTime
{
    public interface IStopwatch
    {
        int Create();
        void Restart(int index);

        float ElapsedTime(int index);
    }
}