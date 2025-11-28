using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class FinalizarNivel : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreSiguienteEscena;

    [Header("Requisito")]
    [Tooltip("Arrastra aquí el objeto que tiene el script PuertaBloqueada")]
    public PuertaBloqueada scriptDeLaPuerta; // Referencia directa a tu puerta

    [Header("Interfaz (UI)")]
    public TextMeshProUGUI textoEnPantalla;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // PREGUNTAMOS: ¿La puerta del barco ya fue abierta?
            if (scriptDeLaPuerta.yaFueAbierta == true)
            {
                MostrarMensaje("Pasando al siguiente nivel");
                Invoke("CargarEscena", 2f);
            }
            else
            {
                // Si la puerta sigue cerrada
                MostrarMensaje("¡No puedes irte! Debes lograr abrir la puerta del camarote primero.");
            }
        }
    }

    void MostrarMensaje(string mensaje)
    {
        if (textoEnPantalla != null)
        {
            textoEnPantalla.text = mensaje;
            StopAllCoroutines();
            StartCoroutine(BorrarTextoDespuesDe(3f));
        }
    }

    IEnumerator BorrarTextoDespuesDe(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (textoEnPantalla != null) textoEnPantalla.text = "";
    }

    void CargarEscena()
    {
        SceneManager.LoadScene(nombreSiguienteEscena);
    }
}