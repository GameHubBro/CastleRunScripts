using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private Animator animator;

    private float dir;                                                          //переменная для обозначения направления движения
    private float currentDir;                                                   //переменная для обозначения направления движения
    private float speed;                                                        //скорость перемещения по дистанции
    private float speedAcceleration = 1.5f;                                     //коэф. увеличения скорости перемещения
    private float timeReduce = 2f;                                              //уменьшение времени движения налево или направо
    private float impulseForce = 7.5f;                                          //сила импулься персонажа
    private float currentDistance;                                              //переменная для плавной смены положения

    public float time;                                                          //время за которое происходит движение влево или вправо    

    private Vector3 moveUpDown = Vector3.zero;                                  //вектор для подъёма и падения

    private bool wishJump = false;                                              //показывает нажат ли пробел для прыжка
    private bool isRunning = true;                                              //показывает бежит ли персонаж
    private bool isDead = false;                                                //показывает мертв ли персонаж

    private int roadCount;                                                      //показывает номер дорожки на которой на данный момент находится персонаж
    private int countCoins = 0;                                                 //счетчик монет

    [SerializeField] private AnimationClip anim;                                //клип с перекатом влево-вправо
    [SerializeField] private int roads = 3;                                     //общее кол-во дорожек
    [SerializeField] private int roadStartPos = 2;                              //в инспекторе устанавливаем стартовую дорожку
    [SerializeField] private float jumpSpeed = 0.12f;                           //скорость прыжка
    [SerializeField] private float gravity = 0.005f;                            //скорость падения
    [SerializeField] private ParticleSystem psLightWhenTake;                    //система частиц свечения персонажа при заборе монеты
    [SerializeField] private ParticleSystem psLightWhenTakeSlowPo;              //система частиц свечения персонажа при заборе замедляющего поушена

    [SerializeField] private AudioSource coinSound;
    [SerializeField] private AudioSource potionSound;
    [SerializeField] private AudioSource smashSound;

    public static float distance = 2f;                                          //дистанция на которую персонаж передвигается во время движения налево или направо  

    private Rigidbody[] bodyParts;                                              //массив всех твердых тел персонажа

    private void Awake()
    {
        cc = GetComponent<CharacterController>();                               //ищем и записываем в переменную компоненты
        animator = GetComponent<Animator>();
        bodyParts = GetComponentsInChildren<Rigidbody>();

        RigidBodyisKinematicOn();                                               //делаем все Rigidbody персонажа кинематичными
    }

    void Start()
    {
        time = anim.length / timeReduce;                                        //устанавливаем время перемещения равное длине анимации
        speed = distance / time;                                                //находим скорость
        speed *= speedAcceleration;                                             //увеличиваем скорость перемещения
        roadCount = roadStartPos;                                               //устанавливаем текужую дорожку в стартовую
    }

    void Update()
    {
        dir = Input.GetAxisRaw("Horizontal");                                   //записываем в переменную нажатие клавиш

        if (dir == -1 && roadCount == roads || dir == 1 && roadCount == 1 || roadCount == roadStartPos)  //если не хотим выйти из крайних дороже то заходим в условие
        {
            if (dir != 0 && isRunning && cc.isGrounded && !isDead)                         //перемещаем персонажа
            {
                currentDir = dir;
                if (currentDir > 0)
                {
                    animator.SetTrigger("Right");
                    roadCount++;
                }
                else if (currentDir < 0)
                {
                    animator.SetTrigger("Left");
                    roadCount--;
                }
                currentDistance = distance;
                isRunning = false;
                StartCoroutine(Moving());
            }
        }

        wishJump = Input.GetKeyDown(KeyCode.Space);                                 //присваиваем true если нажата клавиша Space

        if (cc.velocity.y < -0.5f && !cc.isGrounded && !isDead)                            //проверяем падает ли персонаж
        {
            animator.SetBool("Falling", true);
        }
        else if (cc.isGrounded && !isDead)
        {
            animator.SetBool("Falling", false);
        }

        moveUpDown.y = cc.isGrounded ? -0.01f : moveUpDown.y - gravity * Time.deltaTime;        //если персонаж находится на земле, то прикладываем небольшую гравитацию, если не на земле, то обычную гравитацию


        if (wishJump && isRunning && cc.isGrounded && !isDead)                             //прыгаем если хотим
        {
            Jumping();
        }

        if (!isDead)
        {
            cc.Move(moveUpDown);
        }
    }

    private void Jumping()                                                          //описываем прыжок
    {
        animator.SetTrigger("Jump");
        moveUpDown.y = jumpSpeed;
    }

    IEnumerator Moving()                                                        //перемещение персонажа
    {
        while (currentDistance > 0)
        {
            yield return new WaitForEndOfFrame();
            float tempDist = speed * Time.deltaTime;
            tempDist = Mathf.Clamp(tempDist, 0, currentDistance);
            cc.Move(Vector3.right * currentDir * tempDist);
            currentDistance -= tempDist;
        }
        Debug.Log($"you are on road#{roadCount}");
        isRunning = true;
    }


    private void OnTriggerEnter(Collider other)                               //определяем был ли удар о препятствие
    {
        if (other.CompareTag("Danger"))
        {
            Die();                                                              //если был удар о препятствие, то персонаж умирает
        }

        if (other.CompareTag("ObjToTake"))
        {
            psLightWhenTake.Play();                                             //если персонаж взял бонус, то он светится
            coinSound.Play();
            countCoins++;
            StartCoroutine(WaitToCollect(0.5f));                                //запускаем курутину сбора монет с задержкой 0.5 сек
            Debug.Log($"You've collected {countCoins} coins");
        }

        if(other.CompareTag("SlowPotion"))                                      //если персонаж взял замедляющий поушн и если скорость платформ больше минимальной, то уменьшаем скорость платформ и включаем партикл
        {
            if (WorldController.instance.speedOfPlatforms > 6f)
            {
                psLightWhenTakeSlowPo.Play();
                potionSound.Play();
                WorldController.instance.speedOfPlatforms -= 0.5f;
                Debug.Log($"Speed of platforms was reduced for 0.5f");
            }
        }
    }

    private void RigidBodyisKinematicOn()                                       //метод включения всех rb персонажа кинематичными
    {
        foreach (Rigidbody body in bodyParts)
        {
            body.isKinematic = true;
        }
    }

    private void RigidBodyisKinematicOff()                                      //метод который делает все rb персонажа не кинематичными
    {
        foreach (Rigidbody body in bodyParts)
        {
            body.isKinematic = false;
        }
    }

    private void Die()                                                         //метод при смерти
    {
        isDead = true;
        impulseForce = WorldController.instance.speedOfPlatforms;               //присваиваем импульсу значение скорости платформы

        WorldController.instance.speedOfPlatforms = 0f;                        //останавливаем платформы

        animator.enabled = false;                                               //выключаем аниматор
        cc.enabled = false;                                                     //выключаем Character Controller
        RigidBodyisKinematicOff();                                              //вызываем метод делаюий все rb персонажа не кинематичными
        RigidBodyImpulseApple();                                                //придаем импульс персонажу
        UIGameManager.instance.scoreText.text = $"Your score {UIGameManager.instance.score.text} coins";
        StartCoroutine(UIGameManager.instance.WaitToShowLosePanel(1));
        smashSound.Play();
    }

    private void RigidBodyImpulseApple()                                        //метод импулься персонажа
    {
        foreach (Rigidbody body in bodyParts)
        {
            body.AddForce(Vector3.forward * impulseForce, ForceMode.VelocityChange);
        }
    }

    IEnumerator WaitToCollect(float delay)                                      //курутина задержки сбора монет
    {
        yield return new WaitForSeconds(delay);
        UIGameManager.instance.score.text = countCoins.ToString();
    }
}
