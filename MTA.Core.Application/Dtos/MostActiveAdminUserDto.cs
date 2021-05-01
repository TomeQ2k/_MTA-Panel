using System;

namespace MTA.Core.Application.Dtos
{
    public class MostActiveAdminUserDto
    {
        public string Username { get; set; }
        public DateTime LastLogin { get; set; }

        public int AdminRole { get; set; }
        public int SupporterRole { get; set; }
        public int VctRole { get; set; }
        public int MapperRole { get; set; }
        public int ScripterRole { get; set; }

        public int AdminReportsCount { get; set; }
    }
}