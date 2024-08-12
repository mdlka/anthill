using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
        private TutorialButton[] _targetButtons;

        public IClickCommand CreateTutorialCommand()
        {
            return _clickCommand ??= new TutorialClickCommand(_targetHexPosition);
        }

        public void StartTutorial(float mapScale, Button[] targetButtons)
        {
            _targetHexPoint = new GameObject("TargetHexPoint").transform;
            _targetHexPoint.position = ((AxialCoordinate)_targetHexPosition).ToVector3(mapScale) + Vector3.up * _targetHexPositionOffsetY;
            _targetHexPoint.SetParent(transform);

            _targetButtons = new TutorialButton[targetButtons.Length];
            
            for (int i = 0; i < targetButtons.Length; i++)
                _targetButtons[i] = new TutorialButton(targetButtons[i]);

            StartCoroutine(Tutorial());
        }
        
        private IEnumerator Tutorial()
        {
            foreach (var targetButton in _targetButtons)
                targetButton.Disable();
            
            foreach (var targetButton in _targetButtons)
            {
                targetButton.Enable();
                _uiTutorialArrow.ChangeTarget(targetButton.Transform);
                yield return new WaitUntil(() => targetButton.ClickCount >= 5);
                targetButton.Disable();
            }

            _uiTutorialArrow.Disable();

            _worldTutorialArrow.ChangeTarget(_targetHexPoint);
            yield return new WaitUntil(() => _clickCommand.TargetHexClicked);
            _worldTutorialArrow.Disable();

            foreach (var targetButton in _targetButtons)
            {
                targetButton.Enable();
                targetButton.Dispose();
            }
        }
    }
}
