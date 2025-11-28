using UnityEngine;

// Esto crea un menú en Unity para crear nuevos "Items"
[CreateAssetMenu(fileName = "Nuevo Item", menuName = "Inventario/Item")]
public class ItemData : ScriptableObject
{
    public string nombre = "Nuevo Item";
    public string descripcion = "Descripción del item";
    public Sprite icono;

    // Aquí podrías añadir más cosas:
    // public bool esEquipable;
    // public int precio;
   public GameObject modelo3D;
   [Header("Configuración Visual")]
   public float escalaEnInspeccion = 0.5f;
}