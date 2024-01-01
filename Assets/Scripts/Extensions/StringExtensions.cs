using JetBrains.Annotations;

namespace Assets.Scripts.Extensions
{
    public static class StringExtensions
    {
        public static int GetFnvHashCode([CanBeNull] this string str)
        {
            if (str is null)
                return 0;

            unchecked
            {
                //dotnet string hashing is documented as not guaranteed stable between runtimes!
                //Implement our own hash to ensure stability (FNV-1a Hash http://isthe.com/chongo/tech/comp/fnv/#FNV-1a)
                var hash = 2166136261;

                for (var i = 0; i < str.Length; i++)
                {
                    //FNV works on bytes, so split this char into 2 bytes
                    var c = str[i];
                    var b1 = (byte)(c >> 8);
                    var b2 = (byte)c;

                    hash ^= b1;
                    hash *= 16777619;

                    hash ^= b2;
                    hash *= 16777619;
                }

                return (int)hash;
            }
        }
    }
}