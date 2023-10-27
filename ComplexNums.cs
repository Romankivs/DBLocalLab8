using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace WpfApp2
{
    public class ComplexIntegerConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string strValue)
            {
                string[] parts = strValue.Split(' ', ' ', 'i');
                if (parts.Length >= 2 && int.TryParse(parts[0], out int realPart) && int.TryParse(parts[2], out int imagPart) && (parts[1] == "+" || parts[1] == "-"))
                {
                    var inter = new ComplexInteger();
                    inter.RealPart = realPart;
                    inter.ImaginaryPart = imagPart;
                    if (parts[1] == "-")
                        inter.ImaginaryPart *= -1;
                    return inter;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is ComplexInteger complex)
            {
                string res = "";
                res += complex.RealPart;
                res += complex.ImaginaryPart < 0 ? " - " : " + ";
                res += Math.Abs(complex.ImaginaryPart) + "i";
                return res;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(ComplexIntegerConverter))]
    public class ComplexInteger : IXmlSerializable
    {
        public int RealPart { get; set; }
        public int ImaginaryPart { get; set; }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(RealPart.ToString() + " " + ImaginaryPart.ToString());
        }

        public void ReadXml(XmlReader reader)
        {
            var str = reader.ReadElementContentAsString();
            string[] parts = str.Split(' ');
            int.TryParse(parts[0], out int realPart);
            int.TryParse(parts[1], out int imgPart);
            RealPart = realPart;
            ImaginaryPart = imgPart;
        }

        public XmlSchema? GetSchema()
        {
            return (null);
        }
    }

    public class ComplexRealConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string strValue)
            {
                string[] parts = strValue.Split(' ', ' ', 'i');
                if (parts.Length >= 2 && double.TryParse(parts[0], out double realPart) && double.TryParse(parts[2], out double imagPart) && (parts[1] == "+" || parts[1] == "-"))
                {
                    var inter = new ComplexReal();
                    inter.RealPart = realPart;
                    inter.ImaginaryPart = imagPart;
                    if (parts[1] == "-")
                        inter.ImaginaryPart *= -1;
                    return inter;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is ComplexReal complex)
            {
                string res = "";
                res += complex.RealPart;
                res += complex.ImaginaryPart < 0 ? " - " : " + ";
                res += Math.Abs(complex.ImaginaryPart) + "i";
                return res;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(ComplexRealConverter))]
    public class ComplexReal : IXmlSerializable
    {
        public double RealPart { get; set; }
        public double ImaginaryPart { get; set; }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(RealPart.ToString() + " " + ImaginaryPart.ToString());
        }

        public void ReadXml(XmlReader reader)
        {
            var str = reader.ReadElementContentAsString();
            string[] parts = str.Split(' ');
            double.TryParse(parts[0], out double realPart);
            double.TryParse(parts[1], out double imgPart);
            RealPart = realPart;
            ImaginaryPart = imgPart;
        }

        public XmlSchema? GetSchema()
        {
            return (null);
        }
    }
}
