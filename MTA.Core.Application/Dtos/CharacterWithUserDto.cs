namespace MTA.Core.Application.Dtos
{
    public class CharacterWithUserDto
    {
        public int UserId { get; set; }
        public int CharacterId { get; set; }
        public string Username { get; set; }
        public string Charactername { get; set; }
    }
}