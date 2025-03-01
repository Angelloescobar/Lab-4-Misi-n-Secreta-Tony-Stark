using System;
using System.IO;

class InventosApp
{
    static void Main()
    {
        string rutaArchivo = "inventos.txt";
        string carpetaRespaldo = "Backup";
        string carpetaClasificada = "ArchivosClasificados";
        string carpetaSecreta = "ProyectosSecretos";
        string carpetaLaboratorio = "LaboratorioAvengers";

        while (true)
        {
            Console.WriteLine("\nOpciones:");
            Console.WriteLine("1. Crear archivo 'inventos.txt'");
            Console.WriteLine("2. Agregar nuevos inventos");
            Console.WriteLine("3. Leer inventos línea por línea");
            Console.WriteLine("4. Leer todo el contenido del archivo");
            Console.WriteLine("5. Copiar archivo a 'Backup'");
            Console.WriteLine("6. Mover archivo a 'ArchivosClasificados'");
            Console.WriteLine("7. Crear carpeta 'ProyectosSecretos'");
            Console.WriteLine("8. Listar archivos en 'LaboratorioAvengers'");
            Console.WriteLine("9. Eliminar archivo 'inventos.txt'");
            Console.WriteLine("10. Salir");
            Console.Write("\nSelecciona una opción: ");
            string opcion = Console.ReadLine();

            try
            {
                switch (opcion)
                {
                    case "1":
                        CrearArchivo(rutaArchivo);
                        break;
                    case "2":
                        AgregarInvento(rutaArchivo);
                        break;
                    case "3":
                        LeerLineaPorLinea(rutaArchivo);
                        break;
                    case "4":
                        LeerTodoElTexto(rutaArchivo);
                        break;
                    case "5":
                        CopiarArchivo(rutaArchivo, Path.Combine(carpetaRespaldo, "inventos.txt"));
                        break;
                    case "6":
                        MoverArchivo(rutaArchivo, Path.Combine(carpetaClasificada, "inventos.txt"));
                        break;
                    case "7":
                        CrearCarpeta(carpetaSecreta);
                        break;
                    case "8":
                        ListarArchivos(carpetaLaboratorio);
                        break;
                    case "9":
                        EliminarArchivo(rutaArchivo, carpetaRespaldo);
                        break;
                    case "10":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void CrearArchivo(string ruta)
    {
        if (!File.Exists(ruta))
        {
            using (StreamWriter escritor = File.CreateText(ruta))
            {
                Console.WriteLine($"Archivo '{ruta}' creado.");
            }
        }
        else
        {
            Console.WriteLine($"El archivo '{ruta}' ya existe.");
        }
    }

    static void AgregarInvento(string ruta)
    {
        VerificarExistenciaArchivo(ruta);
        int contador = ObtenerNumeroSiguiente(ruta);
        Console.WriteLine("\nEscribe los nombres de los nuevos inventos (uno por línea). Escribe 'fin' para terminar.");

        using (StreamWriter escritor = new StreamWriter(ruta, append: true))
        {
            string entrada;
            while ((entrada = Console.ReadLine()) != "fin")
            {
                escritor.WriteLine($"{contador}. {entrada}");
                contador++;
            }
        }

        Console.WriteLine("\nLos inventos han sido guardados.");
    }

    static void LeerLineaPorLinea(string ruta)
    {
        VerificarExistenciaArchivo(ruta);
        Console.WriteLine("\nInventos guardados:");
        using (StreamReader lector = new StreamReader(ruta))
        {
            string linea;
            while ((linea = lector.ReadLine()) != null)
            {
                Console.WriteLine(linea);
            }
        }
    }

    static void LeerTodoElTexto(string ruta)
    {
        VerificarExistenciaArchivo(ruta);
        string contenido = File.ReadAllText(ruta);
        Console.WriteLine("\nContenido completo del archivo:");
        Console.WriteLine(contenido);
    }

    static void CopiarArchivo(string origen, string destino)
    {
        VerificarExistenciaArchivo(origen);
        Directory.CreateDirectory(Path.GetDirectoryName(destino));
        File.Copy(origen, destino, true);
        Console.WriteLine($"Archivo copiado a '{destino}'.");
    }

    static void MoverArchivo(string origen, string destino)
    {
        VerificarExistenciaArchivo(origen);
        Directory.CreateDirectory(Path.GetDirectoryName(destino));
        File.Move(origen, destino);
        Console.WriteLine($"Archivo movido a '{destino}'.");
    }

    static void CrearCarpeta(string nombreCarpeta)
    {
        Directory.CreateDirectory(nombreCarpeta);
        Console.WriteLine($"Carpeta '{nombreCarpeta}' creada.");
    }

    static void EliminarArchivo(string ruta, string carpetaRespaldo)
    {
        VerificarExistenciaArchivo(ruta);
        CopiarArchivo(ruta, Path.Combine(carpetaRespaldo, "inventos.txt"));
        File.Delete(ruta);
        Console.WriteLine($"Archivo '{ruta}' eliminado después de hacer una copia de seguridad.");
    }

    static void ListarArchivos(string ruta)
    {
        if (Directory.Exists(ruta))
        {
            Console.WriteLine($"\nArchivos en '{ruta}':");
            string[] archivos = Directory.GetFiles(ruta);
            foreach (var archivo in archivos)
            {
                Console.WriteLine(Path.GetFileName(archivo));
            }
        }
        else
        {
            Console.WriteLine($"El directorio '{ruta}' no existe.");
        }
    }

    static int ObtenerNumeroSiguiente(string ruta)
    {
        int contador = 1;

        if (File.Exists(ruta))
        {
            string[] lineas = File.ReadAllLines(ruta);
            if (lineas.Length > 0)
            {
                string ultimaLinea = lineas[lineas.Length - 1];
                string[] partes = ultimaLinea.Split('.');
                if (int.TryParse(partes[0], out int ultimoNumero))
                {
                    contador = ultimoNumero + 1;
                }
            }
        }

        return contador;
    }

    static void VerificarExistenciaArchivo(string ruta)
    {
        if (!File.Exists(ruta))
        {
            throw new FileNotFoundException($"El archivo '{ruta}' no existe. ¡Ultron debe haberlo borrado!");
        }
    }
}
