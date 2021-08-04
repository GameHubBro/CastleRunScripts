using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawSpeedChanger : MonoBehaviour
{
    private HingeJoint hingeJoint;                                          

    [SerializeField] private int minTargetVelocity = 50;                            //минимальная скорость пилы
    [SerializeField] private int maxTargetVelocity = 200;                           //максимальная скорость пилы

    void Start()
    {
        hingeJoint = GetComponent<HingeJoint>();                                    //находим соединение
        var motor = hingeJoint.motor;                                               
        int random = Random.Range(minTargetVelocity, maxTargetVelocity);
        motor.targetVelocity = random;
        hingeJoint.motor = motor;                                                   //меняем target velocit на случайное между min и max
    }
}
