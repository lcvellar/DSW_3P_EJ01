using System.Text.Json;
using DSW_3P_EJ01.Models;

namespace DSW_3P_EJ01.Repositories;

public class MisionJsonRepository : IMisionRepository
{
    private readonly string _ruta;
    private readonly JsonSerializerOptions _opciones = new() { WriteIndented = true };

    public MisionJsonRepository(IWebHostEnvironment ambiente)
    {
        string carpeta = Path.Combine(ambiente.ContentRootPath, "Data");
        Directory.CreateDirectory(carpeta);
        _ruta = Path.Combine(carpeta, "misiones.json");

        if (!File.Exists(_ruta))
            File.WriteAllText(_ruta, "[]");
    }

    private List<Mision> Leer()
    {
        string json = File.ReadAllText(_ruta);
        return JsonSerializer.Deserialize<List<Mision>>(json) ?? new List<Mision>();
    }

    private void Guardar(List<Mision> misiones)
    {
        string json = JsonSerializer.Serialize(misiones, _opciones);
        File.WriteAllText(_ruta, json);
    }

    public List<Mision> ObtenerTodos() => Leer();

    public Mision? ObtenerPorId(int id) =>
        Leer().FirstOrDefault(m => m.Id == id);

    public Mision Agregar(Mision mision)
    {
        var misiones = Leer();
        mision.Id = misiones.Count == 0 ? 1 : misiones.Max(m => m.Id) + 1;
        misiones.Add(mision);
        Guardar(misiones);
        return mision;
    }

    public bool Actualizar(int id, Mision datos)
    {
        var misiones = Leer();
        var actual = misiones.FirstOrDefault(m => m.Id == id);
        if (actual is null) return false;

        actual.Nombre = datos.Nombre;
        actual.Dificultad = datos.Dificultad;
        actual.RecompensaXP = datos.RecompensaXP;
        actual.Completada = datos.Completada;

        Guardar(misiones);
        return true;
    }

    public bool Eliminar(int id)
    {
        var misiones = Leer();
        var actual = misiones.FirstOrDefault(m => m.Id == id);
        if (actual is null) return false;

        misiones.Remove(actual);
        Guardar(misiones);
        return true;
    }
}
