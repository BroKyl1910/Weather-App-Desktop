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
        this.UserType = UserType.GeneralUser;
    }

    public User(string username, string password, UserType userType)
    {
        Username = username;
        Password = password;
        this.UserType = userType;
    }

    public string GetTextFileFormat() // returns string to write to text file to properly represent forecast
    {
        return Username + "," + Password+ "," + (int) this.UserType;
    }

}
