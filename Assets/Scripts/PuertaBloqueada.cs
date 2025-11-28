using System.Collections;
using UnityEngine;
using TMPro;

public class PuertaBloqueada : MonoBehaviour
{
    [Header("Configuración")]
    public ItemData itemNecesario;
    public GameObject puertaVisual;
    public GameObject TextoAviso;

    [Header("Estado de la Puerta")]
    public bool yaFueAbierta = false; 

    [Header("UI Feedback")]
    public TextMeshProUGUI textoFeedback;

    public void IntentarAbrir(PlayerInventory inventario)
    {
        if (inventario.items.Contains(itemNecesario))
        {
            // MENSAJE DE ÉXITO
            MostrarMensaje("¡Puerta abierta!");

            // --- NUEVO: Marcamos que la puerta se abrió ---
            yaFueAbierta = true;

            puertaVisual.SetActive(false);

            if (TextoAviso != null) TextoAviso.SetActive(false);

            GetComponent<Collider>().enabled = false; // Esto está bien, quita la colisión física

            inventario.puertaCercana = null;
        }
        else
        {
            MostrarMensaje("Necesitas el objeto: " + itemNecesario.nombre);
        }
    }

    void MostrarMensaje(string mensaje)
    {
        if (textoFeedback != null)
        {
            textoFeedback.text = mensaje;
            textoFeedback.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(BorrarMensajeDespuesDeTiempo());
        }
    }

    IEnumerator BorrarMensajeDespuesDeTiempo()
    {
        yield return new WaitForSeconds(3f);
        if (textoFeedback != null) textoFeedback.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInv = other.GetComponent<PlayerInventory>();
            if (playerInv != null)
            {
                playerInv.puertaCercana = this;
                if (TextoAviso != null) TextoAviso.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInv = other.GetComponent<PlayerInventory>();
            if (playerInv != null)
            {
                playerInv.puertaCercana = null;
                if (TextoAviso != null) TextoAviso.SetActive(false);
            }
        }
    }
}