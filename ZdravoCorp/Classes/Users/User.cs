namespace ZdravoCorp
{
    public class User
    {
        public User() { }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Credentials Credentials { get; set; }

        public User(string name, string surname, Credentials credentials)
        {
            Name = name;
            Surname = surname;
            Credentials = credentials;
        }
    }
}
