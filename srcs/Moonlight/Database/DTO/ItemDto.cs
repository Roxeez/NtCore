using Moonlight.Core.Enums;
using Moonlight.Database.DAL;

namespace Moonlight.Database.Dto
{
    internal class ItemDto : IDto<int>
    {
        public string NameKey { get; set; }
        public int Type { get; set; }
        public int SubType { get; set; }
        public BagType BagType { get; set; }
        public string Data { get; set; }
        public int Id { get; set; }
    }
}