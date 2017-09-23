using System;
using System.Collections.Generic;
using System.Text;

namespace Selly.NMS.Web.Infrastructure
{
    public class GoogleChartHelpers
    {
        public static string ToGoogleChartString(IDictionary<string, int> data, string columnA, string columnB)
        {
            var dataString = new StringBuilder();

            dataString.Append($"[['{columnA}','{columnB}']");

            foreach (var pair in data)
            {
                dataString.Append($", ['{pair.Key}', {pair.Value}]");
            }

            dataString.Append("]");

            return dataString.ToString();
        }

        public static string ToGoogleChartString(int[][] data, string columnA, string columnB)
        {
            var dataString = new StringBuilder();

            dataString.Append($"[['{columnA}','{columnB}']");

            foreach (var pair in data)
            {
                dataString.Append($", ['{pair[0]}', {pair[1]}]");
            }

            dataString.Append("]");

            return dataString.ToString();
        }

        public static int[][] To24HourArray(int[][] data)
        {
            int cursor = 0;
            int[][] last24Hours = new int[24][];

            for (int i = 0; i < 24; i++)
            {
                var hour = DateTimeOffset.Now.Subtract(TimeSpan.FromHours( 24 - (i + 1))).Hour;

                last24Hours[i] = new int[2];
                last24Hours[i][0] = hour;

                if (cursor < data.Length)
                {
                    if (data[cursor][0] == hour)
                    {
                        last24Hours[i][1] = data[cursor][1];
                        cursor++;
                    }
                    else
                    {
                        last24Hours[i][1] = 0;
                    }
                }
                else
                {
                    last24Hours[i][1] = 0;
                }
            }

            return last24Hours;
        }
    }
}
