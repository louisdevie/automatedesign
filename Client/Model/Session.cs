namespace AutomateDesign.Client.Model
{
    public class Session
    {
        private string token;
        private string userEmail;

        public string Token => this.token;

        public string UserEmail => this.userEmail;

        public Session(string token, string userEmail)
        {
            this.token = token;
            this.userEmail = userEmail;
        }
    }
}
