using AutomateDesign.Core.Users;

namespace AutomateDesign.Server.Data
{
    public interface IUserDao
    {
        public User Create(User user);

        public User ReadById(int id);

        public User ReadByEmail(string address);

        public void Update(User user);

        public void Delete(int id);
    }
}
