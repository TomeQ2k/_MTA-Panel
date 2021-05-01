using System;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Dtos
{
    public class UserAdminListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Serial { get; set; }
        public bool Activated { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Ip { get; set; }
        public AppStateType AppState { get; set; }
        public bool IsBlocked { get; set; }
    }
}