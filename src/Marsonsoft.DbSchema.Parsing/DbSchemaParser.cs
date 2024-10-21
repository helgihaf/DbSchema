using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;
using YamlDotNet.RepresentationModel;

namespace Marsonsoft.DbSchema.Parsing
{
    public class DbSchemaParser
    {
        public Schema Parse(string yml)
        {
            var yaml = new YamlStream();
            using (var reader = new StringReader(yml))
            {
                yaml.Load(reader);
            }

            return Parse(yaml);
        }

        private Schema Parse(YamlStream yaml)
        {
            var root = (YamlMappingNode)yaml.Documents[0].RootNode;

            var dataTypes = ParseDataTypes((YamlSequenceNode)root.Children[new YamlScalarNode("datatypes")]);
            var tables = ParseTables((YamlSequenceNode)root.Children[new YamlScalarNode("tables")], dataTypes);

            return new Schema(tables);
        }

        private static Dictionary<string, DataType> ParseDataTypes(YamlSequenceNode dataTypeNodes)
            => dataTypeNodes.OfType<YamlMappingNode>().Select(n => ParseDataType(n)).ToDictionary(d => d.Name);

        private static DataType ParseDataType(YamlMappingNode dataTypeNode)
        {
            var name = dataTypeNode.Children[new YamlScalarNode("name")].ToString();
            var nullable = ExtractBool(dataTypeNode.Children[new YamlScalarNode("nullable")], false);
            var primaryKey = ExtractBool(dataTypeNode.Children[new YamlScalarNode("nullable")], false);
            var platforms = ParsePlatforms((YamlMappingNode)dataTypeNode.Children[new YamlScalarNode("platforms")]);
            return new DataType(name, nullable, primaryKey, platforms);
        }

        private static bool ExtractBool(YamlNode yamlNode, bool defaultValue)
        {
            if (yamlNode is YamlScalarNode scalarNode && !string.IsNullOrEmpty(scalarNode.Value))
                return bool.Parse(scalarNode.Value);
            return defaultValue;
        }

        private static Dictionary<string, string> ParsePlatforms(YamlMappingNode platformsNode)
            =>
            platformsNode.Children.Select
            (
                p => new KeyValuePair<string, string>(p.Key.ToString(), p.Value.ToString())
            )
            .ToDictionary(p => p.Key, p => p.Value);

        private static Dictionary<string, Table> ParseTables(YamlSequenceNode tablesNode, Dictionary<string, DataType> dataTypes)
        {
            var tables = new Dictionary<string, Table>();
            foreach (var tableNode in tablesNode.OfType<YamlMappingNode>())
            {
                var table = ParseTable(tableNode, dataTypes, tables);
                tables.Add(table.Name, table);
            }

            return tables;
        }

        private static Table ParseTable(YamlMappingNode tableNode, Dictionary<string, DataType> dataTypes, Dictionary<string, Table> tables)
        {
            var name = tableNode.Children[new YamlScalarNode("name")].ToString();
            var isAbstract = ExtractBool(tableNode.Children[new YamlScalarNode("isAbstract")], false);
            
            var baseTableName = tableNode.Children[new YamlScalarNode("name")].ToString();
            Table? baseTable = null;
            if (baseTableName != null && !tables.TryGetValue(baseTableName, out baseTable))
                throw new InvalidOperationException($"Base table '{baseTableName}' not found.");
            
            var columns = ParseColumns((YamlSequenceNode)tableNode.Children[new YamlScalarNode("columns")], dataTypes);
            
            return new Table(name, columns, isAbstract, baseTable);
        }

        private static Dictionary<string, Column> ParseColumns(YamlSequenceNode columnsNode, IDictionary<string, DataType> dataTypes)
            => columnsNode.OfType<YamlMappingNode>().Select(c => ParseColumn(c, dataTypes)).ToDictionary(c => c.Name);

        private static Column ParseColumn(YamlMappingNode collumnNode, IDictionary<string, DataType> dataTypes)
        {
            throw new NotImplementedException();
        }
    }
}
