using System.ComponentModel.DataAnnotations;

namespace Edu.BLL.SchoolFinance
{
    /// <summary>
    /// generate card list configs model.
    /// </summary>
    public class CardGenParams
    {
        public string configId { get; set; }
        [Display(Name = "密码长度"), RegularExpression(@"^([6-9]|10)$", ErrorMessage = "长度6-10位")]
        [Required]
        public int pswLen { get; set; }
        public string cardPrefix { get; set; }
        [Display(Name = "卡号长度"), RegularExpression(@"^([5-9]|1[0-5])$", ErrorMessage = "长度5-15位")]
        [Required]
        public int cardNoLen { get; set; }
        [Display(Name = "生成数量"), RegularExpression(@"^([1-9][0-9]{0,2}|1000)$", ErrorMessage = "数量在1-1000张")]
        [Required(ErrorMessage = "应该为1~1000的数字")]
        public int count { get; set; }
    }

}
