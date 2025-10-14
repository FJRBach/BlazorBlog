
using ClienteBlogWASM.Modelos;

namespace ClienteBlogWASM.Servicios.IServicio
{
    public interface IServicioAutenticacion
    {
        Task<RespuestaRegistro> RegistrarUsuario(UsuarioRegistro usuarioParaRegistro);

        Task<RespuestaAutenticacion> Acceder(UsuarioAutenticacion usuarioDesdeAutenticacion);
        Task Salir();
    }
}
