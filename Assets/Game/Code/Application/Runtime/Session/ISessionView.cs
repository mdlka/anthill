namespace YellowSquad.Anthill.Application
{
    public interface ISessionView
    {
        void RenderLoadersCount(int currentValue, int maxValue);
        void RenderDiggersCount(int currentValue, int maxValue);
        void RenderAntMoveDuration(float value, float maxMoveDuration);
    }
}