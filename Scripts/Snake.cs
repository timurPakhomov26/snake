using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(MeshRenderer))]
public class Snake : MonoBehaviour
{
  [Range(0,4),SerializeField] private float _bonesDistance;
  [Range(-4,0),SerializeField] private float _speed;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private TextMeshProUGUI _foodCounter;
    [SerializeField] private List<Transform> _tails;
    [SerializeField] private GameObject _bonePrefab;  
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Transform _transformLookTo;
    [SerializeField] private TextMeshProUGUI _krystallCounter;

    private Transform _transform;
    private float _food;
    private  float _foodValue = 0f;
    private int _krystallValue = 0;
    private bool _activeFever = false;
    private MeshRenderer _snakeMeshRenderer;
    private const float _screenWidth = 1f;
    private bool _contactWithRightBoard ;
    private bool _contactWithLeftBoard ;
    



    private void Start()
    {
        _transform = GetComponent<Transform>();
        _snakeMeshRenderer = GetComponent<MeshRenderer>();
        _contactWithLeftBoard = false;
        _contactWithRightBoard = false;
    }

    private void FixedUpdate()
    {
        ColorChange();       

        if (_krystallValue < 3)
        {
            if(_contactWithLeftBoard==false && _contactWithRightBoard == false)
            {
              MoveSnake(_transform.position+transform.forward *_speed);
            }
            else
            {
                MoveSnake(_transform.position + transform.forward * _speed / 1.4f);
            }

            if (Input.GetMouseButton(0))
              {
                Rotate();
              }       
        }
        else
        {
         StartCoroutine(Fever());
        }


         _losePanel.SetActive(false);
              
          _foodCounter.text = _food.ToString();
        _krystallCounter.text = _krystallValue.ToString();
    }

    
       private void MoveSnake(Vector3 newPosition)
       {
           var sqrDistance = _bonesDistance * _bonesDistance;
           Vector3 previusPosition = _transform.position;
           foreach (var bone in _tails)
           {
               if ((bone.position - previusPosition).sqrMagnitude > sqrDistance)
               {
                   var temp = bone.position;
                   bone.position = previusPosition;
                   previusPosition = temp;
               }
               else
               {
                   break;
                   
               }
           }
           _transform.position = newPosition;
       }

       private void OnTriggerEnter(Collider other)
       {
        
           if (other.gameObject.CompareTag("food"))
           {
               Destroy(other.gameObject);
            _food++;
            _foodValue += 0.5f;
            if (_foodValue >= 0.9f)
            {
                var bone = Instantiate(_bonePrefab);
                _tails.Add(bone.transform);
            
                _foodValue = 0f;
            }
           }
            
        if (other.gameObject.CompareTag("BoardLeft"))
        {
            _transform.rotation = Quaternion.identity;
            Debug.Log("IsLeft");
            _contactWithLeftBoard = true;           
            
        }
        if (other.gameObject.CompareTag("BoardRight"))
        {
            _transform.rotation = Quaternion.identity;
            Debug.Log("IsRight");
            _contactWithRightBoard = true;

        }
       
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (_activeFever == false)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            else
            {
                Destroy(other.gameObject);
            }
        }

       if (other.gameObject.CompareTag("Krystal"))

        {          
            Destroy(other.gameObject);
            _krystallValue++;    
        }
       
       }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BoardLeft"))
        {
            Debug.Log("IsNotLeft");
            _contactWithLeftBoard = false;

        }
        if (other.gameObject.CompareTag("BoardRight"))
        {
            _transform.rotation = Quaternion.identity;
            Debug.Log("IsNotRight");
            _contactWithRightBoard = false;

        }
    }

    IEnumerator Fever()
    {
        _activeFever = true;
        MoveSnake(new Vector3(_transform.position.x,0.5f,_transform.position.z) + Vector3.forward * 2 *_speed);
        yield return new WaitForSeconds(2.5f);
        _krystallValue = 0;
        _activeFever = false;
    }


    private void ColorChange()
    {
        foreach(var tail in _tails)
        {
             tail.GetComponent<MeshRenderer>().material.color = _snakeMeshRenderer.material.color;           
        }
    }

    private void Rotate()
    {
      
            Vector3 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            //  Debug.Log(mouse.x);
            Vector3 difference = mouse;


            var rotateZ = Mathf.Atan2(difference.y, difference.z) * Mathf.Rad2Deg * Time.deltaTime;          
            if (mouse.x > _screenWidth / 2f && _contactWithRightBoard==false)
            {                               
                _transform.Rotate(0f, rotateZ, 0f);

            }
              else if (mouse.x <= _screenWidth/2f && _contactWithLeftBoard==false)
            {
                _transform.Rotate(0f, -rotateZ, 0f);

            }
        
    }
   
}


