namespace ConfigurateService.Class.Interface
{
    internal interface IRepository
    {
        string DefaultConnectionString();
        string GetConnectionString();
    }
}
