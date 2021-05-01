using System;
using MTA.Core.Application.Models;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record AddObjectProtectionResponse : BaseResponse
    {
        public ObjectProtectionType ProtectionType { get; init; }
        public int ObjectId { get; init; }
        public DateTime? ProtectedUntil { get; init; }

        public AddObjectProtectionResponse(Error error = null) : base(error)
        {
        }
    }
}