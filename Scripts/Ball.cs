using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform transformPlayer; // Referência ao jogador
    [SerializeField] private Transform transformMarcador; // Referência ao marcador
    [SerializeField] private Vector3 ballStartPosition = new Vector3(0.748f, 0.22f, 0.13f); // Posição inicial da bola
    [SerializeField] private Vector3 playerStartPosition = new Vector3(0.0f, 0.0f, -5.0f); // Posição inicial do jogador
    [SerializeField] private Vector3 marcadorStartPosition = new Vector3(20.0f, 0.07f, 0.8f); // Posição inicial do marcador

    private bool stickToPlayer;
    [SerializeField] public Transform playerBallPosition;
    private Player scriptPlayer;
    private Vector2 previousLocation;
    private float speed;

    public bool StickToPlayer { get => stickToPlayer; set => stickToPlayer = value; }

    void Start()
    {
        scriptPlayer = transformPlayer.GetComponent<Player>();
        playerBallPosition = transformPlayer.Find("Geometry").Find("BallLocation");
    }

    void Update()
    {
        if (!stickToPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transformPlayer.position, transform.position);
            if (distanceToPlayer < 0.5)
            {
                stickToPlayer = true;
                scriptPlayer.BallAttachedToPlayer = this;
            }
        }
        else
        {
            Vector2 currentLocation = new Vector2(transform.position.x, transform.position.z);
            speed = Vector2.Distance(currentLocation, previousLocation) / Time.deltaTime;
            transform.position = playerBallPosition.position;
            transform.Rotate(new Vector3(transformPlayer.right.x, 0, transformPlayer.right.z), speed, Space.World);
            previousLocation = currentLocation;
        }

        // Reiniciar a bola, o jogador e o marcador se a bola cair abaixo de um limite
        if (transform.position.y < -2)
        {
            recomecar();
        }
    }

    public void recomecar()
    {
        // Reposicionar a bola
        transform.position = ballStartPosition;

        // Reposicionar o jogador
        transformPlayer.position = playerStartPosition;

        // Reposicionar o marcador
        if (transformMarcador != null)
        {
            transformMarcador.position = marcadorStartPosition;
            transformMarcador.rotation = Quaternion.identity; // Resetar a rotação do marcador
        }

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        transformPlayer.rotation = Quaternion.identity;
    }
}
