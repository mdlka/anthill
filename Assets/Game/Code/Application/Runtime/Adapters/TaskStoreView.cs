using UnityEngine;

namespace YellowSquad.Anthill.Application.Adapters
{
    public class TaskStoreView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _sellPartEffect;
        
        public void Render(Vector3 sellPosition)
        {
            _sellPartEffect.transform.position = sellPosition;
            _sellPartEffect.Play();
        }
    }
}