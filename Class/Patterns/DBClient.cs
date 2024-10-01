namespace ConfigurateService.Class.Patterns
{
    class DBClient
    {
        internal string Version { get;private  set; }
        internal string Host_Server { get;private set; }
        internal string Database { get; private set; }
        internal string Login { get; private set; }
        internal string Password { get; private set; }
        internal DBClient(string version, string host_server, string database, string login, string password)
        {
            Version = version;
            Host_Server = host_server;
            Database = database;
            Login = login;
            Password = password;
        }
    }
}
