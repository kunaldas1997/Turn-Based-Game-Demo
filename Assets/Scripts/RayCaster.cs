using System;
using TMPro;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    [SerializeField]
    private GameObject dataPanel;

    [SerializeField]
    private TextMeshProUGUI blockNameText;

    [SerializeField]
    private TextMeshProUGUI blockTypeText;

    /// <summary>
    /// A Simple Raycast code to detect the blocks and return their type from their respective Scriptable Object.
    /// </summary>
    void Update()
    {
        Ray pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(pointerRay, out RaycastHit hit))
        {
            BlockScript block = hit.collider.GetComponent<BlockScript>();
            if (block)
            {
                BlockDataInfo blockData = block.GetInfo();

                dataPanel.SetActive(true);
                blockNameText.text = blockData.blockName;
                blockTypeText.text = blockData.blockType;
            }
        }
    }
}
