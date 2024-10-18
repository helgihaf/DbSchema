using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private List<DataType> ParseDataTypes(YamlSequenceNode dataTypeNodes)
            => dataTypeNodes.OfType<YamlMappingNode>().Select(n => ParseDataType(n)).ToList();

        private DataType ParseDataType(YamlMappingNode dataTypeNode)
        {
            var name = dataTypeNode.Children[new YamlScalarNode("name")];
        }
    }
}
