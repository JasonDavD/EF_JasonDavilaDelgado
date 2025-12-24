using Microsoft.Data.SqlClient;
using POOII_EF_Jason_DavilaDelgado.Models;
using System.Data;

namespace POOII_EF_Jason_DavilaDelgado.Repository
{
    public class SeminarioRepository
    {
        private readonly string _connectionString;

        public SeminarioRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<List<Seminario>> ListarSeminariosDisponibles()
        {
            var lista = new List<Seminario>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.sp_ListarSeminarios", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var seminario = new Seminario
                        {
                            CodigoSeminario = reader["CodigoSeminario"].ToString(),
                            NombreCurso = reader["NombreCurso"].ToString(),
                            HorarioClase = reader["HorariosClase"].ToString(),
                            CapacidadSeminario = Convert.ToInt32(reader["CapacidadSeminario"]),
                            FotoUrl = reader["FotoUrl"].ToString()
                        };
                        lista.Add(seminario);
                    }
                }
            }
            return lista;
        }

        public async Task<Seminario> ObtenerSeminarioPorCodigo(string codigoSeminario)
        {
            Seminario seminario = null;
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.sp_ObtenerSeminario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoSeminario", codigoSeminario);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        seminario = new Seminario
                        {
                            CodigoSeminario = reader["CodigoSeminario"].ToString(),
                            NombreCurso = reader["NombreCurso"].ToString(),
                            HorarioClase = reader["HorariosClase"].ToString(),
                            CapacidadSeminario = Convert.ToInt32(reader["CapacidadSeminario"]),
                            FotoUrl = reader["FotoUrl"].ToString()
                        };
                    }
                }
            }
            return seminario;
        }

        public async Task<int> RegistrarAsistencia(string codigoSeminario, string codigoEstudiante)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.sp_RegistrarAsistencia", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoSeminario", codigoSeminario);
                cmd.Parameters.AddWithValue("@CodigoEstudiante", codigoEstudiante);

                var outputParam = new SqlParameter("@NumeroRegistro", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return (int)outputParam.Value;
            }
        }

    }
}
