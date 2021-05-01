using System;

namespace MTA.Core.Application.Models
{
    public sealed class PermissionModel<TPermission> where TPermission : Enum
    {
        public TPermission Permission { get; init; }
        public bool IsPermitted { get; private set; }

        public static PermissionModel<TPermission> Create(TPermission permission) =>
            new PermissionModel<TPermission> {Permission = permission};

        public PermissionModel<TPermission> AppendPermission(Func<bool> permissionCondition)
        {
            if (!IsPermitted)
                IsPermitted = IsPermitted || permissionCondition();

            return this;
        }
    }
}