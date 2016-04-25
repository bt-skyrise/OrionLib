using System;
using System.Collections.Generic;
using System.Linq;

namespace OrionLib
{
    public class OrionEntity
    {
        private readonly OrionAttribute[] _attributes;

        public string Type { get; private set; }
        public string Id { get; private set; }

        public OrionEntity(string type, string id, IEnumerable<OrionAttribute> attributes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (attributes == null) throw new ArgumentNullException(nameof(attributes));

            Type = type;
            Id = id;

            _attributes = attributes.ToArray();

            if (HasDuplicateNames(_attributes))
            {
                throw new ArgumentException("Entity cannot have duplicate attributes.", nameof(attributes));
            }
        }

        public IEnumerable<OrionAttribute> Attributes => _attributes;

        public OrionAttribute GetAttributeByName(string name)
        {
            return _attributes.SingleOrDefault(a => a.Name == name);
        }

        public string GetAttributeValueByName(string name)
        {
            return GetAttributeByName(name).Value;
        }

        private bool HasDuplicateNames(OrionAttribute[] attributes)
        {
            var distinctNames = _attributes
                .Select(a => a.Name)
                .Distinct()
                .ToArray();

            return attributes.Length > distinctNames.Length;
        }
    }
}