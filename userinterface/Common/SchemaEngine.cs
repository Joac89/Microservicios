using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.IO;

namespace userinterface.Common
{
    public static class SchemaEngine
    {
        public static bool Validate<T>(T json, string jsonschema)
        {
            var isvalid = false;
            var jsonstring = JsonConvert.SerializeObject(json).ToLower();

            using (var file = File.OpenText(@jsonschema))
            using (var reader = new JsonTextReader(file))
            {
                var schema = JSchema.Load(reader);
                var validatingReader = new JSchemaValidatingReader(reader);
                var obj = JObject.Parse(jsonstring);

                isvalid = obj.IsValid(schema);
            }

            return isvalid;
        }
    }
}

