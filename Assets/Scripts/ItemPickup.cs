using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemParaRecoger;

    // Esta función la llama tu PlayerInventory
    public void RecogerObjeto(PlayerInventory inventory)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // IMPORTANTE: Aquí buscamos PlayerInventory, NO PlayerMovement
            PlayerInventory inventario = other.GetComponent<PlayerInventory>();

            if (inventario != null)
            {
                inventario.objetoCercano = this;
                inventario.MostrarMensaje(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventario = other.GetComponent<PlayerInventory>();

            if (inventario != null)
            {
                if (inventario.objetoCercano == this)
                {
                    inventario.objetoCercano = null;
                    inventario.MostrarMensaje(false);
                }
            }
        }
    }
}