namespace Auth.Application.Constants;
public static class GeneralMessages
{
    public const string Success = "Request completed successfully.";
    public const string Failure = "Request failed. Please try again.";
}

public static class LoginMessages
{
    public const string LoginSuccess = "Login successful.";
    public const string InvalidLogin = "Invalid username or password.";
    public const string UserNotFound = "User not found.";
    public const string IncorrectPassword = "Incorrect password.";
}

public static class RegistrationMessages
{
    public const string RegistrationSuccess = "Registration successful.";
    public const string EmailTaken = "This email is already registered.";
    public const string UsernameTaken = "This username is taken.";
    public const string RegistrationFailed = "Registration failed.";
    public const string RoleNotAllowed = "This role is not allowed.";
}

public static class RequiredFieldsMessages
{
    public const string UsernameRequired = "Username is required.";
    public const string EmailRequired = "Email is required.";
    public const string PasswordRequired = "Password is required.";
}