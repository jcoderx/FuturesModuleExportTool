using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FuturesModuleExportTool
{
    public class ExcelExport
    {
        public void exportExcel(string[,] data)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("Sheet1");

            for (int i = 0; i < data.GetLength(0); i++)
            {
                HSSFRow row = (HSSFRow)sheet.CreateRow(i);
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    row.CreateCell(j).SetCellValue(data[i, j]);
                }
            }

            String filePath = Utils.getExportDir() + "original_" + Utils.getDate() + "_" + Utils.getTimeMillisecond() + ".xls";
            FileStream file = new FileStream(filePath, FileMode.Create);
            workbook.Write(file);
            file.Close();
        }

        private ICellStyle commonStyle;
        private ICellStyle dateStyle;
        private ICellStyle profitLossStyle;
        private ICellStyle profitStyle;
        private ICellStyle holdErrorStyle;
        private ICellStyle commonNumberStyle;
        private ICellStyle titleStyle0;
        private ICellStyle titleStyle1;
        private ICellStyle titleStyle2;
        private ICellStyle titleStyle3;
        private ICellStyle titleStyle4;

        private HSSFPalette palette;

        private void initStyle(HSSFWorkbook workbook)
        {
            commonStyle = createCommonStyle(workbook);
            dateStyle = createDateStyle(workbook);
            profitLossStyle = createProfitLossStyle(workbook);
            profitStyle = createProfitStyle(workbook);
            holdErrorStyle = createHoldErrorStyle(workbook);
            commonNumberStyle = createCommonNumberStyle(workbook);

            titleStyle0 = createTitleStyle(workbook, 0);
            titleStyle1 = createTitleStyle(workbook, 1);
            titleStyle2 = createTitleStyle(workbook, 2);
            titleStyle3 = createTitleStyle(workbook, 3);
            titleStyle4 = createTitleStyle(workbook, 4);
        }


        public void exportExcel(List<string[,]> data, string dirName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            palette = workbook.GetCustomPalette();
            initColor();
            initStyle(workbook);
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("Sheet1");


            setColumnWidth(sheet);

            //日期
            //当日盈亏
            //有效信号数
            //浮动盈亏
            //子账户持仓
            //理论持仓
            //滑点损耗
            for (int i = 0; i < 7; i++)
            {
                HSSFRow row = (HSSFRow)sheet.CreateRow(i);
                setRowHeight(row);
            }
            HSSFRow row0 = (HSSFRow)sheet.GetRow(0);
            HSSFRow row1 = (HSSFRow)sheet.GetRow(1);
            HSSFRow row2 = (HSSFRow)sheet.GetRow(2);
            HSSFRow row3 = (HSSFRow)sheet.GetRow(3);
            HSSFRow row4 = (HSSFRow)sheet.GetRow(4);
            HSSFRow row5 = (HSSFRow)sheet.GetRow(5);
            HSSFRow row6 = (HSSFRow)sheet.GetRow(6);

            HSSFCell cell = (HSSFCell)row0.CreateCell(0);
            cell.SetCellValue(Utils.getDate());
            cell.CellStyle = dateStyle;
            cell = (HSSFCell)row1.CreateCell(0);
            cell.SetCellValue("当日盈亏");
            cell.CellStyle = profitLossStyle;
            cell = (HSSFCell)row2.CreateCell(0);
            cell.SetCellValue("有效信号数");
            cell.CellStyle = commonStyle;
            cell = (HSSFCell)row3.CreateCell(0);
            cell.SetCellValue("浮动盈亏");
            cell.CellStyle = commonStyle;
            cell = (HSSFCell)row4.CreateCell(0);
            cell.SetCellValue("子账户持仓");
            cell.CellStyle = commonStyle;
            cell = (HSSFCell)row5.CreateCell(0);
            cell.SetCellValue("理论持仓");
            cell.CellStyle = commonStyle;
            cell = (HSSFCell)row6.CreateCell(0);
            cell.SetCellValue("滑点损耗");
            cell.CellStyle = commonStyle;

            int column = 1;
            for (int i = 0; i < data.Count; i++)
            {
                ICellStyle style;
                switch (i % 5)
                {
                    case 0:
                        style = titleStyle0;
                        break;
                    case 1:
                        style = titleStyle1;
                        break;
                    case 2:
                        style = titleStyle2;
                        break;
                    case 3:
                        style = titleStyle3;
                        break;
                    case 4:
                        style = titleStyle4;
                        break;
                    default:
                        style = commonStyle;
                        break;
                }
                string[,] partitionData = data[i];
                for (int j = 0; j < partitionData.GetLength(0); j++)
                {
                    //周期+模型
                    cell = (HSSFCell)row0.CreateCell(column);
                    string cycle = partitionData[j, 4];
                    string model = partitionData[j, 5];
                    setCycleModelCell(cell, cycle, model);
                    cell.CellStyle = style;
                    //当日盈亏，对应平仓盈亏
                    cell = (HSSFCell)row1.CreateCell(column);
                    int result;
                    bool isInt = Utils.convertToInt(partitionData[j, 17], out result);
                    if (isInt)
                    {

                        if (result > 0)
                        {
                            cell.SetCellValue(result);
                            cell.CellStyle = profitStyle;
                        }
                        else if (result < 0)
                        {
                            cell.SetCellValue(result);
                            cell.CellStyle = commonNumberStyle;
                        }
                        else
                        {
                            //result == 0
                        }
                    }
                    //有效信号书
                    cell = (HSSFCell)row2.CreateCell(column);
                    isInt = Utils.convertToInt(partitionData[j, 19], out result);
                    if (isInt)
                    {
                        if (result != 0)
                        {
                            cell.SetCellValue(result);
                        }
                    }
                    cell.CellStyle = commonStyle;
                    //浮动盈亏
                    cell = (HSSFCell)row3.CreateCell(column);
                    isInt = Utils.convertToInt(partitionData[j, 18], out result);
                    if (isInt)
                    {

                        if (result > 0)
                        {
                            cell.SetCellValue(result);
                            cell.CellStyle = profitStyle;
                        }
                        else if (result < 0)
                        {
                            cell.SetCellValue(result);
                            cell.CellStyle = commonNumberStyle;
                        }
                        else
                        {
                            // result == 0
                            string subaccountHoldx = partitionData[j, 13];
                            if (!string.IsNullOrEmpty(subaccountHoldx))
                            {
                                cell.SetCellValue(0);
                                cell.CellStyle = commonNumberStyle;
                            }
                        }
                    }

                    //子账户持仓和理论持仓，若子账户持仓和理论持仓数据不一致，说明数据存在问题
                    ICell subaccountHoldCell = (HSSFCell)row4.CreateCell(column);
                    ICell theoryHoldCell = (HSSFCell)row5.CreateCell(column);
                    bool holdError = false;
                    string subaccountHold = partitionData[j, 13];
                    string theoryHold = partitionData[j, 14];
                    string signalPerform = partitionData[j, 9];
                    if (!string.IsNullOrEmpty(signalPerform) && signalPerform.Contains("正在执行"))
                    {
                        Console.WriteLine("正在执行");
                        holdError = true;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(subaccountHold))
                        {
                            holdError = false;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(theoryHold))
                            {
                                holdError = false;
                            }
                            else
                            {
                                if (subaccountHold.Contains(theoryHold))
                                {
                                    //空 1/1
                                    if (subaccountHold.Length > 2 && subaccountHold.Contains("/"))
                                    {
                                        string temp = subaccountHold.Substring(2);
                                        string[] tempArray = temp.Split('/');
                                        if (tempArray.Length >= 2 && tempArray[0].Equals(tempArray[1]))
                                        {
                                            holdError = false;
                                        }
                                        else
                                        {
                                            holdError = true;
                                        }
                                    }
                                    else
                                    {
                                        holdError = true;
                                    }
                                }
                                else
                                {
                                    if (subaccountHold.Length > 2 && subaccountHold.Contains("/"))
                                    {
                                        string temp = subaccountHold.Substring(2);
                                        string[] tempArray = temp.Split('/');
                                        if (tempArray.Length >= 2 && tempArray[0].Equals(tempArray[1]))
                                        {
                                            // 看动态
                                            string dynamic = partitionData[j, 6];
                                            Console.WriteLine(dynamic);
                                            if (string.IsNullOrEmpty(dynamic))
                                            {
                                                holdError = true;
                                            }
                                            else
                                            {
                                                holdError = false;
                                            }
                                        }
                                        else
                                        {
                                            holdError = true;
                                        }
                                    }
                                    else
                                    {
                                        holdError = true;
                                    }
                                }
                            }
                        }
                    }

                    if (holdError)
                    {
                        subaccountHoldCell.CellStyle = holdErrorStyle;
                        theoryHoldCell.CellStyle = holdErrorStyle;
                    }
                    else
                    {
                        subaccountHoldCell.CellStyle = commonStyle;
                        theoryHoldCell.CellStyle = commonStyle;
                    }
                    subaccountHoldCell.SetCellValue(convertHold(subaccountHold));
                    theoryHoldCell.SetCellValue(convertHold(theoryHold));

                    //滑点损耗
                    cell = (HSSFCell)row6.CreateCell(column);
                    isInt = Utils.convertToInt(partitionData[j, 21], out result);
                    if (isInt)
                    {
                        if (result != 0)
                        {
                            cell.SetCellValue(result);
                        }
                    }
                    cell.CellStyle = commonStyle;
                    //end
                    column++;
                }
            }

            String filePath = Utils.getExportDir() + Utils.getDate() + "_" + dirName + "_" + Utils.getTimeMillisecond() + ".xls";
            FileStream file = new FileStream(filePath, FileMode.Create);
            workbook.Write(file);
            file.Close();
        }

        private void initColor()
        {
            //8 黑
            palette.SetColorAtIndex(8, 0, 0, 0);
            //9 红
            palette.SetColorAtIndex(9, 255, 0, 0);
            //10 蓝
            palette.SetColorAtIndex(10, 0, 0, 255);
            //11 黄
            palette.SetColorAtIndex(11, 255, 255, 0);
            //12 深红
            palette.SetColorAtIndex(12, 192, 0, 0);
            // 13 青
            palette.SetColorAtIndex(13, 84, 130, 53);
            // 14 粉
            palette.SetColorAtIndex(14, 255, 0, 255);
        }


        private ICellStyle createCommonStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            style.SetFont(font);
            return style;
        }

        private ICellStyle createCommonNumberStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            style.SetFont(font);
            style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0");
            return style;
        }


        private ICellStyle createDateStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            IFont font = workbook.CreateFont();
            font.Color = 10;
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            style.SetFont(font);
            return style;
        }

        private ICellStyle createProfitLossStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            IFont font = workbook.CreateFont();
            font.Color = 9;
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            style.SetFont(font);
            return style;
        }

        private ICellStyle createTitleStyle(HSSFWorkbook workbook, int index)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            IFont font = workbook.CreateFont();
            switch (index)
            {
                case 0:
                    font.Color = 12;
                    break;
                case 1:
                    font.Color = 10;
                    break;
                case 2:
                    font.Color = 8;
                    break;
                case 3:
                    font.Color = 13;
                    break;
                case 4:
                    font.Color = 14;
                    break;
            }
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            style.SetFont(font);
            return style;
        }

        private ICellStyle createProfitStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            IFont font = workbook.CreateFont();
            font.Color = 9;
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            style.SetFont(font);
            style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0");
            return style;
        }

        private ICellStyle createHoldErrorStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            style.SetFont(font);
            style.FillForegroundColor = 11;
            style.FillPattern = FillPattern.SolidForeground;
            return style;
        }

        private void setColumnWidth(HSSFSheet sheet)
        {
            for (int i = 0; i < 150; i++)
            {
                sheet.SetColumnWidth(i, 12 * 256);
            }
        }

        private void setRowHeight(HSSFRow row)
        {
            row.Height = 270;
        }


        //-----------------------------------------------------------------------
        //设置周期+模型，分钟=>分；小时=>H
        private void setCycleModelCell(ICell cell, string cycle, string model)
        {
            if (!string.IsNullOrEmpty(cycle))
            {
                if (cycle.Contains("分钟"))
                {
                    cycle = cycle.Replace("分钟", "分");
                }
                if (cycle.Contains("小时"))
                {
                    cycle = cycle.Replace("小时", "H");
                }
                //秒、日不变
            }
            cell.SetCellValue(cycle + model);
        }

        //持仓转换
        private string convertHold(string hold)
        {
            if (string.IsNullOrEmpty(hold))
            {
                return hold;
            }
            return hold.Replace("空", "S").Replace("多", "B");
        }
    }
}
