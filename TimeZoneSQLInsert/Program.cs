using System;
using System.Collections.ObjectModel;
using System.Text;
using TimeZoneConverter;

namespace TimeZoneSQLInsert
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                ReadOnlyCollection<TimeZoneInfo> tzCollection = TimeZoneInfo.GetSystemTimeZones();
                StringBuilder sb = new StringBuilder();
                sb.Append(Environment.NewLine).Append("SET IDENTITY_INSERT [dbo].[TimeZone] ON");
                int i = 1; // SQL default auto increment value

                try
                {
                    foreach (TimeZoneInfo tz in tzCollection)
                    {
                        sb.Append(Environment.NewLine)
                            .Append("INSERT INTO [TimeZone] ([Id], [WindowsId], [DisplayName], [StandardName], [IanaName], [SupportsDaylightSavingTime], [UtcOffsetTicks]) VALUES (N'")
                            .Append(i++).Append("', N'")
                            .Append(tz.Id).Append("', N'")
                            .Append(tz.DisplayName.Replace("'","''")).Append("', N'")
                            .Append(tz.StandardName).Append("', N'")
                            .Append(TZConvert.WindowsToIana(tz.Id)).Append("', N'")
                            .Append(tz.SupportsDaylightSavingTime).Append("', N'")
                            .Append(tz.BaseUtcOffset.Ticks).Append("');");
                    }
                }
                catch (TimeZoneNotFoundException)
                {
                    Console.WriteLine("COULD NOT CONVERT FROM WIN TO IANA. FOREACH ABORTED.");
                }

                sb.Append(Environment.NewLine).Append("SET IDENTITY_INSERT [dbo].[TimeZone] OFF");
                Console.WriteLine(sb);
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
