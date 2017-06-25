namespace Cars
{
    using System;
    using System.Linq;

    internal static class UriExtensions
    {
        public static Uri Sanitize(this Uri uri) => uri.Sanitize(false);

        public static Uri Sanitize(this Uri uri, bool includeTrailingSlash) => uri.Combine(includeTrailingSlash);

        public static Uri Combine(this Uri uri, params string[] path) => uri.Combine(false, path);

        public static Uri Combine(this Uri uri, bool includeTrailingSlash, params string[] path)
        {
            Guard.Against.Null(() => uri);

            var relativeUri = string.Join("/", path.Where(value => !string.IsNullOrEmpty(value)).Select(value => value.Trim('/')));

            var absolutePath = string.IsNullOrEmpty(relativeUri)
                ? uri.AbsolutePath.TrimEnd('/')
                : string.Concat(uri.AbsolutePath.TrimEnd('/'), "/", relativeUri.TrimStart('/'));

            if (includeTrailingSlash)
            {
                absolutePath += "/";
            }

            return new Uri(uri, absolutePath);
        }
    }
}
