using System.Collections;
using UnityEngine;
public class KnifeController : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [Header("Movement")]
    [SerializeField] private float maxPower;
    [SerializeField] private float shootPower;
    [SerializeField] private float gravity = 1;
    [SerializeField] [Range(0f, 1f)] private float slowMotion;

    #endregion

    #region Private Variables

    private readonly bool _shootWhileMoving = true;
    private readonly bool _forwardDraging = false;
    private readonly bool _showLineOnScreen = true;

    private Transform _direction;
    private Rigidbody2D _rb;
    private LineRenderer _line;
    private LineRenderer _screenLine;
    
    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private Vector2 _startMousePos;
    private Vector2 _currentMousePos;
    
    private bool _canShoot = true;
    private bool _isSlow;

    #endregion

    #endregion
    
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _line = GetComponent<LineRenderer>();
        _direction = transform.GetChild(0);
        _screenLine = _direction.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _isSlow = true;
        _rb.gravityScale = gravity;
    }

    private void Update()
    {
        if(_isSlow)
        {
            _rb.velocity /= (1 + slowMotion);
        }

        if (UIManager.Instance.IsActive && UIManager.Instance.canvasPlay.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseClick();
            }
            if (Input.GetMouseButton(0))
            {
                MouseDrag();
            }

            if (Input.GetMouseButtonUp(0))
            {
                MouseRelease();
                if(UIManager.Instance.Sound && UIManager.Instance.Play && !UIManager.Instance.Success)
                {
                    UIManager.Instance.KnifeSound.Play();
                }
                _isSlow = false;
                StartCoroutine(Wait(0.75f));
            }
        }


        if (_rb.velocity.magnitude < 0.7f)
        {
            _rb.velocity = new Vector2(0, 0);
            _canShoot = true;
        }
    }

    private void MouseClick()
    {
        if (_shootWhileMoving)
        {
            var direction = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.right = direction * 1;

            _startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            if (_canShoot)
            {
                var direction = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.right = direction * 1;

                _startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

    }

    private void MouseDrag()
    {
        if (_shootWhileMoving)
        {
            LookAtShootDirection();
            DrawLine();

            if (_showLineOnScreen)
                DrawScreenLine();

            float distance = Vector2.Distance(_currentMousePos, _startMousePos);

            if (distance > 1)
            {
                _line.enabled = true;

                if (_showLineOnScreen)
                    _screenLine.enabled = true;
            }
        }
        else
        {
            if (_canShoot)
            {
                LookAtShootDirection();
                DrawLine();

                if (_showLineOnScreen)
                    DrawScreenLine();

                var distance = Vector2.Distance(_currentMousePos, _startMousePos);

                if (distance > 1)
                {
                    _line.enabled = true;

                    if (_showLineOnScreen)
                        _screenLine.enabled = true;
                }
            }
        }

    }

    private void MouseRelease()
    {
        if (_shootWhileMoving)
        {
            Shoot();
            _screenLine.enabled = false;
            _line.enabled = false;
        }
        else
        {
            if (_canShoot)
            {
                Shoot();
                _screenLine.enabled = false;
                _line.enabled = false;
            }
        }

    }

    private void LookAtShootDirection()
    {
        var direction = _startMousePos - _currentMousePos;

        if (_forwardDraging)
        {
            transform.right = direction * -1;
        }
        else
        {
            transform.right = direction;
        }


        var distance = Vector2.Distance(_startMousePos, _currentMousePos);
        distance *= 4;


        if (distance < maxPower)
        {
            _direction.localPosition = new Vector2(distance / 6, 0);
            shootPower = distance;
        }
        else
        {
            shootPower = maxPower;
            _direction.localPosition = new Vector2(maxPower / 6, 0);
        }

    }
    private void Shoot()
    {
        _canShoot = false;
        _rb.velocity = transform.right * (shootPower * 1.3f);
    }
    
    private void DrawScreenLine()
    {
        
        _screenLine.positionCount = 1;
        _screenLine.SetPosition(0, _startMousePos);


        _screenLine.positionCount = 2;
        _screenLine.SetPosition(1, _currentMousePos);
    }

    private void DrawLine()
    {
        _startPosition = transform.position;

        _line.positionCount = 1;
        _line.SetPosition(0, _startPosition);


        _targetPosition = _direction.transform.position;
        _currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _line.positionCount = 2;
        _line.SetPosition(1, _targetPosition);
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isSlow = true;
    }    
}
