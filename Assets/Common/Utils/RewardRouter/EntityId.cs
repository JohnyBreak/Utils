namespace Services.RewardRouter
{
    public struct EntityId
    {
        public EntityTypeCode TypeCode;
        public int Id;

        public EntityId(EntityTypeCode typeCode, int id)
        {
            TypeCode = typeCode;
            Id = id;
        }

        public override string ToString()
        {
            return $"TypeCode: '{TypeCode.ToString()}', Id: {Id.ToString()}";
        }
    }
}