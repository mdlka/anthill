using System;
using YellowSquad.Anthill.Core.Ants;

namespace YellowSquad.Anthill.Meta
{
    internal class IncreaseSpeedCommand : IButtonCommand
    {
        private readonly ISession _session;
        private readonly UpgradeAntMoveDurationList _upgradeAntMoveDurationList;

        public IncreaseSpeedCommand(ISession session, UpgradeAntMoveDurationList upgradeAntMoveDurationList)
        {
            _session = session;
            _upgradeAntMoveDurationList = upgradeAntMoveDurationList;
        }

        public bool CanExecute => _upgradeAntMoveDurationList.HasNext;
        
        public void Execute()
        {
            if (CanExecute == false)
                throw new InvalidOperationException();
            
            _upgradeAntMoveDurationList.Next();
            _session.ChangeAntsMoveDuration(_upgradeAntMoveDurationList.CurrentValue);
        }
    }
}