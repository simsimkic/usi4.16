namespace ZdravoCorp
{
    public class Credentials
    {
        public string Username { get; set; } //public set
        public string Password { get; set; } //public set

        public Credentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public void ChangeUsername(string username)
        {
            Username = username;
        }

        public void ChangePassword(string newPassword)
        {
            Password = newPassword;
        }

        public bool IsPasswordCorrect(string password)
        {
            return Password == password;
        }

    }
}
