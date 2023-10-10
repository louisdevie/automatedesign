using AutomateDesign.Core.Users;

namespace AutomateDesign.Server.Data
{
    public interface IUserDao : IBaseDAO<int, User>
    {
        User ReadByEmail(string address);
    }
}
