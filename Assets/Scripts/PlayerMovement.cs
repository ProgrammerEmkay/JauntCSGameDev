using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to govern the movement of the player character
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    #region PlayerMovementAttributes
    /// <summary>
    /// float to govern the turn speed of the player character
    /// </summary>
    public float turnSpeed = 20f;

    /// <summary>
    /// Animator component of the model to determine animation states
    /// </summary>
    Animator m_Animator;
    /// <summary>
    /// Rigbody component to be used with the player character model
    /// </summary>
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    /// <summary>
    /// Vextor3 used to determine the direction of the player character
    /// </summary>
    Vector3 m_Movement;
    /// <summary>
    /// Quaternion stores the rotation for player character
    /// </summary>
    Quaternion m_Rotation = Quaternion.identity;
    #endregion

    #region PlayerMovement Methods
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        m_Animator = GetComponent<Animator>(); //retrieve the animator component for use with player character
        m_Rigidbody = GetComponent<Rigidbody>(); //retrieve the rigbody component for use with the player character
        m_AudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void FixedUpdate()
    {
        #region PlayerCharacter WASDMovement
        float horizontal = Input.GetAxis("Horizontal"); //Input from the A and D keys to determine direction stored in float horizontal
        float vertical = Input.GetAxis("Vertical"); //Input from the W and S keys to determine direction stored in float vertical

        m_Movement.Set(horizontal, 0f, vertical); //combineation of horizintal and vertical create movemnt vector
        m_Movement.Normalize(); //noormalize the movemnet vector to balance diagnoal movement to be the same speep as any one direction

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f); //define if there is player movement on horizontal or not
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f); //define if there is player movemet on vertical or not
        bool isWalking = hasHorizontalInput || hasVerticalInput; //if there is player movement on either axis the player is walking in game

        m_Animator.SetBool("IsWalking", isWalking); //set the isWalking attribute from the playercharacter state machine

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
        #endregion

        #region PlayerCharacter Rotation
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f); //player charter will rotate to the vector of movement
        m_Rotation = Quaternion.LookRotation(desiredForward); //creates a rotation looking in the given pareter of desiredforward
        #endregion
    }

    /// <summary>
    /// Method to govern the movement of the player character model when moving under player input
    /// </summary>
    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude); //user rigbody and animator delta poistion ot track moving player character
        m_Rigidbody.MoveRotation(m_Rotation); //applies rotation to the player character
    }
    #endregion
}

