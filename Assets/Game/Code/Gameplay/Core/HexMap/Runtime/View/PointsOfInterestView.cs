using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class PointsOfInterestView : MonoBehaviour
    {
        [SerializeField] private List<PointSetting> _settings;

        public void Render(float mapScale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells)
        {
            foreach (var setting in _settings)
                setting.DividedObjectView?.Clear(); // TODO: Need optimization
            
            foreach (var cell in cells)
            {
                if (cell.Value.PointOfInterestType == PointOfInterestType.Empty)
                    continue;
                
                if (cell.Value.HasDividedPointOfInterest == false)
                    continue;

                var setting = SettingBy(cell.Value.PointOfInterestType);
                setting.DividedObjectView.Render(cell.Value.DividedPointOfInterest.Parts, 
                    PointOfInterestMatrixBy(mapScale, cell.Key, cell.Value.PointOfInterestType));
            }

            foreach (var setting in _settings)
            {
                if (setting.PointOfInterest == PointOfInterestType.Empty)
                    continue;
                
                if (setting.SolidObjectView == null)
                    continue;

                if (setting.SolidObjectView.Rendered)
                    continue;

                var positions = new List<AxialCoordinate>();

                foreach (var cell in cells)
                    if (cell.Value.PointOfInterestType == setting.PointOfInterest)
                        positions.Add(cell.Key);
                
                setting.SolidObjectView.Render(positions, position =>
                    PointOfInterestMatrixBy(mapScale, position, setting.PointOfInterest));
            }
        }
        
        public Matrix4x4 PointOfInterestMatrixBy(float mapScale, AxialCoordinate position, PointOfInterestType type)
        {
            var setting = SettingBy(type);
            return Matrix4x4.TRS(position.ToVector3(mapScale), Quaternion.Euler(setting.Rotation), Vector3.one * setting.SizeFactor);
        }

        private PointSetting SettingBy(PointOfInterestType pointOfInterest)
        {
            return _settings.First(mesh => mesh.PointOfInterest == pointOfInterest);
        }
        
        [Serializable]
        private class PointSetting
        {
            [field: SerializeField] public PointOfInterestType PointOfInterest { get; private set; }
            [field: SerializeField] public SolidObjectView SolidObjectView { get; private set; }
            [field: SerializeField] public DividedObjectView DividedObjectView { get; private set; }
            [field: SerializeField, Min(0f)] public float SizeFactor { get; private set; }
            [field: SerializeField] public Vector3 Rotation { get; private set; }
        }
    }
}