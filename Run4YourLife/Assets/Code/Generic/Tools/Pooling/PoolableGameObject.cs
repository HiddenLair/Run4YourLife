using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolableGameObject : MonoBehaviour {

    [SerializeField]
    private PoolRequest[] m_poolPetitions;

    public PoolRequest[] PoolPetitions { get { return m_poolPetitions; } }
}
