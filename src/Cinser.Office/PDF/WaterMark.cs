using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Configuration;
using System.Drawing;

namespace Cinser.Office.PDF
{
    public class WaterMark
    {
        /// <summary>
        /// pdf文字水印设置类
        /// </summary>
        public class pdfWaterMarkSettings
        {
            string _fontColor;
            int _fontSize;
            int _horizontalSpace;
            int _verticalSpace;
            float _angle;
            bool _fillScreen;
            float _opacityValue;
            List<int> _waterPagesIndex;

            public string fontColor
            {
                get { return _fontColor; }
                set { _fontColor = value; }
            }
            public int fontSize
            {
                get { return _fontSize; }
                set { _fontSize = value; }
            }
            public int horizontalSpace
            {
                get { return _horizontalSpace; }
                set { _horizontalSpace = value; }
            }
            public int verticalSpace
            {
                get { return _verticalSpace; }
                set { _verticalSpace = value; }
            }
            public float angle
            {
                get { return _angle; }
                set { _angle = value; }
            }
            public bool fillScreen
            {
                get { return _fillScreen; }
                set { _fillScreen = value; }
            }
            public float opacityValue
            {
                get { return _opacityValue; }
                set { _opacityValue = value; }
            }
            public List<int> waterPagesIndex
            {
                get { return _waterPagesIndex; }
                set { _waterPagesIndex = value; }
            }
        }

        /// <summary>
        /// 在PDF上加上文字水印
        /// </summary>
        /// <param name="inputFilePath">PDF输入路径</param>
        /// <param name="waterMarkString">水印文字</param>
        /// <param name="s">水印设置</param>
        public static bool AddPdfTextWaterMark(string inputFilePath, string waterMarkString, pdfWaterMarkSettings s)
        {
            bool bReturn = false;
            FileInfo pdfFile = new System.IO.FileInfo(inputFilePath);
            if (pdfFile.Exists)
            {
                string outputFilePath = pdfFile.Directory + "\\" + Guid.NewGuid().ToString() + ".pdf";
                if (AddPdfTextWaterMark(pdfFile.FullName, outputFilePath, waterMarkString, s))
                {
                    FileInfo outputFile = new System.IO.FileInfo(outputFilePath);
                    if (outputFile.Exists)
                    {
                        outputFile.CopyTo(inputFilePath, true);
                        outputFile.Delete();
                        bReturn = true;
                    }
                }
            }
            else
            {
                throw new Exception(pdfFile.FullName + ",文件路径不存在！");
            }
            return bReturn;
        }

        /// <summary>
        /// 在PDF上加上文字水印
        /// </summary>
        /// <param name="inputFilePath">PDF输入路径</param>
        /// <param name="outputFilePath">打上水印后的PDF输出路径</param>
        /// <param name="waterMarkString">水印文字</param>
        /// <param name="s">水印设置</param>
        public static bool AddPdfTextWaterMark(string inputFilePath, string outputFilePath, string waterMarkString, pdfWaterMarkSettings s)
        {
            return SetPDFWaterMark(inputFilePath, outputFilePath, waterMarkString, s);
        }

        /// <summary>
        /// 在PDF上加上文字水印
        /// </summary>
        /// <param name="inputPdfStream">输入文件流</param>
        /// <param name="waterMarkString">水印文字</param>
        /// <param name="s">水印设置</param>
        /// <returns>返回打上水印后的PDF的文件流</returns>
        public static MemoryStream AddPdfTextWaterMark(MemoryStream inputPdfStream, string waterMarkString, pdfWaterMarkSettings s)
        {
            return SetPDFWaterMark(inputPdfStream, waterMarkString, s);
        }

        private static bool SetPDFWaterMark(string inputFilePath, string outputFilePath, string waterMarkString, pdfWaterMarkSettings s)
        {
            bool bReturn = false;
            //验证阶段
            if (File.Exists(inputFilePath) == false)
            {
                throw new Exception(inputFilePath + ",文件不存在！");
            }
            if (inputFilePath.ToUpper().EndsWith(".PDF") == false)
            {
                throw new Exception(inputFilePath + ",文件不是要求的.pdf文件！");
            }
            if (inputFilePath == outputFilePath)
            {
                throw new Exception("输入路径和输出路径不能相同！");
            }

            MemoryStream ms = Utils.ReadFileMemoryStream(inputFilePath);
            if (ms != null)
            {
                MemoryStream newPdfMemoryStream = new System.IO.MemoryStream();
                newPdfMemoryStream = SetPDFWaterMark(ms, waterMarkString, s);
                if (newPdfMemoryStream != null)
                {
                    FileStream fs = null;
                    try
                    {

                        fs = new System.IO.FileStream(outputFilePath, FileMode.OpenOrCreate);
                        byte[] bt = newPdfMemoryStream.GetBuffer();
                        fs.Write(bt, 0, bt.Length);
                    }
                    catch (Exception ex)
                    {
                        ex.Message.Trim();
                        return false;
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }
                }
            }
            return bReturn;
        }

        private static MemoryStream SetPDFWaterMark(MemoryStream inputPdfStream, string waterMarkString, pdfWaterMarkSettings s)
        {
            MemoryStream newPdfMemoryStream = new System.IO.MemoryStream();
            //验证阶段
            if (string.IsNullOrEmpty(waterMarkString))
            {
                throw new Exception("水印值waterMarkString不能为null或者空！");
            }

            //添加水印
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                //pdfReader = new PdfReader(inputFilePath);
                pdfReader = new PdfReader(inputPdfStream);
                int numberOfPages = pdfReader.NumberOfPages;
                if (s.waterPagesIndex == null)
                {
                    s.waterPagesIndex = new List<int>();
                    for (int i = 0; i < numberOfPages; i++)
                    {
                        s.waterPagesIndex.Add(i + 1);
                    }
                }
                else
                {
                    foreach (var item in s.waterPagesIndex)
                    {
                        if (item >= numberOfPages)
                            throw new Exception("指定要加水印页的页码不能大于，pdf文件总页数！");
                    }
                }

                pdfStamper = new PdfStamper(pdfReader, newPdfMemoryStream);
                PdfContentByte waterMarkContent;

                PdfGState gstate = new PdfGState();
                gstate.FillOpacity = s.opacityValue;
                gstate.StrokeOpacity = s.opacityValue;

                //给指定页加水印 
                foreach (var i in s.waterPagesIndex)
                {
                    iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(i);
                    float width = psize.Width;
                    float height = psize.Height;

                    waterMarkContent = pdfStamper.GetOverContent(i);
                    waterMarkContent.SetGState(gstate);
                    waterMarkContent.BeginText();
                    waterMarkContent.SetColorFill(new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml(s.fontColor)));
                    iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.COURIER,
                        iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                    waterMarkContent.SetFontAndSize(bf, s.fontSize);

                    if (s.fillScreen)
                    {
                        // 设置水印文字字体倾斜 开始                    
                        for (int h = 15, start = 0; h < height; h += s.verticalSpace, start -= 15)
                        {
                            for (int w = 15; w < width + s.horizontalSpace; w += s.horizontalSpace)
                            {
                                waterMarkContent.SetTextMatrix(w + start, h);
                                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, waterMarkString, w + start, h, s.angle);
                            }
                        }
                    }
                    else
                    {
                        waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, waterMarkString, width / 2f, s.verticalSpace, 0);
                        waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, waterMarkString, width / 2f, height - s.verticalSpace, 0);
                    }

                    // 水印文字设置结束  
                    waterMarkContent.EndText();
                }
                return newPdfMemoryStream;
            }
            catch (Exception ex)
            {
                ex.Message.Trim();
                return null;
            }
            finally
            {
                if (pdfStamper != null)
                    pdfStamper.Close();
                if (pdfReader != null)
                    pdfReader.Close();
            }
        }

    }
}
