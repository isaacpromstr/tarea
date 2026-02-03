using System;
using System.Collections.Generic;
using System.Linq;

// --- DEFINICIÓN DE LAS CLASES PRINCIPALES ---

/// <summary>
/// Representa a una mascota en el sistema.
/// </summary>
public class Mascota
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Tipo { get; set; } // Ejemplo: "Perro", "Gato", "Ave", "Reptil"
    public int Edad { get; set; }
    public string Genero { get; set; } // Ejemplo: "Macho", "Hembra"

    public override string ToString()
    {
        return $"Id: {Id} | Nombre: {Nombre} | Tipo: {Tipo} | Edad: {Edad} | Género: {Genero}";
    }
}

/// <summary>
/// Representa al dueño de una o más mascotas.
/// </summary>
public class Dueño
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public List<Mascota> Mascotas { get; set; }

    public Dueño()
    {
        Mascotas = new List<Mascota>();
    }

    public override string ToString()
    {
        // Crea una lista legible de los nombres de las mascotas del dueño
        string nombresMascotas = Mascotas.Any() 
            ? string.Join(", ", Mascotas.Select(m => m.Nombre)) 
            : "Ninguna mascota registrada";
        
        return $"Id: {Id} | Nombre: {Nombre} | Dirección: {Direccion} | Teléfono: {Telefono} | Mascotas: [{nombresMascotas}]";
    }
}

// --- CLASE PRINCIPAL DEL PROGRAMA ---

public class Veterinaria
{
    // Usamos listas estáticas para simular una base de datos en memoria.
    private static List<Mascota> todasLasMascotas = new List<Mascota>();
    private static List<Dueño> todosLosDueños = new List<Dueño>();
    private static int proximoIdMascota = 1;
    private static int proximoIdDueño = 1;

    public static void Main(string[] args)
    {
        // Cargamos datos de ejemplo para que las consultas se puedan probar desde el inicio
        CargarDatosIniciales();

        bool salir = false;
        while (!salir)
        {
            MostrarMenu();
            string opcion = Console.ReadLine();
            Console.Clear();

            switch (opcion)
            {
                case "1":
                    RegistrarMascota();
                    break;
                case "2":
                    RegistrarDueño();
                    break;
                case "3":
                    ConsultarTodasLasMascotas();
                    break;
                case "4":
                    ConsultarMascotasPorTipo("Gato");
                    break;
                case "5":
                    ConsultarTodosLosDueños();
                    break;
                case "6":
                    ConsultarDueñosConSoloPerros();
                    break;
                case "0":
                    salir = true;
                    Console.WriteLine("Saliendo del programa...");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Por favor, intente de nuevo.");
                    break;
            }

            if (!salir)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
    
    /// <summary>
    /// Muestra el menú de opciones al usuario.
    /// </summary>
    private static void MostrarMenu()
    {
        Console.Clear();
        Console.WriteLine("===== Sistema de Gestión de Veterinaria =====");
        Console.WriteLine("1. Registrar una nueva mascota");
        Console.WriteLine("2. Registrar un nuevo dueño");
        Console.WriteLine("---------------------------------------------");
        Console.WriteLine("3. Consultar todas las mascotas");
        Console.WriteLine("4. Consultar todas las mascotas que son gatos");
        Console.WriteLine("5. Consultar todos los dueños");
        Console.WriteLine("6. Consultar dueños que solo tienen perros");
        Console.WriteLine("---------------------------------------------");
        Console.WriteLine("0. Salir");
        Console.Write("Seleccione una opción: ");
    }

    /// <summary>
    /// Pide al usuario los datos para registrar una nueva mascota.
    /// </summary>
    private static void RegistrarMascota()
    {
        Console.WriteLine("--- Registro de Nueva Mascota ---");
        try
        {
            Console.Write("Nombre de la mascota: ");
            string nombre = Console.ReadLine();

            Console.Write("Tipo de mascota (ej. Perro, Gato, Ave): ");
            string tipo = Console.ReadLine();

            Console.Write("Edad de la mascota: ");
            int edad = int.Parse(Console.ReadLine());

            Console.Write("Género de la mascota (Macho/Hembra): ");
            string genero = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(genero))
            {
                Console.WriteLine("Error: Todos los campos de texto son obligatorios.");
                return;
            }

            var nuevaMascota = new Mascota
            {
                Id = proximoIdMascota++,
                Nombre = nombre,
                Tipo = tipo,
                Edad = edad,
                Genero = genero
            };

            todasLasMascotas.Add(nuevaMascota);
            Console.WriteLine("\n¡Mascota registrada con éxito!");
            Console.WriteLine(nuevaMascota.ToString());
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: La edad debe ser un número válido.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ha ocurrido un error inesperado: {ex.Message}");
        }
    }

    /// <summary>
    /// Pide al usuario los datos para registrar un nuevo dueño y le permite asociar mascotas existentes.
    /// </summary>
    private static void RegistrarDueño()
    {
        Console.WriteLine("--- Registro de Nuevo Dueño ---");
        try
        {
            Console.Write("Nombre del dueño: ");
            string nombre = Console.ReadLine();

            Console.Write("Dirección: ");
            string direccion = Console.ReadLine();

            Console.Write("Teléfono: ");
            string telefono = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(direccion) || string.IsNullOrWhiteSpace(telefono))
            {
                Console.WriteLine("Error: Nombre, dirección y teléfono son obligatorios.");
                return;
            }

            var nuevoDueño = new Dueño
            {
                Id = proximoIdDueño++,
                Nombre = nombre,
                Direccion = direccion,
                Telefono = telefono
            };

            // Lógica para asociar mascotas sin dueño
            var mascotasSinDueño = todasLasMascotas.Except(todosLosDueños.SelectMany(d => d.Mascotas)).ToList();

            if (mascotasSinDueño.Any())
            {
                Console.WriteLine("\n--- Asociar Mascotas ---");
                Console.WriteLine("Mascotas disponibles para asociar:");
                mascotasSinDueño.ForEach(m => Console.WriteLine(m.ToString()));
                
                while (true)
                {
                    Console.Write("\nIngrese el ID de la mascota que desea agregar (o escriba 'fin' para terminar): ");
                    string input = Console.ReadLine();

                    if (input.ToLower() == "fin") break;

                    if (int.TryParse(input, out int mascotaId))
                    {
                        var mascotaAAsociar = mascotasSinDueño.FirstOrDefault(m => m.Id == mascotaId);
                        if (mascotaAAsociar != null)
                        {
                            nuevoDueño.Mascotas.Add(mascotaAAsociar);
                            mascotasSinDueño.Remove(mascotaAAsociar); // La removemos para no poder seleccionarla de nuevo
                            Console.WriteLine($"Mascota '{mascotaAAsociar.Nombre}' agregada al dueño.");
                        }
                        else
                        {
                            Console.WriteLine("ID de mascota no válido o ya ha sido asignada.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Entrada no válida.");
                    }
                }
            }
            else
            {
                Console.WriteLine("\nNo hay mascotas sin dueño disponibles para asociar.");
            }

            todosLosDueños.Add(nuevoDueño);
            Console.WriteLine("\n¡Dueño registrado con éxito!");
            Console.WriteLine(nuevoDueño.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ha ocurrido un error inesperado: {ex.Message}");
        }
    }

    /// <summary>
    /// Muestra por consola la lista completa de mascotas registradas.
    /// </summary>
    private static void ConsultarTodasLasMascotas()
    {
        Console.WriteLine("--- Listado de Todas las Mascotas ---");
        if (!todasLasMascotas.Any())
        {
            Console.WriteLine("No hay mascotas registradas en el sistema.");
            return;
        }
        foreach (var mascota in todasLasMascotas)
        {
            Console.WriteLine(mascota.ToString());
        }
    }
    
    /// <summary>
    /// Muestra por consola las mascotas que coinciden con el tipo especificado.
    /// </summary>
    private static void ConsultarMascotasPorTipo(string tipo)
    {
        Console.WriteLine($"--- Listado de Mascotas que son '{tipo}' ---");
        var mascotasFiltradas = todasLasMascotas.Where(m => m.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!mascotasFiltradas.Any())
        {
            Console.WriteLine($"No se encontraron mascotas del tipo '{tipo}'.");
            return;
        }
        foreach (var mascota in mascotasFiltradas)
        {
            Console.WriteLine(mascota.ToString());
        }
    }

    /// <summary>
    /// Muestra por consola la lista completa de dueños registrados.
    /// </summary>
    private static void ConsultarTodosLosDueños()
    {
        Console.WriteLine("--- Listado de Todos los Dueños ---");
        if (!todosLosDueños.Any())
        {
            Console.WriteLine("No hay dueños registrados en el sistema.");
            return;
        }
        foreach (var dueño in todosLosDueños)
        {
            Console.WriteLine(dueño.ToString());
        }
    }
    
    /// <summary>
    /// Muestra por consola los dueños que tienen al menos una mascota y todas sus mascotas son perros.
    /// </summary>
    private static void ConsultarDueñosConSoloPerros()
    {
        Console.WriteLine("--- Dueños que solo tienen Perros ---");
        // La consulta LINQ se asegura de dos cosas:
        // 1. Que el dueño tenga al menos una mascota (d.Mascotas.Any())
        // 2. Que TODAS las mascotas que tiene sean del tipo "Perro" (d.Mascotas.All(...))
        var dueñosConPerros = todosLosDueños
            .Where(d => d.Mascotas.Any() && d.Mascotas.All(m => m.Tipo.Equals("Perro", StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (!dueñosConPerros.Any())
        {
            Console.WriteLine("No se encontraron dueños que cumplan con este criterio.");
            return;
        }
        foreach (var dueño in dueñosConPerros)
        {
            Console.WriteLine(dueño.ToString());
        }
    }
    
    /// <summary>
    /// Carga un conjunto de datos inicial para facilitar las pruebas.
    /// </summary>
    private static void CargarDatosIniciales()
    {
        // Mascotas
        var mascota1 = new Mascota { Id = proximoIdMascota++, Nombre = "Firulais", Tipo = "Perro", Edad = 5, Genero = "Macho" };
        var mascota2 = new Mascota { Id = proximoIdMascota++, Nombre = "Mishu", Tipo = "Gato", Edad = 3, Genero = "Hembra" };
        var mascota3 = new Mascota { Id = proximoIdMascota++, Nombre = "Rex", Tipo = "Perro", Edad = 2, Genero = "Macho" };
        var mascota4 = new Mascota { Id = proximoIdMascota++, Nombre = "Piolín", Tipo = "Ave", Edad = 1, Genero = "Macho" };
        var mascota5 = new Mascota { Id = proximoIdMascota++, Nombre = "Luna", Tipo = "Gato", Edad = 7, Genero = "Hembra" };
        var mascota6 = new Mascota { Id = proximoIdMascota++, Nombre = "Rocky", Tipo = "Perro", Edad = 4, Genero = "Macho" };
        todasLasMascotas.AddRange(new List<Mascota> { mascota1, mascota2, mascota3, mascota4, mascota5, mascota6 });

        // Dueños
        var dueño1 = new Dueño { Id = proximoIdDueño++, Nombre = "Juan Pérez", Direccion = "Calle Falsa 123", Telefono = "555-1111" };
        dueño1.Mascotas.Add(mascota1); // Juan tiene a Firulais (Perro)
        dueño1.Mascotas.Add(mascota2); // y a Mishu (Gato)

        var dueño2 = new Dueño { Id = proximoIdDueño++, Nombre = "Ana Gómez", Direccion = "Avenida Siempreviva 742", Telefono = "555-2222" };
        dueño2.Mascotas.Add(mascota3); // Ana solo tiene a Rex (Perro)
        dueño2.Mascotas.Add(mascota6); // y a Rocky (Perro)

        var dueño3 = new Dueño { Id = proximoIdDueño++, Nombre = "Carlos Ruiz", Direccion = "Boulevard de los Sueños Rotos 45", Telefono = "555-3333" };
        // Carlos no tiene mascotas asignadas inicialmente

        todosLosDueños.AddRange(new List<Dueño> { dueño1, dueño2, dueño3 });
    }
}
