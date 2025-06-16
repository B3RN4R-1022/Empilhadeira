using UnityEngine;

public class ForkliftCollisionDetector : MonoBehaviour
{
    public ForkliftReportSystem reportSystem;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        string otherTag = other.tag;
        string otherName = other.name;

        Debug.Log($"[COLISÃO DETECTADA] Colidiu com: {otherName} (Tag: {otherTag})");

        // Ignora colisão com o chão (por tag)
        if (otherTag == "Ground")
        {
            Debug.Log("→ Ignorado: chão");
            return;
        }

        if (otherTag == "Box_01")
        {
            Debug.Log("→ Ignorado: Caixa");
            return;
        }

        if (otherTag == "Box")
        {
            Debug.Log("→ Ignorado: Caixa");
            return;
        }

        // Ignora colisão com a caixa monitorada pelo ReportSystem
        if (reportSystem != null && reportSystem.box != null && other == reportSystem.box.gameObject)
        {
            Debug.Log("→ Ignorado: caixa");
            return;
        }

        // Qualquer outra colisão é considerada inválida
        reportSystem.NotifyCollision();
        Debug.Log("→ COLISÃO VÁLIDA! Notificando sistema.");
    }
}
