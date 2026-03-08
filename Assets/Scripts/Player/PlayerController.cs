using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Контроллер игрока для сетевой ARPG.
/// Обрабатывает ввод, движение и базовую синхронизацию состояния.
/// </summary>
/// <remarks>
/// Наследуется от NetworkBehaviour для доступа к сетевым методам.
/// Движение выполняется только на стороне владельца (клиента).
/// </remarks>
public class PlayerController : NetworkBehaviour
{
    #region Serialized Fields

    /// <summary>Скорость перемещения игрока (единиц в секунду).</summary>
    [SerializeField]
    [Tooltip("Базовая скорость движения персонажа")] 
    private float _moveSpeed = 5f;

    /// <summary>Сила прыжка (если будет добавлен).</summary>
    [SerializeField]
    [Tooltip("Сила вертикального импульса при прыжке")]
    private float _jumpForce = 5f;
    #endregion
    
    #region Private Fields

    private Vector2 _input;
    private CharacterController _controller;
    private bool _isGrounded;
    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Инициализация компонентов.
    /// Вызывается один раз при создании объекта.
    /// </summary>
    private void Awake()
    {
        if (TryGetComponent<CharacterController>(out _controller) == false)
            Debug.LogError($"[{name}] CharacterController not found! Please add it to the player prefab.");
    }

    /// <summary>
    /// Сбор ввода игрока.
    /// Выполняется каждый кадр, но применяется только для локального владельца.
    /// </summary>
    private void Update()
    {
        // Ввод обрабатывается только на клиенте, которому принадлежит этот игрок
        if (IsOwner == false) 
            return;

        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");

        // TODO: Добавить обработку прыжка и атаки
        // if (Input.GetButtonDown("Jump")) TryJump();
        // if (Input.GetMouseButtonDown(0)) TryAttack();
    }

    /// <summary>
    /// Физическое перемещение.
    /// Использует FixedUpdate для стабильной синхронизации с физикой.
    /// </summary>
    private void FixedUpdate()
    {
        if (IsOwner == false || _controller == null)
            return;

        // Преобразуем 2D ввод в 3D вектор движения (по плоскости XZ)
        Vector3 moveDirection = new Vector3(_input.x, 0f, _input.y);
        // Применяем движение с учётом времени и скорости
        _controller.Move(moveDirection * _moveSpeed * Time.fixedDeltaTime);
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Попытка выполнить прыжок
    /// </summary>
    /// <returns>True, если прыжок был выполнен.</returns>
    public bool TryJump()
    {
        if (_isGrounded == false) 
            return false;

        // TODO: Реализовать прыжок через Rigidbody.AddForce или CharacterController
        return true;
    }
    #endregion
}