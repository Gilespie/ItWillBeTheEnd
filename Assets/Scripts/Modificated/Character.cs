using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] bool _isAlive = true;

    [SerializeField] CharacterAnimationController _animationController;
    [SerializeField] CharacterInputController _inputController;
    [SerializeField] GroundRaycast _groundRaycast;
    [SerializeField] SlopeRaycast _slopeRaycast;
    [SerializeField] InteractRaycast _interactRaycast;
    [SerializeField] CommonMovement _walk;
    [SerializeField] SwimmMovement _swimm;
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _walk.Initialize(_rb);
        _swimm.Initialize(_rb);
        _animationController.SetTirggerStartSwim();
    }

    void Update()
    {
        _inputController.ArtificialUpdate();

        _groundRaycast.IsRaycasting(-transform.up);
        _interactRaycast.IsRaycasting(transform.forward);
        _slopeRaycast.IsRaycasting(-transform.up);

        _animationController.SetBoolMove(_inputController.Direction.sqrMagnitude > 0.01f);
        _animationController.SetFloatXAxis(_inputController.Direction.x);
        _animationController.SetFloatYAxis(_inputController.Direction.y);
        _animationController.SetFloatZAxis(_inputController.Direction.z);
    }

    void FixedUpdate()
    {
        //_walk.Advance(_inputController.Direction);
        _swimm.Advance(_inputController.Direction);
    }

    public void InstantKill(params object[] parameters)
    {
        _isAlive = false;
        this.enabled = false;
        EventManager.Trigger(EventType.OnDead, _isAlive);
    }
}