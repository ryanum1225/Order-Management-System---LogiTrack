namespace LogiTrack.Dto
{
    public class AddRoleToUserDTO
    {
        public string Email { get; set; }

        public string Role { get; set; } = null!;
    }
}