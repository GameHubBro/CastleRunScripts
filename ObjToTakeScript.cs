using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjToTakeScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem psMain;
    //[SerializeField] private ParticleSystem psWhenTake;

    private MeshRenderer ms;
    private BoxCollider bc;

    private void Start()
    {
        ms = GetComponentInChildren<MeshRenderer>();
        bc = GetComponentInChildren<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))                     //если игрок сталкивается с объектом, то
        {
            Destroy(psMain);                                //уничтожаем свечение объекта
            /*psWhenTake.Play(); */                             //включаем систему частиц взрыва объекта
            ms.enabled = false;                             //выключаем меш
            bc.enabled = false;                             //выключаем коллайдер объекта
            StartCoroutine(DestroyDelay());                 //запускаем корутину удаления объекта
        }
    }

    IEnumerator DestroyDelay()                              //корутина удаления объекта
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
