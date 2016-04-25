using System;
using System.Collections.Generic;
using System.Linq;

namespace OrionLib
{
    public class OrionAttribute
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Value { get; private set; }
        public IEnumerable<OrionAttributeMetadata> Metadatas => _metadatas; 

        private readonly OrionAttributeMetadata[] _metadatas;

        public OrionAttribute(string name, string type, string value, IEnumerable<OrionAttributeMetadata> metadatas = null)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (value == null) throw new ArgumentNullException(nameof(value));

            Name = name;
            Type = type;
            Value = value;
            _metadatas = metadatas?.ToArray() ?? new OrionAttributeMetadata[0];
        }
    }

    public class OrionAttributeMetadata
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Value { get; private set; }

        public OrionAttributeMetadata(string name, string type, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (value == null) throw new ArgumentNullException(nameof(value));

            Name = name;
            Type = type;
            Value = value;
        }
    }
}