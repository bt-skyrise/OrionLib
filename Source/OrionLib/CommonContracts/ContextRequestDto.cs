using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionLib.CommonContracts
{
    public class ContextRequestDto
    {
        public ContextElementDto[] ContextElements { get; private set; }
        public string UpdateAction { get; private set; }

        private ContextRequestDto(string updateAction, IEnumerable<ContextElementDto> contextElements)
        {
            ContextElements = contextElements.ToArray();
            UpdateAction = updateAction;
        }

        public static ContextRequestDto RemoveAttributesRequest(string type, string id, IEnumerable<string> attributeNames)
        {
            return new ContextRequestDto(
                updateAction: "DELETE",
                contextElements: new[]
                {
                    new ContextElementDto
                    {
                        Id = id,
                        Type = type,
                        IsPattern = "false",
                        Attributes = attributeNames.Select(atr =>
                            new GetAttributeDto
                            {
                                Name = atr,
                                Type = "",
                                Value = ""
                            }).ToList()
                    },
                });
        }
    }
}
