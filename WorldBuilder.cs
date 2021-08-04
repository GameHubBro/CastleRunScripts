using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public static WorldBuilder instance;

    public GameObject[] freePlatforms;                                      //массив свободных платформ
    public GameObject[] obstaclePlatforms;                                  //массив платформ с препятствиями
    public GameObject[] objects;                                            //массив обычных объектов
    public GameObject[] objectsToTake;                                      //массив бонусов

    public Transform platformContainer;                                     //позиция platformContainer

    private bool isObstacle;                                                //переменная определяющее была ли последняя платформа с препятствиями

    private Transform lastPlatform = null;                                  //позиция последней платформы

    private int randomMax = 10;                                             //число для расчета случайности
    private int countOfPlatforms = 0;                                                  //счетчик созданных платформ
    private int countToNextLevel = 30;                                      //счетчик созданных платформ для перехода на новый уровень
       
    [SerializeField] private int instProbabilityForObjToTake = 3;           //переменная для расчета вероятности появления бонусов
    [SerializeField] private int instProbabilityForObj = 2;                 //переменная для расчета вероятности появления обычных объектов   
    [SerializeField] private int countToNextLevelRaiser = 30;               //переменная для наращивания колва платформ для следующего уровня
    [SerializeField] private float speedRaiser = 0.5f;                      //прирост скорости платформ с каждым уровнем
    [SerializeField] private int hardPlatformsCount = 3;
    [SerializeField] private float maxSpeedOfPlatforms = 9f;

    private void Awake()
    {
        if (WorldBuilder.instance != null)                       //если есть другой объект класс WorldController, то удаляем его
        {
            Destroy(gameObject);
            return;
        }
        WorldBuilder.instance = this;
    }

    void Start()
    {
        Init();
    }

    public void Init()                                                      //инициалицизируем начальный состав платформ
    {
        for (int i = 0; i < 5; i++)
        {
            CreateFreePlatform();
        }
        for (int i = 0; i < 15; i++)
        {
            CreatePlatform();          
        }
    }
    public void CreatePlatform()                                            //создаем платформу
    {
        if (isObstacle)
        {
            CreateFreePlatform();
        }
        else
        {
            CreateObsctaclePlatform();
        }

        int random0 = Random.Range(0, randomMax);
        if (random0 % instProbabilityForObj == 0)
        {
            CreateObject();
        }

        int random = Random.Range(0, randomMax);
        if (random % instProbabilityForObjToTake == 0)
        {
            CreateObjectToTake();
        }

        countOfPlatforms++;

        if (countOfPlatforms == countToNextLevel && WorldController.instance.speedOfPlatforms < maxSpeedOfPlatforms)                  //если число платформ равно числу платформ для повышения уровня и скорость платформ меньше максимальной, то повышаем скорость платформ
        {
            levelChecker();
        }
        Debug.Log(countOfPlatforms);
    }

    private void levelChecker()                                             //метод повышения скорости платформ
    {
        WorldController.instance.speedOfPlatforms += speedRaiser;
        countToNextLevel += countToNextLevelRaiser;
        if (hardPlatformsCount > 1)
        {
            hardPlatformsCount--;
        }
        Debug.Log($"speed = {WorldController.instance.speedOfPlatforms}, countToNextLevel = {countToNextLevel}");
    }

    private void CreateFreePlatform()                                       //метод создания свободной платформы
    {
        Vector3 pos = (lastPlatform == null) ?
            platformContainer.position :
            lastPlatform.GetComponent<PlatformController>().endPoint.position;

        int index = Random.Range(0, freePlatforms.Length);
        GameObject res = Instantiate(freePlatforms[index], pos, Quaternion.identity, platformContainer);
        lastPlatform = res.transform;
        isObstacle = false;
    }

    private void CreateObsctaclePlatform()                                  //метод создания платформы с препятствиями
    {
        Vector3 pos = (lastPlatform == null) ?
            platformContainer.position :
            lastPlatform.GetComponent<PlatformController>().endPoint.position;

        int index = Random.Range(0, obstaclePlatforms.Length - hardPlatformsCount);
        GameObject res = Instantiate(obstaclePlatforms[index], pos, Quaternion.identity, platformContainer);
        lastPlatform = res.transform;
        isObstacle = true;
    }

    private void CreateObject()                                             //метод создания обычных объектов
    {
        Vector3 pos = (lastPlatform == null) ?
            platformContainer.position :
            lastPlatform.GetComponent<PlatformController>().endPoint.position;

        Vector3 tempPos = pos;

        Quaternion rotate = Quaternion.identity;
        
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            tempPos.x = 4.5f;
            rotate = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            tempPos.x = -4.5f;        
        }
        
        tempPos.y = 0f;
        tempPos.z = pos.z - 2f;

        int index = Random.Range(0, objects.Length);
        Instantiate(objects[index], tempPos, rotate, lastPlatform);
    }

    private void CreateObjectToTake()                                       //метод создания бонусов
    {
        Vector3 pos = (lastPlatform == null) ?
            platformContainer.position :
            lastPlatform.GetComponent<PlatformController>().endPoint.position;

        Vector3 tempPos = pos;
        int index = Random.Range(0, objectsToTake.Length);
        int randomX = Random.Range(0, 3);
        switch (randomX)
        {
            case 0:
                tempPos.x = -PlayerController.distance;
                break;
            case 1:
                tempPos.x = 0;
                break;
            case 2:
                tempPos.x = PlayerController.distance;
                break;
        }
        tempPos.y = 2f;
        tempPos.z = pos.z - 2f;
    
            Instantiate(objectsToTake[index], tempPos, Quaternion.identity, lastPlatform);      
    }
}
