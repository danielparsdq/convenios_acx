using ConveniosApi.ServiciosAcromaxEntities;

namespace ConveniosApi.Models.ConveniosModels
{
    public class UsuarioModel
    {
        public UsuarioModel(SysUsuario? _user)
        {
            if(_user != null)
            {
                User = _user.User;
                Nombres = _user.Nombres;
                Apellidos = _user.Apellidos;
                Email = _user.Email;
            }
        }
        public string? User { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Email { get; set; }
    }
}
