using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    [CreateAssetMenu(menuName = "Anthill/Tests/Create HexFactory", fileName = "HexFactory", order = 56)]
    public class HexFactory : ScriptableObject
    {
        [SerializeField] private HexMesh _hexMesh;
        
        public IHex Create()
        {
            return new EmptyHex(_hexMesh);
        }
    }
}