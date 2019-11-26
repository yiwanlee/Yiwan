using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowversionTest.DAL;

namespace Rowversion.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid dlrID = Guid.Parse("76BA4B7A-E547-4F4E-B1AB-00010F99C802");
            for (int i = 1; i < 3; i++)
            {
                int idx = i;
                Task.Run(async () =>
                {
                    try
                    {
                        using (var db = new QebbDBContext())
                        {

                            var r = db.GamDogluckyrec.Where(q => q.GamDlrId == dlrID).FirstOrDefault();

                            string s = r.GamDlkId.ToString() + "|" + BitConverter.ToInt64(r.revision, 0) + "|" + r.QeVersion;
                            System.Console.WriteLine($"{idx}：查询结果{r.revison_long}：{s}：{Convert.ToInt64(BitConverter.ToString(r.revision).Replace("-", ""), 16)}");
                            System.Console.WriteLine($"{idx}：睡眠 {idx * 2} 秒");
                            await Task.Delay(idx * 2000).ConfigureAwait(false);
                            System.Console.WriteLine($"{idx}：睡眠完成");

                            r.QeVersion += 10;
                            r.MallTraOrderid = "a";
                            //r.GamDlrId2 = Guid.Parse("E2D17D54-F901-48F9-B32B-000C2E0E0E3B");

                            //db.Configuration.ValidateOnSaveEnabled = false;
                            int non = db.SaveChanges();
                            db.Configuration.ValidateOnSaveEnabled = true;

                            var r2 = db.GamDogluckyrec.Where(q => q.GamDlrId == dlrID).AsNoTracking().FirstOrDefault();
                            string s2 = r2.GamDlkId.ToString() + "|" + BitConverter.ToInt64(r2.revision, 0) + "|" + r2.QeVersion;
                            System.Console.WriteLine($"{idx}：执行结果：" + s2);
                        }
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                    {
                        System.Console.WriteLine($"{idx}：错误：" + ex.Message + "|" + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        System.Console.WriteLine($"{idx}：错误：" + ex.Message + "|" + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        var errors = ex.EntityValidationErrors.ToList();
                        
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"{idx}：错误：" + ex.Message + "|" + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                    }
                });

            }
            while (true) System.Console.ReadKey();
        }
    }
}
