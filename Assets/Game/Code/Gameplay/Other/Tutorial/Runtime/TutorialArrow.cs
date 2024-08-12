using UnityEngine;

namespace YellowSquad.Anthill.Tutorial
{
    internal class TutorialArrow : MonoBehaviour
    {
        private void Awake()
        {
            Disable();
        }

        public void ChangeTarget(Transform target)
        {
            gameObject.SetActive(true);
            
            transform.SetParent(target);
            transform.localPosition = Vector3.zero;
        }
        
        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}