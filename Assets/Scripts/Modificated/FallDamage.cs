using UnityEngine;

public class FallDamage : MonoBehaviour
{
    [Header("Falling")]
    [SerializeField] private float _fallDamageMultiplier = 100f;
    [SerializeField] private float _fallDamageThreshold = -10f;
    private float _maxFallSpeed = 0f;
    private float _fallDamage = 0f;
    private bool _wasGrounded = false;

    public void Tick(bool isGrounded, bool isSwimming, bool isSlope, float velocityY)
    {
        if (isSwimming)
        {
            _maxFallSpeed = 0f;
            return;
        }

        if (!isGrounded && !isSwimming)
        {
            if (!isSlope && velocityY < _maxFallSpeed) _maxFallSpeed = velocityY;

            //Debug.Log($"Current fall speed: {velocityY}, Max fall speed: {_maxFallSpeed}");
        }

        if (isGrounded && !_wasGrounded)
        {
            if (_maxFallSpeed < _fallDamageThreshold)
            {
                _fallDamage = Mathf.Abs(_maxFallSpeed + _fallDamageThreshold) * _fallDamageMultiplier;

                //Debug.Log($"Fall damage: {_fallDamage}");

                EventManager.Trigger(EventType.OnFalled);
            }

            _maxFallSpeed = 0f;
        }

        _wasGrounded = isGrounded;
    }
}