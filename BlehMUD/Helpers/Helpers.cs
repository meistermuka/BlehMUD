using BlehMUD.Constants;

namespace BlehMUD.Helpers
{
    public static class TextHelpers
    {
        public static string ColourizeText(string text)
        {
            text = text.Replace("{black}", AsciiColourConstants.Black);
            text = text.Replace("{red}", AsciiColourConstants.Red);
            text = text.Replace("{green}", AsciiColourConstants.Green);
            text = text.Replace("{yellow}", AsciiColourConstants.Yellow);
            text = text.Replace("{blue}", AsciiColourConstants.Blue);
            text = text.Replace("{magenta}", AsciiColourConstants.Magenta);
            text = text.Replace("{cyan}", AsciiColourConstants.Cyan);
            text = text.Replace("{white}", AsciiColourConstants.White);
            text += AsciiColourConstants.Reset;

            return text;
        }
    }
}
