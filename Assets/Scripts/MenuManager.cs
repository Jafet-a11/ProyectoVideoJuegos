using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas
using UnityEngine.UI; // Necesario para controlar la Imagen

public class MenuManager : MonoBehaviour
{
    [Header("Configuración de Juego")]
    // Escribe aquí EXACTAMENTE el nombre de la escena de tu juego (ej: "SampleScene")
    public string nombreEscenaJuego = "LaPlaya";

    [Header("Configuración de Fondo")]
    public Image imagenFondo; // Arrastra aquí el objeto "Fondo" de la UI
    public Sprite[] listaDeImagenes; // Aquí pondrás tus fotos
    public float tiempoEntreFotos = 3.0f; // Cada cuántos segundos cambia

    private float temporizador;
    private int indiceActual = 0;

    void Update()
    {
        // Lógica para cambiar las fotos automáticamente
        if (listaDeImagenes.Length > 0 && imagenFondo != null)
        {
            temporizador += Time.deltaTime;

            if (temporizador >= tiempoEntreFotos)
            {
                temporizador = 0f;
                CambiarSiguienteFoto();
            }
        }
    }

    void CambiarSiguienteFoto()
    {
        indiceActual++;
        // Si llegamos al final de la lista, volvemos a la primera (0)
        if (indiceActual >= listaDeImagenes.Length)
        {
            indiceActual = 0;
        }

        // Cambiamos la imagen
        imagenFondo.sprite = listaDeImagenes[indiceActual];
    }

    // --- FUNCIONES PARA LOS BOTONES ---

    public void Jugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}