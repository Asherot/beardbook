using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeardBook.Attributes
{
    public class AllowedContentTypesAttribute : ValidationAttribute
    {
        private readonly string[] _allowedContentTypes;

        public AllowedContentTypesAttribute(string[] allowedContentTypes)
        {
            _allowedContentTypes = allowedContentTypes;
        }
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var file = value as HttpPostedFileBase;
            return file != null
                && _allowedContentTypes.Any(c => file.ContentType == c);
        }
        public override string FormatErrorMessage(string name)
        {
            var fileTypes = _allowedContentTypes.Select(c => c.Substring(c.LastIndexOf("/", StringComparison.Ordinal) + 1));
            return $"Allowed file types: {string.Join(" ", fileTypes)}";
        }
    }
}