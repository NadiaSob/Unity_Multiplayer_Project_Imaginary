using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CopyJoinCode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CopyCode()
    {
        TextEditor te = new TextEditor();
        te.content = new GUIContent(joinCode.text);
        te.SelectAll();
        te.Copy();
    }
}
