using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.Anthill.Core.Ants;
using YellowSquad.Anthill.UserInput;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Tutorial
{
    public class TutorialRoot : MonoBehaviour
    {
        [SerializeField] private TutorialArrow _uiTutorialArrow;
        [SerializeField] private TutorialArrow _worldTutorialArrow;
        [SerializeField] private float _targetHexPositionOffsetY;
        [SerializeField] private SerializedAxialCoordinate _targetHexPosition;

        private Transform _targetHexPoint;
        private TutorialClickCommand _clickCommand;
        private TutorialButton _buyDiggersButton;
        private TutorialButton _buyLoadersButton;
        private IAnthill _anthill;

        public IClickCommand CreateTutorialCommand()
        {
            return _clickCommand ??= new TutorialClickCommand(_targetHexPosition);
        }

        public IEnumerator StartTutorial(float mapScale, IAnthill anthill, Button buyDiggersButton, Button buyLoadersButton)
        {
            _targetHexPoint = new GameObject("TargetHexPoint").transform;
            _targetHexPoint.position = ((AxialCoordinate)_targetHexPosition).ToVector3(mapScale) + Vector3.up * _targetHexPositionOffsetY;
            _targetHexPoint.SetParent(transform);

            _anthill = anthill;
            _buyDiggersButton = new TutorialButton(buyDiggersButton);
            _buyLoadersButton = new TutorialButton(buyLoadersButton);
            
            yield return Tutorial();
        }
        
        private IEnumerator Tutorial()
        {
            _buyLoadersButton.Disable();
            _buyDiggersButton.Enable();
            
            _uiTutorialArrow.ChangeTarget(_buyDiggersButton.Transform);
            yield return new WaitUntil(() => _anthill.Diggers.CurrentCount >= 5);
            _buyDiggersButton.Disable();
            _uiTutorialArrow.Disable();
            
            _worldTutorialArrow.ChangeTarget(_targetHexPoint);
            yield return new WaitUntil(() => _clickCommand.TargetHexClicked);
            _worldTutorialArrow.Disable();
            
            yield return new WaitUntil(() => _anthill.CanAddLoader);
            
            _buyLoadersButton.Enable();
            _uiTutorialArrow.ChangeTarget(_buyLoadersButton.Transform);
            yield return new WaitUntil(() => _anthill.Loaders.CurrentCount >= 5);
            _uiTutorialArrow.Disable();
            
            _buyDiggersButton.Enable();
        }
    }
}
