using DSW_3P_EJ01.Models;
using DSW_3P_EJ01.Repositories;

namespace DSW_3P_EJ01.Services;

public class MisionService : IMisionService
{
    private readonly IMisionRepository _repositorio;

    public MisionService(IMisionRepository repositorio)
    {
        _repositorio = repositorio;
    }

    private static void Validar(Mision mision)
    {
        if (string.IsNullOrWhiteSpace(mision.Nombre))
            throw new ArgumentException("El nombre de la misión es obligatorio.");

        string[] dificultades = { "Fácil", "Media", "Difícil" };
        if (!dificultades.Contains(mision.Dificultad, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("La dificultad debe ser Fácil, Media o Difícil.");

        if (mision.RecompensaXP < 0)
            throw new ArgumentException("La recompensa XP no puede ser negativa.");
    }

    public List<Mision> ObtenerTodos() => _repositorio.ObtenerTodos();

    public Mision? ObtenerPorId(int id) => _repositorio.ObtenerPorId(id);

    public Mision Agregar(Mision mision)
    {
        Validar(mision);
        mision.Completada = false; // Regla de negocio: toda misión nueva inicia pendiente.
        return _repositorio.Agregar(mision);
    }

    public bool Actualizar(int id, Mision mision)
    {
        Validar(mision);
        return _repositorio.Actualizar(id, mision);
    }

    public bool Eliminar(int id) => _repositorio.Eliminar(id);
}
