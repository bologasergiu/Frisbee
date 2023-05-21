using FrisbeeApp.Logic.DtoModels;


namespace FrisbeeApp.Logic.Abstractions
{
    public interface IAdminRepository 
    {
        Task<bool> CreateTeam(string teamName);
        Task<bool> DeleteUser(string Id);
        Task<bool> DeleteTeam(string name);
        Task<List<TeamModelDTO>> GetAllTeams();
        Task<List<TeamMemberDTO>> GetAllUsers();
        Task<TeamModelDTO> GetTeamModel(string teamName);

    }
}
