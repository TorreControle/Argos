/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using Plotly.NET;
using Plotly.NET.CSharp;
using Plotly.NET.ImageExport;
using System.Data;
using System.Drawing;
using Color = Plotly.NET.Color;
using Font = Plotly.NET.Font;

namespace ArgosOnDemand.Skill
{
    // Classe para criação de graficos em C#

    public class Graphics
    {
        // Campo genérico para a criação do gráfico.

        public static GenericChart.GenericChart chart;


        // Gráfico de barras


        public static void Bar(DataTable dataBase, string paramX, string paramY, string titleX, string titleY, string title, string nameGraphic)
        {
            // Obtém as colunas definidas nos parametros X e Y.

            DataColumn columnQuantitative = dataBase.Columns[paramX];
            DataColumn columnQualitative = dataBase.Columns[paramY];

            // Criação dos eixos x, y no formato de lista com os valores obtidos.

            // X 

            List<decimal> x = new();
            for (int i = 0; i <= columnQuantitative.Table.Rows.Count - 1; i++)
            {
                x.Add(decimal.Parse(columnQuantitative.Table.Rows[i][columnQuantitative.ColumnName].ToString()));
            }


            // Y 

            List<string> y = new();
            for (int i = 0; i <= columnQualitative.Table.Rows.Count - 1; i++)
            {
                y.Add(columnQualitative.Table.Rows[i][columnQualitative.ColumnName].ToString());
            }


            // Criação do gráfico de barras horizontais.

            chart = Plotly.NET.CSharp.Chart.Bar<decimal, string, decimal>(values: x, Keys: y, MultiText: x, Marker: Plotly.NET.TraceObjects.Marker.init(Color: Color.fromRGB(0, 78, 140)))
                    .WithTitle(@$"{title}", TitleFont: Font.init(StyleParam.FontFamily.Consolas))
                    .WithXAxisStyle<double, double, string>(Title: Title.init(titleX), TitleFont: Font.init(StyleParam.FontFamily.Consolas), Color: Color.fromRGB(0, 0, 0), CategoryOrder: StyleParam.CategoryOrder.TotalDescending)
                    .WithYAxisStyle<double, double, string>(Title: Title.init(titleY), TitleFont: Font.init(StyleParam.FontFamily.Consolas), Color: Color.fromRGB(0, 0, 0));


            // Salva como JPG

            chart.SaveJPG(@$"{Utilities.Directory.Folders.Charts}\{nameGraphic}");
           

        }
        public static void Bar(List<decimal> paramX, List<string> paramY, string title, string titleX, string titleY, string nameGraphic)
        {
            // Criação do gráfico de barras horizontais.

            chart = Plotly.NET.CSharp.Chart.Bar<decimal, string, decimal>(values: paramX, Keys: paramY, MultiText: paramX, Marker: Plotly.NET.TraceObjects.Marker.init(Color: Color.fromRGB(0, 78, 140)))
            .WithTitle(@$"{title}", TitleFont: Font.init(StyleParam.FontFamily.Consolas))
                    .WithXAxisStyle<double, double, string>(Title: Title.init(titleX), TitleFont: Font.init(StyleParam.FontFamily.Consolas), Color: Color.fromRGB(0, 0, 0), CategoryOrder: StyleParam.CategoryOrder.TotalDescending)
                    .WithYAxisStyle<double, double, string>(Title: Title.init(titleY), TitleFont: Font.init(StyleParam.FontFamily.Consolas), Color: Color.fromRGB(0, 0, 0));

            // Salva como JPG

            chart.SaveJPGAsync(@$"{Utilities.Directory.Folders.Charts}\{nameGraphic}");

        }

    }

}


