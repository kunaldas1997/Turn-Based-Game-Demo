using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private Image closeImage;

    [SerializeField]
    private GameObject infoPanel;

    /// <summary>
    /// A simple script to close the block description panel.
    /// </summary>
    void Start()
    {
        closeImage.GetComponent<Button>().onClick.AddListener(() =>
        {
            ClosePanel();
        });
    }

    private void ClosePanel()
    {
        infoPanel.SetActive(false);
    }
   
}
