using System;
using Vulcan.AspNetMvc.Interfaces;

namespace Vulcan.AspNetMvc.Sample.Core.Impl
{
    public class DefaultResource : IResource
    {
        public string Code { get;set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}