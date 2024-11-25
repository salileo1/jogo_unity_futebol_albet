using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // Arraste o Player no inspetor
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10); // Ajuste o deslocamento da câmera em relação ao Player
    [SerializeField] private float followSpeed = 5f; // Velocidade de acompanhamento

    void LateUpdate()
    {
        if (player != null)
        {
            // Suavemente move a câmera para a posição do Player + Offset
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Faz a câmera olhar para o Player
            transform.LookAt(player);
        }
    }
}
