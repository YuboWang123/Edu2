using System.ComponentModel.DataAnnotations;


namespace Edu.Entity.Account
{
    /// <summary>
    /// for searching user in console.
    /// </summary>
    public class Aspnetuser 
    {
        [Key]public string Id { get; set; }
        [StringLength(300)] public string Avatar { get; set; }
        [StringLength(100)] public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        
    }
}