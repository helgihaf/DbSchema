namespace Marsonsoft.DbSchema
{
    public record Table(string Name, bool IsAbstract = false, Table? BaseTable = null)
    {
        public List<Column> Columns { get; } = [];
    }
}
