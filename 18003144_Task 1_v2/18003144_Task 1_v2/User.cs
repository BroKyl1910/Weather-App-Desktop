public enum UserType
{
    GeneralUser, //0
    Forecaster   //1
}

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public UserType UserType { get; set; } //0 = GeneralUser, 1 = Forecaster

    public User()
    {
        Username = "";
        Password = "";
        UserType = UserType.GeneralUser;
    }

    public User(string username, string password, UserType userType)
    {
        Username = username;
        Password = password;
        UserType = userType;
    }

}
