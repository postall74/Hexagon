using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if NEW_INPUT_SYSTEM_INSTALLED
using UnityEngine.InputSystem.UI;
#endif

/// <summary>
/// Простой класс для запуска хоста/клиента.
/// </summary>
public class NetworkUI : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    [Tooltip("Кнопка запуска хоста")]
    private Button _startHostButton;

    [SerializeField]
    [Tooltip("Кнопка запуска клиента")]
    private Button _startClientButton;
    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Определение системы ввода и наличие системы событий для UI.
    /// </summary>
    private void Awake()
    {
        if (FindAnyObjectByType<EventSystem>() == false)
        {
            var inputType = typeof(StandaloneInputModule);
#if ENABLE_INPUT_SYSTEM && NEW_INPUT_SYSTEM_INSTALLED
            inputType = typeof(InputSystemUIInputModule);                
#endif
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), inputType);
            eventSystem.transform.SetParent(transform);
        }
    }

    /// <summary>
    /// Инициализация кнопок и подписывание их на события запуска Хоста и Клиента.
    /// </summary>
    private void Start()
    {
        _startHostButton.onClick.AddListener(StartHost);
        _startClientButton.onClick.AddListener(StartClient);
    }
    #endregion

    #region Privete Methods

    /// <summary>
    /// Запуск в режиме Хоста (сервер + клиент).
    /// </summary>
    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        DeactivateButtons();
        Debug.Log("[NetworkUI] Host started.");
    }

    /// <summary>
    /// Запуск в режиме Клиента (подключение к хосту).
    /// </summary>
    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        DeactivateButtons();
        Debug.Log("[NetworkUI] Client started. Connecting to localhost...");
    }

    /// <summary>
    /// Установка кнопок в режим недоступности для повторного нажатия.
    /// </summary>
    private void DeactivateButtons()
    {
        _startClientButton.interactable = false;
        _startHostButton.interactable = false;
    }
    #endregion
}