namespace Datiss.Budget.Services.Models
{
    public class CreateConstantDTO
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string ConstantKey { get; set; }

        public int DisplayOrder { get; set; }

        public bool Enabled { get; set; }
    }

    public class UpdateConstantDTO : CreateConstantDTO
    {
        public int Id { get; set; }
    }

    public class ConstantDTO
    {

    }
}
