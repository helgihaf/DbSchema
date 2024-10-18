namespace Marsonsoft.DbSchema.Parsing.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Parse_Simple_Table()
        {
            var parser = new DbSchemaParser();
            var schema = parser.Parse(File.ReadAllText("./simpleTest/schema.yml"));
        }
    }
}