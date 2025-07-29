using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; 
using System.Collections.Generic;

public class LoginInputTab : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    [Header("Login Button")]
    public Button loginButton;
    public Button registerButton;
    public Button faceIdLoginButton;

    private List<GameObject> tabOrder;

    void Start()
    {
        tabOrder = new List<GameObject>
        {
            emailInput.gameObject,
            passwordInput.gameObject,
            loginButton.gameObject,
            registerButton.gameObject,
            faceIdLoginButton.gameObject
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            int idx = 0;
            if (current != null)
            {
                idx = tabOrder.IndexOf(current);
                idx = (idx + 1) % tabOrder.Count;
            }
            EventSystem.current.SetSelectedGameObject(tabOrder[idx]);
            TMP_InputField input = tabOrder[idx].GetComponent<TMP_InputField>();
            if (input != null)
                input.ActivateInputField();
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            if (current == emailInput.gameObject || current == passwordInput.gameObject)
            {
                if (!string.IsNullOrEmpty(emailInput.text) && !string.IsNullOrEmpty(passwordInput.text))
                {
                    loginButton.onClick.Invoke();
                }
            }
            else if (current == loginButton.gameObject)
            {
                loginButton.onClick.Invoke();
            }
            else if (current == registerButton.gameObject)
            {
                registerButton.onClick.Invoke();
            }
            else if (current == faceIdLoginButton.gameObject)
            {
                faceIdLoginButton.onClick.Invoke();
            }
        }
    }
}