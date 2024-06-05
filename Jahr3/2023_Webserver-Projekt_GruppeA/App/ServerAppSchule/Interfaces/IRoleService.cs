namespace ServerAppSchule.Interfaces
{

    public interface IRoleService
    {
        /// <summary>
        /// Ruft Alle Rollen ab, die nicht admin sind
        /// </summary>
        /// <returns>Liste aller Rollen</returns>
        List<string> GetRoleNames();
        /// <summary>
        /// Fragt Ab ob eine Rolle existiert
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>True=> Rolle exisistiert |False=> Rolle Existiert nicht</returns>
        bool RoleExists(string roleName);
    }

}
