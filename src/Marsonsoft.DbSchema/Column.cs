namespace Marsonsoft.DbSchema
{
    public record Column(Table Table, string Name, DataType DataType)
    {
    }
}