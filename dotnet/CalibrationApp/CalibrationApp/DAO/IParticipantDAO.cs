using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IParticipantDAO
    {
        public List<Team> GetTeams();
        public List<Role> GetRoles();
        public int SwitchActive(int userId);
        public List<Participant> GetAllParticipants();
    }
}
