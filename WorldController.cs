using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public float speedOfPlatforms;                                  //скорость платформ

    public WorldBuilder worldBuilder;                               //переменная класса WorldBuilder
    public float minZ = -10f;                                       //точка Z за которой уничтожаются платформы


    public delegate void TryToDelAndAddPlatform();                  //делегат для удаления и удаления платформ
    public event TryToDelAndAddPlatform OnPlatformMovement;         //ивент для удаления и удаления платформ

    public static WorldController instance;                         //статическая переменная класса для доступа из любой точки кода

    private void Awake()
    {
        if (WorldController.instance != null)                       //если есть другой объект класс WorldController, то удаляем его
        {
            Destroy(gameObject);
            return;
        }
        WorldController.instance = this;                            //этот объект делаем объектом класса WorldController
    }

    private void OnDestroy()                                        //записываем в переменную instance null
    {
        WorldController.instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OnPlatformMovementCoroutine());              //запускаем корутину ивента делегата
    }

    // Update is called once per frame
    void Update()                                                   //двигаем платформы
    {
        transform.position -= Vector3.forward * speedOfPlatforms * Time.deltaTime;      
    }

    IEnumerator OnPlatformMovementCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);                    //через каждую секунду выполняем ивент
            if (OnPlatformMovement != null)
            {
                OnPlatformMovement();
            }
            
        }
    }
}
