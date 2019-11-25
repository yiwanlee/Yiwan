using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Utilities.Office
{
    public static class Exceler
    {
        public static string DataTableToExcel(DataTable dt, string tableName, string path, List<int> errRowIdxs = null, List<int> moreRowIdxs = null, string filename = "")
        {
            try
            {
                //创建一个工作簿
                IWorkbook workbook = new HSSFWorkbook();

                //创建一个 sheet 表
                ISheet sheet = workbook.CreateSheet(tableName);

                //创建一行
                IRow rowH = sheet.CreateRow(0);

                //创建一个单元格
                ICell cell = null;

                Font headerFont = new Font("宋体", 24, FontStyle.Bold);
                #region 单元格样式
                IFont font = workbook.CreateFont(); //创建一个字体样式对象
                font.FontName = "宋体"; //和excel里面的字体对应
                //默认头部样式
                ICellStyle headerCellStyle = workbook.CreateCellStyle();
                headerCellStyle.SetFont(font); //将字体样式赋给样式对象
                headerCellStyle.FillPattern = FillPattern.SolidForeground;
                headerCellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                //原始头部样式
                ICellStyle headerOrgCellStyle = workbook.CreateCellStyle();
                //默认样式 无
                ICellStyle cellStyle = workbook.CreateCellStyle();
                //特殊样式 背景 红
                ICellStyle cellRedStyle = workbook.CreateCellStyle();
                cellRedStyle.FillPattern = FillPattern.SolidForeground;
                cellRedStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
                //特殊样式 背景 黄波点
                ICellStyle cellYellowStyle = workbook.CreateCellStyle();
                cellYellowStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                cellYellowStyle.FillPattern = FillPattern.AltBars;// FillPatternType.ALT_BARS;
                cellYellowStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Rose.Index;
                //特殊样式 背景 绿波点
                ICellStyle cellGreenStyle = workbook.CreateCellStyle();
                cellGreenStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Lime.Index;
                cellGreenStyle.FillPattern = FillPattern.LessDots;// FillPatternType.LESS_DOTS;
                cellGreenStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                //特殊样式 背景 粉波点
                ICellStyle cellPinkStyle = workbook.CreateCellStyle();
                cellPinkStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                cellPinkStyle.FillPattern = FillPattern.LeastDots;// FillPatternType.LEAST_DOTS;
                cellPinkStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Rose.Index;
                #endregion 单元格样式

                //创建格式
                IDataFormat dataFormat = workbook.CreateDataFormat();

                //设置为文本格式，也可以为 text，即 dataFormat.GetFormat("text");
                cellStyle.DataFormat = dataFormat.GetFormat("@");

                //设置列宽
                for (int i = 0; i < dt.Columns.Count; i++) sheet.SetColumnWidth(i, 40 * 100);

                //设置列名
                foreach (DataColumn col in dt.Columns)
                {
                    //创建单元格并设置单元格内容
                    rowH.CreateCell(col.Ordinal).SetCellValue(col.Caption.Trim());
                    //设置单元格格式
                    rowH.Cells[col.Ordinal].CellStyle = headerCellStyle;
                }

                //写入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //跳过第一行，第一行为列名
                    IRow row = sheet.CreateRow(i + 1);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                        cell.CellStyle = cellStyle;

                        if (moreRowIdxs != null && moreRowIdxs.IndexOf(i) != -1) cell.CellStyle = cellGreenStyle;
                        if (errRowIdxs != null && errRowIdxs.IndexOf(i) != -1) cell.CellStyle = cellPinkStyle;
                    }
                }

                //设置导出文件路径
                //string path = HttpContext.Server.MapPath("/Export/");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                //设置新建文件路径及名称 
                filename = filename.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + (new Random().Next(1000, 9999)) + ".xls" : filename.Replace(".xlsx", ".xls");
                string savePath = path + (path.EndsWith("/") ? "" : "/") + filename;

                //创建文件
                FileStream file = new FileStream(savePath, FileMode.CreateNew, FileAccess.Write);

                //创建一个 IO 流
                MemoryStream ms = new MemoryStream();

                //写入到流
                workbook.Write(ms);

                //转换为字节数组
                byte[] bytes = ms.ToArray();

                file.Write(bytes, 0, bytes.Length);
                file.Flush();

                //释放资源
                bytes = null;

                ms.Close();
                ms.Dispose();

                file.Close();
                file.Dispose();

                workbook.Close();
                sheet = null;
                workbook = null;
                return filename;
            }
            catch (Exception ex)
            {
                var x = ex;
                return "";
            }
        }

        public static byte[] DataTableToExcel(DataTable dt, string sheetName, List<int> errRowIdxs = null, List<int> moreRowIdxs = null)
        {
            try
            {
                //创建一个工作簿
                IWorkbook workbook = new HSSFWorkbook();

                //创建一个 sheet 表
                ISheet sheet = workbook.CreateSheet(sheetName);

                //创建一行
                IRow rowH = sheet.CreateRow(0);

                //创建一个单元格
                ICell cell = null;

                Font headerFont = new Font("宋体", 24, FontStyle.Bold);
                #region 单元格样式
                IFont font = workbook.CreateFont(); //创建一个字体样式对象
                font.FontName = "宋体"; //和excel里面的字体对应
                //默认头部样式
                ICellStyle headerCellStyle = workbook.CreateCellStyle();
                headerCellStyle.SetFont(font); //将字体样式赋给样式对象
                headerCellStyle.FillPattern = FillPattern.SolidForeground;
                headerCellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                //原始头部样式
                ICellStyle headerOrgCellStyle = workbook.CreateCellStyle();
                //默认样式 无
                ICellStyle cellStyle = workbook.CreateCellStyle();
                //特殊样式 背景 红
                ICellStyle cellRedStyle = workbook.CreateCellStyle();
                cellRedStyle.FillPattern = FillPattern.SolidForeground;
                cellRedStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
                //特殊样式 背景 黄波点
                ICellStyle cellYellowStyle = workbook.CreateCellStyle();
                cellYellowStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                cellYellowStyle.FillPattern = FillPattern.AltBars;// FillPatternType.ALT_BARS;
                cellYellowStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Rose.Index;
                //特殊样式 背景 绿波点
                ICellStyle cellGreenStyle = workbook.CreateCellStyle();
                cellGreenStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Lime.Index;
                cellGreenStyle.FillPattern = FillPattern.LessDots;// FillPatternType.LESS_DOTS;
                cellGreenStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                //特殊样式 背景 粉波点
                ICellStyle cellPinkStyle = workbook.CreateCellStyle();
                cellPinkStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                cellPinkStyle.FillPattern = FillPattern.LeastDots;// FillPatternType.LEAST_DOTS;
                cellPinkStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Rose.Index;
                #endregion 单元格样式

                //创建格式
                IDataFormat dataFormat = workbook.CreateDataFormat();

                //设置为文本格式，也可以为 text，即 dataFormat.GetFormat("text");
                cellStyle.DataFormat = dataFormat.GetFormat("@");

                //设置列宽
                for (int i = 0; i < dt.Columns.Count; i++) sheet.SetColumnWidth(i, 40 * 100);

                //设置列名
                foreach (DataColumn col in dt.Columns)
                {
                    //创建单元格并设置单元格内容
                    rowH.CreateCell(col.Ordinal).SetCellValue(col.Caption.Trim());
                    //设置单元格格式
                    rowH.Cells[col.Ordinal].CellStyle = headerCellStyle;
                }

                //写入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //跳过第一行，第一行为列名
                    IRow row = sheet.CreateRow(i + 1);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                        cell.CellStyle = cellStyle;

                        if (moreRowIdxs != null && moreRowIdxs.IndexOf(i) != -1) cell.CellStyle = cellGreenStyle;
                        if (errRowIdxs != null && errRowIdxs.IndexOf(i) != -1) cell.CellStyle = cellPinkStyle;
                    }
                }

                //创建一个 IO 流
                MemoryStream ms = new MemoryStream();

                //写入到流
                workbook.Write(ms);

                //转换为字节数组
                byte[] bytes = ms.ToArray();

                ms.Close();
                ms.Dispose();

                workbook.Close();
                sheet = null;
                workbook = null;
                return bytes;
            }
            catch (Exception ex)
            {
                NLogger.Current.Error(ex);
                return null;
            }
        }

        public static DataTable ExcelToDataTable(byte[] excelData, string extension, bool first = true)
        {
            try
            {
                DataTable dt = new DataTable();
                IWorkbook wk = null;
                using (Stream stream = new MemoryStream(excelData))
                {
                    if (!extension.ToLower().Equals(".xlsx") && !extension.ToLower().Equals(".xls")) return null;
                    //判断excel的版本
                    if (extension == ".xlsx") wk = new XSSFWorkbook(stream);
                    else wk = new HSSFWorkbook(stream);

                    //获取第一个sheet
                    ISheet sheet = wk.GetSheetAt(0);
                    //获取第一行
                    IRow headrow = sheet.GetRow(0);
                    int maxColumn = 0;
                    //创建列
                    for (int i = headrow.FirstCellNum; i < headrow.Cells.Count; i++)
                    {
                        headrow.Cells[i].SetCellType(CellType.String);
                        var colname = headrow.Cells[i].StringCellValue.Trim();
                        if (colname.Equals("")) break;
                        maxColumn = i;
                        DataColumn datacolum = new DataColumn(ColumnNameNoLike(dt, colname));
                        dt.Columns.Add(datacolum);
                    }
                    //读取每行,从第二行起
                    for (int r = 1; r <= sheet.LastRowNum; r++)
                    {
                        bool result = false;
                        DataRow dr = dt.NewRow();
                        //获取当前行
                        IRow row = sheet.GetRow(r);
                        if (row == null) break; //当发现行为空，则认为触底
                                                //读取每列
                        for (int j = 0; j <= maxColumn; j++)
                        {
                            ICell cell = row.GetCell(j); //一个单元格
                            dr[j] = GetCellValue(cell); //获取单元格的值
                                                        //全为空则不取
                            if (!string.IsNullOrWhiteSpace(dr[j].ToString().Trim())) result = true;
                        }
                        if (result == false) break; //当发现一行全为空，则认为触底
                        dt.Rows.Add(dr); //把每行追加到DataTable
                    }

                    return dt;
                }
            }
            catch (Exception ex)
            {
                if (first) return ExcelToDataTable(excelData, extension == ".xlsx" ? ".xls" : ".xlsx", false);
                else { NLogger.Current.Error(ex); return null; }
            }
        }

        private static string GetCellValue(ICell cell)
        {
            if (cell == null) return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank: //空数据类型 这里类型注意一下，不同版本NPOI大小写可能不一样,有的版本是Blank（首字母大写)
                    return string.Empty;
                case CellType.Boolean: //bool类型
                    return cell.BooleanCellValue.ToString().Trim();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString().Trim();
                case CellType.Numeric: //数字类型
                    if (HSSFDateUtil.IsCellDateFormatted(cell))//日期类型
                    {
                        return cell.DateCellValue.ToString().Trim();
                    }
                    else //其它数字
                    {
                        return cell.NumericCellValue.ToString().Trim();
                    }
                case CellType.Unknown: //无法识别类型
                default: //默认类型
                    return cell.ToString().Trim();
                case CellType.String: //string 类型
                    return cell.StringCellValue.Trim();
                case CellType.Formula: //带公式类型
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString().Trim();
                    }
                    catch
                    {
                        try
                        {
                            if (HSSFDateUtil.IsCellDateFormatted(cell))//日期类型
                            {
                                return cell.DateCellValue.ToString().Trim();
                            }
                            else //其它数字
                            {
                                var nc = cell.NumericCellValue.ToString().Trim();
                                return nc.Equals("0") || nc.Equals("0.0") ? "" : nc;
                            }
                        }
                        catch (Exception ex)
                        {
                            var x = ex;
                            return "";
                        }
                    }
            }
        }

        /// <summary>
        /// 获取一个不重复的列名，如果重复+1+2
        /// </summary>
        private static string ColumnNameNoLike(DataTable dt, string colName, int num = 0)
        {
            for (int i = 0; i < dt.Columns.Count; i++)  //如果有重复，则列明加“-2”标注
            {
                if (dt.Columns[i].ColumnName == colName + (num == 0 ? "" : num.ToString()))
                {
                    return ColumnNameNoLike(dt, colName, num + 1);
                }
            }
            return colName + (num == 0 ? "" : num.ToString());
        }
    }
}
