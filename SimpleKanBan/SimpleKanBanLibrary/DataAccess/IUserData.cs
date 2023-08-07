namespace SimpleKanBanLibrary.DataAccess;

public interface IUserData
{
    Task CreateUser(UserModel user);
    Task<UserModel> GetUser(string id);
    Task<UserModel> GetUserFromAuthentication(string objectId);
    Task<List<UserModel>> GetAllUsers();
    Task UpdateUser(UserModel user);
}