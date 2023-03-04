using MAUIToDoList2023.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIToDoList2023.Converters
{
    public class ImportanceToImageConverter : IValueConverter
    { 
        public string Low { get; set; }
        public string Medium { get; set; }
        public string High { get; set; }
        public string Default { get; set; }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Importance importance)
            {
                switch (importance) 
                {
                    case Importance.Low: return Low;
                    case Importance.Medium: return Medium;
                    case Importance.High: return High;
                }
            }

            return Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
