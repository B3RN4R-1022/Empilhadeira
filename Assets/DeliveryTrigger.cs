using UnityEngine;

public class DeliveryTrigger : MonoBehaviour
{
    public ForkliftReportSystem reportSystem; // arraste no Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box")) // A caixa precisa estar com tag "Box"
        {
            Debug.Log("Caixa entrou na área de entrega!");

            if (reportSystem != null)
            {
                reportSystem.SetCorrectPlacement(); // atualiza o objetivo corretamente
                reportSystem.FinalizeDelivery();     // para o cronômetro e atualiza UI
            }
        }
    }
}
