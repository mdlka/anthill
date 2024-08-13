using System.Collections;
using TMPro;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTextMeshPro : MonoBehaviour
    {
        [SerializeField] private LocalizedText[] _localizedTexts;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GamePlatformSdkContext.Current.Initialized);
            
            var target = GetComponent<TMP_Text>();
            target.text = _localizedTexts.SelectCurrentLanguageText();
        }
    }
}
