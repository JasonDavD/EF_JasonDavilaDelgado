using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POOII_EF_Jason_DavilaDelgado.Data;
using POOII_EF_Jason_DavilaDelgado.Repository;
using POOII_EF_Jason_DavilaDelgado.Models.ViewModels;

namespace POOII_EF_Jason_DavilaDelgado.Controllers
{
    public class SeminarioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public SeminarioController(AppDbContext context)
        {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var repo = new SeminarioRepository(_connectionString);
            var seminarios = await repo.ListarSeminariosDisponibles();
            return View(seminarios);
        }

        [HttpGet]
        public async Task<IActionResult> Registrar(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                TempData["Mensaje"] = "Código de seminario inválido.";
                TempData["TipoMensaje"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            var repo = new SeminarioRepository(_connectionString);
            var seminario = await repo.ObtenerSeminarioPorCodigo(codigo);

            if (seminario == null)
            {
                TempData["Mensaje"] = "No se encontró el seminario.";
                TempData["TipoMensaje"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new RegistroAsistenciaViewModel
            {
                CodigoSeminario = codigo,
                Seminario = seminario
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistroAsistenciaViewModel model)
        {
            var repo = new SeminarioRepository(_connectionString);

            if (string.IsNullOrWhiteSpace(model.CodigoEstudiante))
            {
                ViewBag.Mensaje = "Debe ingresar un código de estudiante.";
                ViewBag.TipoMensaje = "danger";
                model.Seminario = await repo.ObtenerSeminarioPorCodigo(model.CodigoSeminario);
                return View(model);
            }

            try
            {
                int numeroRegistro = await repo.RegistrarAsistencia(
                    model.CodigoSeminario,
                    model.CodigoEstudiante
                );

                TempData["Mensaje"] = $"Registro exitoso. Número de registro: {numeroRegistro}";
                TempData["TipoMensaje"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al registrar. Verifique que no esté ya inscrito.";
                ViewBag.TipoMensaje = "danger";
                model.Seminario = await repo.ObtenerSeminarioPorCodigo(model.CodigoSeminario);
                return View(model);
            }
        }
    }
}
