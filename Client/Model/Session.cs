namespace AutomateDesign.Client.Model
{
    public class Session
    {
        private string token;
        private int userId;
        private string userEmail;

        public string Token => this.token;

        public int UserId => this.userId;

        public string UserEmail => this.userEmail;

        public Session(string token, int userId, string userEmail)
        {
            this.token = token;
            this.userId = userId;
            this.userEmail = userEmail;
        }
    }
}
