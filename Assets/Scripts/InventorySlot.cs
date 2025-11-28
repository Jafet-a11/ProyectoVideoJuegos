using UnityEngine;
using UnityEngine.UI;
using System; // Necesario para 'Action'

public class InventorySlot : MonoBehaviour
{
    public Image icono;
    public Button botonSlot; // <--- Tienes que asignar esto en el Inspector del Prefab

    private ItemData itemActual;

    // Este es el "grito" que escucha el libro
    public static event Action<ItemData> OnItemClicked;

    public void ActualizarSlot(ItemData item)
    {
        itemActual = item;

        if (itemActual != null)
        {
            icono.sprite = itemActual.icono;
            icono.enabled = true;
            botonSlot.interactable = true; // Activamos el botón
        }
        else
        {
            icono.sprite = null;
            icono.enabled = false;
            botonSlot.interactable = false; // Desactivamos el botón si no hay item
        }
    }

    // Esta función la conectas al evento OnClick del Botón en Unity
    // O dejamos que Start lo haga automático:
    private void Start()
    {
        if (botonSlot != null)
        {
            botonSlot.onClick.AddListener(AlHacerClick);
        }
    }

    void AlHacerClick()
    {
        if (itemActual != null)
        {
            // Enviamos el aviso al InventoryBookUI
            OnItemClicked?.Invoke(itemActual);
        }
    }
}