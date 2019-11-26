namespace RowversionTest.DAL
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 类 名 称：GamDogluckyrec    
    /// 类 说 明：GAM_DOGLUCKYREC 游戏_狗屎运记录
    /// 内容摘要：表[GAM_DOGLUCKYREC]对应的实体类
    /// 完成日期：2019/06/27
    /// 编码作者：赵立立 liley@foxmail.com QQ:897250000
    /// </summary>
    [Table("GAM_DOGLUCKYREC")]
    public class GamDogluckyrec : QedbBaseModel
    {
        public GamDogluckyrec()
        {
            GamDlrId = Guid.NewGuid();
            GamDlrId2 = Guid.NewGuid();
            GamDltId = Guid.NewGuid();
            GamDlkId = Guid.NewGuid();
            UscWrUnionid = "";
            GamDlrNumber = -1;
        }

        /// <summary>
        /// 狗屎记录GUID
        /// </summary>
        [Key]
        [Display(Name = "狗屎记录GUID", Order = 1)]
        [Column("GAM_DLR_ID")]
        public Guid GamDlrId { get; set; }


        [Display(Name = "狗屎记录GUID", Order = 1)]
        [Column("GAM_DLR_ID2")]
        public Guid GamDlrId2 { get; set; }

        /// <summary>
        /// 狗屎队伍GUID
        /// </summary>
        [Required]
        [Display(Name = "狗屎队伍GUID")]
        [Column("GAM_DLT_ID")]
        public Guid GamDltId { get; set; }

        /// <summary>
        /// 狗屎运GUID
        /// </summary>
        [Required]
        [Display(Name = "狗屎运GUID")]
        [Column("GAM_DLK_ID")]
        public Guid GamDlkId { get; set; }

        /// <summary>
        /// 选择人UNIONID
        /// </summary>
        [Required]
        [Display(Name = "选择人UNIONID")]
        [Column("USC_WR_UNIONID")]
        [StringLength(100)]
        public string UscWrUnionid { get; set; }

        /// <summary>
        /// 选择号码
        /// </summary>
        [Required]
        [Display(Name = "选择号码")]
        [Column("GAM_DLR_NUMBER")]
        public int GamDlrNumber { get; set; }

        /// <summary>
        /// 订单GUID
        /// </summary>
        [Display(Name = "订单GUID")]
        [Column("MALL_TRA_ID")]
        public Guid? MallTraId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [EmailAddress]
        [Display(Name = "订单编号")]
        [Column("MALL_TRA_ORDERID")]
        [StringLength(500)]
        public string MallTraOrderid { get; set; }

        [Timestamp]
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] revision { get; set; }

        public long revison_long
        {
            get
            {
                return Convert.ToInt64(BitConverter.ToString(revision).Replace("-", ""), 16);
            }
        }
    }
}