using TMPro;
using UnityEngine;

public class Toast : MonoBehaviour
{
    public static Toast Instance;
    [SerializeField] private GameObject toastUI;
    [SerializeField] private TMP_Text toastText;
    
    private void Awake()
    {
        //Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
    }

    void Start()
    {
        toastUI.SetActive(false);
    }

    public void ShowToast(string textValue)
    {
        toastUI.SetActive(true);
        toastText.SetText(textValue);
    }

    public void HideToast()
    {
        toastUI.SetActive(false);
    }
    
}
