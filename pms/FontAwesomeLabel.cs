using Xamarin.Forms;

namespace pms
{
    public class FontAwesomeLabel : Label
    {
        public static readonly string FontAwesomeName = "FontAwesome";

        //Parameterless constructor for XAML
        public FontAwesomeLabel()
        {
            FontFamily = FontAwesomeName;
        }

        public FontAwesomeLabel(string fontAwesomeLabel = null)
        {
            FontFamily = FontAwesomeName;
            Text = fontAwesomeLabel;
        }
    }

    public static class Icon
    {
        public static readonly string FACamera = "\uf030";
        public static readonly string FAImage = "\uf03e";
        public static readonly string FASync = "\uf2f1";
    }
}
