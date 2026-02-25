namespace Talaryon.Toolbox.Extensions;

public static class CharExtensions
{
    private static readonly Dictionary<char, string> CharMappings = new()
    {
        { 'à', "a" }, { 'å', "a" }, { 'á', "a" }, { 'â', "a" }, { 'ä', "a" },
        { 'ã', "a" }, { 'ą', "a" },
        { 'è', "e" }, { 'é', "e" }, { 'ê', "e" }, { 'ë', "e" }, { 'ę', "e" },
        { 'ì', "i" }, { 'í', "i" }, { 'î', "i" }, { 'ï', "i" }, { 'ı', "i" },
        { 'ò', "o" }, { 'ó', "o" }, { 'ô', "o" }, { 'õ', "o" }, { 'ö', "o" }, { 'ø', "o" }, { 'ő', "o" }, { 'ð', "o" },
        { 'ù', "u" }, { 'ú', "u" }, { 'û', "u" }, { 'ü', "u" }, { 'ŭ', "u" }, { 'ů', "u" },
        { 'ç', "c" }, { 'ć', "c" }, { 'č', "c" }, { 'ĉ', "c" },
        { 'ż', "z" }, { 'ź', "z" }, { 'ž', "z" },
        { 'ś', "s" }, { 'ş', "s" }, { 'š', "s" }, { 'ŝ', "s" },
        { 'ñ', "n" }, { 'ń', "n" },
        { 'ý', "y" }, { 'ÿ', "y" },
        { 'ğ', "g" }, { 'ĝ', "g" },
        { 'ř', "r" },
        { 'ł', "l" },
        { 'đ', "d" },
        { 'ß', "ss" },
        { 'þ', "th" },
        { 'ĥ', "h" },
        { 'ĵ', "j" }
    };
    
    extension(char c)
    {
        public string RemapInternationalCharToAscii()
        {
            return CharMappings.TryGetValue(c, out var mappedValue) ? mappedValue : string.Empty;
        }
    }
}