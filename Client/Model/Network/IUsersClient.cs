namespace AutomateDesign.Client.Model.Network
{
    internal interface IUsersClient
    {
        public int SignUp(string email, string password);
    }
}
