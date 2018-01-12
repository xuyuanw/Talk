using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using Xunit;

namespace Talk.NPOI.Tests
{
    public class NPOIHelper_Test
    {
        private string basePath = "";
        public NPOIHelper_Test()
        {
            DirectoryInfo dir = new DirectoryInfo(AppContext.BaseDirectory);
            basePath = dir.Parent.Parent.Parent.FullName + @"\Data\";//��Ŀ¼         
        }

        [Fact]
        public void xlsx��ȡxlsд�����()
        {
            var table = NPOIHelper.GetDataTables(basePath + "����ģ��1.xlsx");

            NPOIHelper.DataTableToExcel(table[0], basePath + @"Temp\����ģ��1_Test.xls");
        }

        [Fact]
        public void xls��ȡxlsxд�����()
        {
            var table = NPOIHelper.GetDataTables(basePath + "����ģ��2.xls");

            NPOIHelper.DataTableToExcel(table[0], basePath + @"Temp\����ģ��2_Test.xlsx");
        }

        [Fact]
        public void Excel����DataTable��ת����ʵ��()
        {
            //��ȡ��DataTable
            var tableList = NPOIHelper.GetDataTables(basePath + "����ģ��2.xls");
            //DataTableת��ʵ��
            var listEntity = tableList.GetEntityList<AirEntity>((row, air) =>
            {
                air.FromTerminal = row["*���˿ո�"]?.ToString();
                air.ToTerminal = row["*Ŀ�Ŀո�"]?.ToString(); ;
                air.Carrier = row["*���չ�˾"]?.ToString(); ;
                //air.ETD = row["*��������"]?.ToDateTimeOrNull();
            });
        }

        [Fact]
        public void ����ʵ��ת����DataTable()
        {
            var tableList = NPOIHelper.GetDataTables(basePath + "����ģ��2.xls");
            var listEntity = tableList.GetEntityList<AirEntity>((row, air) =>
            {
                air.FromTerminal = row["*���˿ո�"].ToString();
                air.ToTerminal = row["*Ŀ�Ŀո�"].ToString();
                air.Carrier = row["*���չ�˾"].ToString();
            });

            Dictionary<string, string> head = new Dictionary<string, string>()
            {
                { "FromTerminal","*���˿ո�"},
                { "ToTerminal","*Ŀ�Ŀո�"},
                { "Carrier","*���չ�˾"},
            };
            //ʵ��ת��DataTable
            var table = listEntity.ToDataTable(head, (row, air) =>
            {
                row[head["FromTerminal"]] = air.FromTerminal;
                row[head["ToTerminal"]] = air.ToTerminal;
                row[head["Carrier"]] = air.Carrier;
            });
        }

        [Fact]
        public void ���Shett��ȡ�ɶ��Table�ٷֱ�д��Excel()
        {
            var tableList = NPOIHelper.GetDataTables(basePath + "����ȫ����_Test.xlsx");
            var i = 0;
            foreach (var table in tableList)
            {
                if (table != null)
                    NPOIHelper.DataTableToExcel(table, basePath + @"Temp\����ȫ����_Test" + i++ + ".xlsx");
            }
        }

        [Fact]
        public void ��ȡ�Զ����ͷ�����ݿ�ʼ��()
        {
            var list = new List<Point>() {
                 new Point(1,0),
                 new Point(2,1),
                 new Point(2,2),
                 new Point(1,3),
                 new Point(1,4),
                 new Point(1,5),
                 new Point(1,6),
            };
            var table = NPOIHelper.GetDataTable(basePath + "����ȫ����_Test.xlsx", list, 3, 1);

            NPOIHelper.DataTableToExcel(table, basePath + @"Temp\����ȫ����_Test.xlsx");
        }

        [Fact]
        public void �޸�ԴExcel�ļ�()
        {
            var filePath = basePath + @"Temp\����ȫ����_Copy.xlsx";
            var dic = new Dictionary<int, string>() {
                { 4,"��עaaaa"},
                { 5,"��עaʿ���aaa"},
                { 6,"werewolf"},
                { 7,"ʿ�������ҷ�"},
                { 8,"ί�Ξ�"},
                { 10,"˹�ٷ�˹�ٷҵ�����˹�ٷ������������vqwreqwerewqvbewreyvrtybw"},
            };

            NPOIHelper.SaveExcel(basePath + "����ȫ����_Test.xlsx", filePath, 1, 14, dic);
        }

        public class AirEntity
        {
            public string FromTerminal { get; set; }
            public string ToTerminal { get; set; }
            public string Carrier { get; set; }
            public DateTime? ETD { get; set; }
        }
    }
}