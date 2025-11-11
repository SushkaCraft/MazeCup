using UnityEngine;
using UnityEditor;
using TMPro;

public class ProjectInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text? _companyName;
    [SerializeField] private TMP_Text? _productName;
    [SerializeField] private TMP_Text? _productVersion;

    private void Start()
    {
        _companyName.text = $"©{PlayerSettings.companyName}";
        _productName.text = $"{PlayerSettings.productName}";
        _productVersion.text = $"V.{PlayerSettings.bundleVersion}";
    }
}
