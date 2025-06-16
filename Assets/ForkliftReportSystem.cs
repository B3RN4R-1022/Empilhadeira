using UnityEngine;
using TMPro;

public class ForkliftReportSystem : MonoBehaviour
{
    [Header("Referências")]
    public ForkliftController forklift;
    public Transform box;
    public Transform platformCheck;

    [Header("UI")]
    public TMP_Text reportText;
    public TMP_Text timerText;

    [Header("Parâmetros")]
    public float maxLiftHeight = 5.5f;
    public float maxDeliveryTime = 60f;
    public float maxReverseDistance = 5f;
    public float boxFallThreshold = 1.0f;

    private Vector3 lastPosition;
    private float reverseDistance;
    private float deliveryTimer;
    private bool deliveryStarted;
    private bool deliveryEnded;

    private bool boxDropped;
    private bool collided;
    private bool liftedTooHigh;
    private bool correctPlacement;

    private void Start()
    {
        lastPosition = forklift.transform.position;
        AtualizarObjetivosUI();
    }

    private void Update()
    {
        if (!deliveryStarted)
        {
            //mostrar cronometro
            timerText.text = $"<color=white>{0:F1}s</color>";

            if (IsBoxLifted())
            {
                deliveryStarted = true;
                deliveryTimer = 0;
            }
        }

        if (deliveryStarted && !deliveryEnded)
        {
            deliveryTimer += Time.deltaTime;

            // Atualiza a cor do cronômetro 
            string tempoCor = deliveryTimer <= maxDeliveryTime ? "white" : "red";
            timerText.text = $"<color={tempoCor}>{deliveryTimer:F1}s</color>";

            CheckReverse();
            CheckLiftSafety();
            CheckBoxDropped();

            AtualizarObjetivosUI();
        }
    }

    private void CheckReverse()
    {
        Vector3 movement = forklift.transform.position - lastPosition;
        if (Vector3.Dot(forklift.transform.forward, movement.normalized) < -0.5f)
        {
            reverseDistance += movement.magnitude;
        }
        lastPosition = forklift.transform.position;
    }

    private void CheckLiftSafety()
    {
        if (forklift.lift.localPosition.y > maxLiftHeight && forklift.GetVelocity().magnitude > 0.5f)
        {
            liftedTooHigh = true;
        }
    }

    private void CheckBoxDropped()
    {
        if (box == null) return;

        float alturaEmpilhadeira = forklift.transform.position.y;
        float alturaCaixa = box.position.y;

        if (alturaEmpilhadeira - alturaCaixa > boxFallThreshold)
        {
            boxDropped = true;
        }
    }

    private bool IsBoxLifted()
    {
        return box != null && Vector3.Distance(box.position, forklift.lift.position) > 0.5f;
    }

    public void FinalizeDelivery()
    {
        deliveryEnded = true;
        CheckBoxDropped();
    }

    public void NotifyCollision()
    {
        collided = true;
    }

    public void SetBoxDropped()
    {
        boxDropped = true;
        AtualizarObjetivosUI();
    }

    public void SetCorrectPlacement()
    {
        if (!boxDropped)
        {
            correctPlacement = true;
            AtualizarObjetivosUI();
        }
    }
    private void AtualizarObjetivosUI()
    {
        string GetStatus(bool? isOk)
        {
            if (!isOk.HasValue) return "-";
            return isOk.Value ? "<color=#00FF00>■</color>" : "<color=#FF0000>■</color>";
        }

        bool? caixaOk = deliveryStarted ? !boxDropped : (bool?)null;
        bool? colisaoOk = deliveryStarted ? !collided : (bool?)null;
        bool? alturaOk = deliveryStarted ? !liftedTooHigh : (bool?)null;
        bool? tempoOk = deliveryEnded ? deliveryTimer <= maxDeliveryTime : (bool?)null;
        bool? reOk = deliveryStarted ? reverseDistance <= maxReverseDistance : (bool?)null;

        bool? posicaoOk = null;
        if (boxDropped)
            posicaoOk = false;
        else if (correctPlacement)
            posicaoOk = true;

        string tempoCorFinal = deliveryEnded
            ? (deliveryTimer <= maxDeliveryTime ? "#00FF00" : "#FF0000") 
            : "white"; 

        string tempoFinal = $"\n<color={tempoCorFinal}>{GetStatus(tempoOk)} Tempo: {deliveryTimer:F1}s</color>";


        reportText.text =
            $"{GetStatus(caixaOk)} Caixa não caiu\n" +
            $"{GetStatus(colisaoOk)} Sem colisão\n" +
            $"{GetStatus(alturaOk)} Altura segura\n" +
            $"{GetStatus(posicaoOk)} Posição correta\n" +
            $"{GetStatus(reOk)} Ré controlada" +
            tempoFinal;
    }
}
