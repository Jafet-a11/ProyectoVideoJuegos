using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryBookUI : MonoBehaviour
{
    [Header("UI Página Derecha")]
    public Image iconoDetalle;       // La imagen grande dorada
    public TextMeshProUGUI titulo;   // "DAGA RITUAL"
    public TextMeshProUGUI descripcion; // El texto de historia

    [Header("Configuración")]
    public GameObject visualesDetalle; // Un objeto padre para ocultar todo si no hay selección

    private void OnEnable()
    {
        // Nos suscribimos al evento del slot
        InventorySlot.OnItemClicked += ActualizarPaginaDerecha;
        LimpiarDetalle();
    }

    private void OnDisable()
    {
        // Nos desuscribimos para evitar errores
        InventorySlot.OnItemClicked -= ActualizarPaginaDerecha;
    }

    void Start()
    {
        // Al inicio, ocultamos la página derecha
        LimpiarDetalle();
    }

    void ActualizarPaginaDerecha(ItemData item)
    {
        visualesDetalle.SetActive(true);

        iconoDetalle.sprite = item.icono;
        // Si quieres que brille como en la imagen, podrías añadir un material o cambiar el color
        iconoDetalle.color = Color.white;

        titulo.text = item.nombre;
        descripcion.text = item.descripcion;
    }

    public void LimpiarDetalle()
    {
        visualesDetalle.SetActive(false);
    }
}