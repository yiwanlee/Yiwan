namespace RowversionTest.DAL
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 类 名 称：QedbBaseModel
    /// 类 说 明：数据库Model的基类，所有的Model类都继承此类，等价于PowerDesigner中的DEFAULTFIELD
    /// 内容摘要：创建人、创建时间、修改人、修改时间、是否可用
    /// 完成日期：2017/9/22
    /// 编码作者：赵立立 liley@foxmail.com
    /// </summary>
    public class QedbBaseModel
    {
        public QedbBaseModel()
        {
            QeCreatetime = DateTime.Now;
            QeUpdatetime = DateTime.Now;
            QeEnabled = 1;
            QeVersion = 1;
        }

        [Display(Name = "创建人")]
        [Column("QE_CREATEBY", Order = 91)]
        public string QeCreateby { get; set; }

        [Display(Name = "创建时间")]
        [Column("QE_CREATETIME", Order = 92, TypeName = "datetime2")]
        public DateTime QeCreatetime { get; set; }

        [Display(Name = "修改人")]
        [Column("QE_UPDATEBY", Order = 93)]
        public string QeUpdateby { get; set; }

        [Display(Name = "修改时间")]
        [Column("QE_UPDATETIME", Order = 94, TypeName = "datetime2")]
        public DateTime QeUpdatetime { get; set; }

        [Display(Name = "是否可用")]
        [Column("QE_ENABLED", Order = 95)]
        public int QeEnabled { get; set; }

        [Display(Name = "自增索引")]
        [Column("QE_INDEX", Order = 96)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QeIndex { get; set; }

        [Display(Name = "乐观锁")]
        [Column("QE_VERSION", Order = 97)]
        public int QeVersion { get; set; }
    }
}
