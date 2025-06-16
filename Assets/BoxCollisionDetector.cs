using UnityEngine;

public class BoxCollisionDetector : MonoBehaviour
{
    public ForkliftReportSystem reportSystem;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (reportSystem != null)
            {
                reportSystem.SetBoxDropped();
            }
        }
    }
}
