using Dapper;

namespace JobHubAPI.Model
{
    [Table("Candidates")]
    public class Candidate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public long PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string PreferredContactTime { get; set; }
        public string LinkedInProfile { get; set; }
        public string GitHubProfile { get; set; }
        [Required]
        public string Comment { get; set; }
        [IgnoreUpdate]
        public DateTime AddedOn { get; set; } = DateTime.Now;
        [IgnoreInsert]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
    public class CandidateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PreferredContactTime { get; set; }
        public string LinkedInProfile { get; set; }
        public string GitHubProfile { get; set; }
        public string Comment { get; set; }
    }
}
