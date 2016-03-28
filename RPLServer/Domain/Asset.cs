using System;

namespace Domain
{
    public class Asset
    {
        public AssetType AssetType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime InsertDate { get; set; }
    }
}