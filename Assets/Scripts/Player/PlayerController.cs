using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    private Vector2 _input;
    private CharacterController _controller;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //Клиентский ввод - только для локального игрока
        if (IsOwner == false) 
            return;

        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (IsOwner == false)
            return;

        Vector3 move = new Vector3(_input.x, 0, _input.y) * _moveSpeed * Time.fixedDeltaTime;
        _controller.Move(move);
    }
}