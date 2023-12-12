using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    public class DocumentHeaderDtoTests
    {
        [Fact]
        public void ModelToDto()
        {
            DateTime time = new(2023, 6, 21, 17, 15, 43);
            DocumentHeader header = new(1, "Unholy Meat Obelisk™", time);

            DocumentHeaderDto dto = DocumentHeaderDto.MapFromModel(header);
            Assert.Equal("Unholy Meat Obelisk™", dto.Name);
            Assert.Equal(time, dto.LastModificationDate);
        }

        [Fact]
        public void DtoToModel()
        {
            DateTime time = new(2023, 6, 21, 17, 15, 43);
            DocumentHeaderDto dto = new()
            {
                Name = "Unholy Meat Obelisk™",
                LastModificationDate = time,
            };

            DocumentHeader header = dto.MapToModel();
            Assert.Equal("Unholy Meat Obelisk™", header.Name);
            Assert.Equal(time, header.LastModificationdate);
        }
    }
}
