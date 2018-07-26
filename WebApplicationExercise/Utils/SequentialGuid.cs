using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace WebApplicationExercise.Utils
{
    /// <summary>
    ///     Sequential GUID utils
    /// </summary>
    public static class SequentialGuid
    {
        /// <summary>
        ///     Next sequential guid
        /// </summary>
        public static Guid Next => CreateGuid();

        /// <summary>
        ///     Create new sequential guid
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Guid CreateGuid()
        {
            var result = NativeMethods.UuidCreateSequential(out var guid);
            if (result != 0) throw new Exception("Error generating sequential GUID");

            var bytes = guid.ToByteArray();
            var indexes = new[] {3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15};
            return new Guid(indexes.Select(i => bytes[i]).ToArray());
        }
    }

    internal static class NativeMethods
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        public static extern int UuidCreateSequential(out Guid guid);
    }
}