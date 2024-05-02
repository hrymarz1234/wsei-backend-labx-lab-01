namespace BlazorApp.UserServices;

public class UserService : IUserService
{
    public Dictionary<string, string> Data = new Dictionary<string, string>();
    public void Add(string connectionId, string username)
    {
        Data.Add(connectionId, username);
    }

    public IEnumerable<(string ConnectionId, string Username)> GetAll()
    {
        return Data.Select(x => (x.Key, x.Value));
    }

    public string GetConnectioIdByName(string username)
    {
        var pair = Data.FirstOrDefault(x => x.Value == username);
        return pair.Key;
    }

    public void RemoveByName(string username)
    {
        var pairsToRemove = Data.Where(pair => pair.Value == username).ToList();
        foreach (var pair in pairsToRemove)
        {
            Data.Remove(pair.Key);
        }
    }
}
