using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // Necesario para los textos UI
public class PlayerInventory : MonoBehaviour
{
    [Header("Inventario Datos")]
    public List<ItemData> items = new List<ItemData>();
    public int capacidadMaxima = 10;

    [Header("UI Inventario")]
    public GameObject inventarioUI;
    public Transform slotsParent;
    private List<InventorySlot> slots = new List<InventorySlot>();
    private bool inventarioAbierto = false;

    [Header("UI Mensajes")]
    public GameObject mensajeRecoger; // El texto de "Presione E"

    [Header("Configuración Inspección")]
    public GameObject panelInspeccion;    // El panel negro de fondo
    public TextMeshProUGUI nombreObjetoTexto; // El texto para el nombre del objeto
    public Transform puntoInspeccion;     // El objeto vacío hijo de la cámara
    public float velocidadRotacion = 20f; // Velocidad de giro del objeto 3D

    // Variables privadas de control
    private bool enModoInspeccion = false;
    private GameObject modeloActualVisual; // La copia visual que flota
    private ItemPickup itemPendiente;      // El objeto real en el suelo
    [Range(0.1f, 2f)]
    public float tamanoVisual = 0.5f;
    // Variable que se llena automáticamente desde el script ItemPickup
    [HideInInspector] public ItemPickup objetoCercano;
    [HideInInspector] public PuertaBloqueada puertaCercana;
    void Start()
    {
        // Apagamos todas las UI al inicio para que no estorben
        if (inventarioUI != null) inventarioUI.SetActive(false);
        if (mensajeRecoger != null) mensajeRecoger.SetActive(false);
        if (panelInspeccion != null) panelInspeccion.SetActive(false);

        // Buscamos los slots en el panel
        if (slotsParent != null)
        {
            slots.AddRange(slotsParent.GetComponentsInChildren<InventorySlot>());
        }

        ActualizarInventarioUI();
    }
    void Update()
    {
        // Si estamos en modo inspección, hacemos girar el modelo 3D
        if (enModoInspeccion && modeloActualVisual != null)
        {
            modeloActualVisual.transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime);
        }
    }
    public void OnCollect(InputValue value)
    {
        if (value.isPressed)
        {
            // CASO 1: Si ya estamos viendo el objeto (Modo Inspección), Confirmamos y guardamos
            if (enModoInspeccion)
            {
                ConfirmarRecogida();
                return;
            }
            // CASO 2: Si NO estamos inspeccionando, pero hay un objeto cerca, iniciamos la inspección
            if (objetoCercano != null && !enModoInspeccion)
            {
                IniciarInspeccion(objetoCercano);
            }
            // PRIORIDAD 3: Abrir Puertas (NUEVO)
            if (puertaCercana != null)
            {
                // Le mandamos "este" inventario a la puerta para que se revise
                puertaCercana.IntentarAbrir(this);
            }
        }
    }

    // --- LÓGICA DE INSPECCIÓN ---
    // --- CORRECCIÓN EN ESTA FUNCIÓN ---
    void IniciarInspeccion(ItemPickup itemWorld)
    {
        // 1. ¡PRIMERO apagamos el mensaje a la fuerza!
        if (mensajeRecoger != null)
        {
            mensajeRecoger.SetActive(false);
        }

        // 2. LUEGO activamos el modo inspección
        enModoInspeccion = true;
        itemPendiente = itemWorld;

        // 3. Mostrar Panel Negro y Nombre
        if (panelInspeccion != null) panelInspeccion.SetActive(true);
        if (nombreObjetoTexto != null) nombreObjetoTexto.text = itemWorld.itemParaRecoger.nombre;

        // 4. Instanciar el modelo 3D
        if (itemWorld.itemParaRecoger.modelo3D != null && puntoInspeccion != null)
        {
            modeloActualVisual = Instantiate(itemWorld.itemParaRecoger.modelo3D, puntoInspeccion);
            modeloActualVisual.transform.localPosition = Vector3.zero;
            modeloActualVisual.transform.localRotation = Quaternion.identity;

            // --- CAMBIO AQUÍ ---
            // Usamos el tamaño global multiplicado por el tamaño específico de ESTE ítem
            float escalaFinal = tamanoVisual * itemWorld.itemParaRecoger.escalaEnInspeccion;
            modeloActualVisual.transform.localScale = Vector3.one * escalaFinal;

            Destroy(modeloActualVisual.GetComponent<Rigidbody>());
        }
    }

    // --- CORRECCIÓN EN ESTA FUNCIÓN ---
    void ConfirmarRecogida()
    {
        if (itemPendiente != null)
        {
            bool exito = AddItem(itemPendiente.itemParaRecoger);

            if (exito)
            {
                Destroy(itemPendiente.gameObject);

                // ¡IMPORTANTE! Como el objeto se destruye, nunca nos "salimos" de su trigger.
                // Así que debemos limpiar la variable manual y forzar que el mensaje no vuelva.
                objetoCercano = null;
            }
        }

        enModoInspeccion = false;
        itemPendiente = null;

        if (modeloActualVisual != null) Destroy(modeloActualVisual);
        if (panelInspeccion != null) panelInspeccion.SetActive(false);

        // Aseguramos que el mensaje se quede apagado al terminar
        if (mensajeRecoger != null) mensajeRecoger.SetActive(false);
    }

    // --- CONTROL DE INVENTARIO (Tecla I) ---
    public void OnInventory(InputValue value)
    {
        if (enModoInspeccion) return;
        if (value.isPressed)
        {
            ToggleInventory();
        }
    }
    public void ToggleInventory()
    {
        inventarioAbierto = !inventarioAbierto;

        if (inventarioUI != null)
            inventarioUI.SetActive(inventarioAbierto);

        if (inventarioAbierto)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ActualizarInventarioUI();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void MostrarMensaje(bool mostrar)
    {
        if (!enModoInspeccion && mensajeRecoger != null)
        {
            mensajeRecoger.SetActive(mostrar);
        }
    }

    public bool AddItem(ItemData item)
    {
        if (items.Count >= capacidadMaxima)
        {
            Debug.Log("Inventario lleno.");
            return false;
        }

        items.Add(item);
        ActualizarInventarioUI();
        return true;
    }
    public void RemoveItem(ItemData item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            ActualizarInventarioUI();
        }
    }
    void ActualizarInventarioUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                slots[i].ActualizarSlot(items[i]);
            }
            else
            {
                slots[i].ActualizarSlot(null);
            }
        }
    }
}