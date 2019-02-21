using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace FuturesModuleExportTool
{
    public class ExcelExport
    {
        public void exportExcel(string[,] data)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet("Sheet1");

            for (int i = 0; i < data.GetLength(0); i++)
            {
                XSSFRow row = (XSSFRow)sheet.CreateRow(i);
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    row.CreateCell(j).SetCellValue(data[i, j]);
                }
            }

            String filePath = Utils.getExportDir() + "original_" + Utils.getDate() + "_" + Utils.getTimeMillisecond() + ".xlsx";
            FileStream file = new FileStream(filePath, FileMode.Create);
            workbook.Write(file);
            file.Close();
        }

        private XSSFCellStyle commonStyle;
        private XSSFCellStyle dateStyle;
        private XSSFCellStyle profitLossStyle;
        private XSSFCellStyle profitStyle;
        private XSSFCellStyle holdErrorStyle;
        private XSSFCellStyle commonNumberStyle;
        private XSSFCellStyle titleStyle0;
        private XSSFCellStyle titleStyle1;
        private XSSFCellStyle titleStyle2;
        private XSSFCellStyle titleStyle3;
        private XSSFCellStyle titleStyle4;

        private XSSFColor color0;//黑
        private XSSFColor color1;//红
        private XSSFColor color2;//蓝
        private XSSFColor color3;//黄
        private XSSFColor color4;//深红
        private XSSFColor color5;//青
        private XSSFColor color6;//粉

        private void initStyle(XSSFWorkbook workbook)
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

        private void initColor()
        {
            //黑
            color0 = new XSSFColor();
            color0.SetRgb(new byte[] { 0, 0, 0 });
            //红
            color1 = new XSSFColor();
            color1.SetRgb(new byte[] { 255, 0, 0 });
            //蓝
            color2 = new XSSFColor();
            color2.SetRgb(new byte[] { 0, 0, 255 });
            //黄
            color3 = new XSSFColor();
            color3.SetRgb(new byte[] { 255, 255, 0 });
            //深红
            color4 = new XSSFColor();
            color4.SetRgb(new byte[] { 192, 0, 0 });
            //青
            color5 = new XSSFColor();
            color5.SetRgb(new byte[] { 84, 130, 53 });
            //粉
            color6 = new XSSFColor();
            color6.SetRgb(new byte[] { 255, 0, 255 });
        }


        public void exportExcel(List<string[,]> data, string dirName)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            initColor();
            initStyle(workbook);
            XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet("Sheet1");

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
                XSSFRow row = (XSSFRow)sheet.CreateRow(i);
                setRowHeight(row);
            }
            XSSFRow row0 = (XSSFRow)sheet.GetRow(0);
            XSSFRow row1 = (XSSFRow)sheet.GetRow(1);
            XSSFRow row2 = (XSSFRow)sheet.GetRow(2);
            XSSFRow row3 = (XSSFRow)sheet.GetRow(3);
            XSSFRow row4 = (XSSFRow)sheet.GetRow(4);
            XSSFRow row5 = (XSSFRow)sheet.GetRow(5);
            XSSFRow row6 = (XSSFRow)sheet.GetRow(6);

            XSSFCell cell = (XSSFCell)row0.CreateCell(0);
            cell.SetCellValue(Utils.getDate());
            cell.CellStyle = dateStyle;
            cell = (XSSFCell)row1.CreateCell(0);
            cell.SetCellValue("当日盈亏");
            cell.CellStyle = profitLossStyle;
            cell = (XSSFCell)row2.CreateCell(0);
            cell.SetCellValue("有效信号数");
            cell.CellStyle = commonStyle;
            cell = (XSSFCell)row3.CreateCell(0);
            cell.SetCellValue("浮动盈亏");
            cell.CellStyle = commonStyle;
            cell = (XSSFCell)row4.CreateCell(0);
            cell.SetCellValue("子账户持仓");
            cell.CellStyle = commonStyle;
            cell = (XSSFCell)row5.CreateCell(0);
            cell.SetCellValue("理论持仓");
            cell.CellStyle = commonStyle;
            cell = (XSSFCell)row6.CreateCell(0);
            cell.SetCellValue("滑点损耗");
            cell.CellStyle = commonStyle;

            int column = 1;
            for (int i = 0; i < data.Count; i++)
            {
                XSSFCellStyle style;
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
                    cell = (XSSFCell)row0.CreateCell(column);
                    string cycle = partitionData[j, 4];
                    string agreement = partitionData[j, 2];
                    string model = partitionData[j, 5];
                    setCycleModelCell(cell, cycle, agreement, model);
                    cell.CellStyle = style;
                    //当日盈亏，对应平仓盈亏
                    cell = (XSSFCell)row1.CreateCell(column);
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
                    cell = (XSSFCell)row2.CreateCell(column);
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
                    cell = (XSSFCell)row3.CreateCell(column);
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
                    ICell subaccountHoldCell = (XSSFCell)row4.CreateCell(column);
                    ICell theoryHoldCell = (XSSFCell)row5.CreateCell(column);
                    bool holdError = false;
                    string subaccountHold = partitionData[j, 13];
                    string theoryHold = partitionData[j, 14];
                    string signalPerform = partitionData[j, 9];
                    if (!string.IsNullOrEmpty(signalPerform) && signalPerform.Contains("正在执行"))
                    {
                        holdError = true;
                    }
                    else
                    {
                        // 动态
                        string dynamic = partitionData[j, 6];
                        //下单信号
                        string orderSignal = partitionData[j, 7];
                        if (string.IsNullOrEmpty(dynamic))
                        {

                            if (string.IsNullOrEmpty(subaccountHold))
                            {
                                if (string.IsNullOrEmpty(theoryHold))
                                {
                                    holdError = false;
                                }
                                else
                                {
                                    //子账户持仓为空，理论持仓不为空
                                    holdError = true;
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(theoryHold))
                                {
                                    //子账户持仓不为空，理论持仓为空
                                    holdError = true;
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
                                                holdError = true;
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
                        else
                        {
                            if (dynamic.Equals(orderSignal))
                            {
                                holdError = false;
                            }
                            else
                            {
                                holdError = true;
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
                    cell = (XSSFCell)row6.CreateCell(column);
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

            String filePath = Utils.getExportDir() + Utils.getDate() + "_" + dirName + "_" + Utils.getTimeMillisecond() + ".xlsx";
            FileStream file = new FileStream(filePath, FileMode.Create);
            workbook.Write(file);
            file.Close();
        }

        private XSSFCellStyle createCommonStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            style.SetFont(font);
            return style;
        }

        private XSSFCellStyle createCommonNumberStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            style.SetFont(font);
            XSSFDataFormat format = (XSSFDataFormat)workbook.CreateDataFormat();
            style.DataFormat = format.GetFormat("0");
            return style;
        }


        private XSSFCellStyle createDateStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            font.SetColor(color2);
            style.SetFont(font);
            return style;
        }

        private XSSFCellStyle createProfitLossStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            font.SetColor(color1);
            style.SetFont(font);
            return style;
        }

        private XSSFCellStyle createTitleStyle(XSSFWorkbook workbook, int index)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            XSSFColor color = color0;
            switch (index)
            {
                case 0:
                    color = color4;
                    break;
                case 1:
                    color = color2;
                    break;
                case 2:
                    color = color0;
                    break;
                case 3:
                    color = color5;
                    break;
                case 4:
                    color = color6;
                    break;
            }
            font.SetColor(color);
            style.SetFont(font);
            return style;
        }

        private XSSFCellStyle createProfitStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            font.SetColor(color1);
            style.SetFont(font);
            XSSFDataFormat format = (XSSFDataFormat)workbook.CreateDataFormat();
            style.DataFormat = format.GetFormat("0");
            return style;
        }

        private XSSFCellStyle createHoldErrorStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            style.SetFont(font);
            style.FillForegroundColorColor = color3;
            style.FillPattern = FillPattern.SolidForeground;
            return style;
        }

        private void setColumnWidth(XSSFSheet sheet)
        {
            for (int i = 0; i < 150; i++)
            {
                sheet.SetColumnWidth(i, 12 * 256);
            }
        }

        private void setRowHeight(XSSFRow row)
        {
            row.Height = 270;
        }


        //-----------------------------------------------------------------------
        //设置周期+模型，分钟=>分；小时=>H
        private void setCycleModelCell(ICell cell, string cycle, string agreement, string model)
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
            if (!string.IsNullOrEmpty(agreement))
            {
                if (agreement.Contains("指数")|| agreement.Contains("加权"))
                {
                    agreement = "";
                }
                else if (agreement.Contains("主连"))
                {
                    agreement = "连";
                }
                else
                {
                    agreement = "个";
                }
            }
            cell.SetCellValue(cycle + agreement + model);
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
