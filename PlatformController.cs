using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform endPoint;                                    //точка конца платформы

    void Start()
    {
        WorldController.instance.OnPlatformMovement += TryToDelAndAddPlatform;          //добавляем в ивент делегата метод удаления и добавления платформы
    }

    private void TryToDelAndAddPlatform() 
    {
        if (transform.position.z < WorldController.instance.minZ)                       //если платформа оказывается за точкой min.Z то платформа удаляется
        {
            WorldController.instance.worldBuilder.CreatePlatform();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()                                                            //отписываемся от метода
    {
        if (WorldController.instance != null)
        WorldController.instance.OnPlatformMovement -= TryToDelAndAddPlatform;
    }
}
