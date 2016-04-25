using System;
using OrionLib.CommonContracts;

namespace OrionLib
{
    [Serializable]
    public class OrionException : Exception
    {
        public string Code { get; private set; }
        public string ReasonPhrase { get; private set; }
        public string Details { get; private set; }

        public OrionException(OrionStatusDto orionStatus)
            : base(orionStatus.Serialize())
        {
            Code = orionStatus.Code;
            ReasonPhrase = orionStatus.ReasonPhrase;
            Details = orionStatus.Details;
        }
    }
}