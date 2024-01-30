using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    public class StateDtoTests
    {
        [Fact]
        public void ModelToDto()
        {
            Document document = new();

            State state = new State(document, 5, "État X", default, StateKind.Final);

            StateDto stateDto = StateDto.MapFromModel(state);
            Assert.Equal(5, stateDto.Id);
            Assert.Equal("État X", stateDto.Name);
            Assert.Equal(StateKind.Final, stateDto.Kind);
        }

        [Fact]
        public void DtoToModel()
        {
            Document document = new();

            StateDto stateDto = new()
            {
                Id = 5,
                Name = "État X",
                Kind = StateKind.Final,
            };

            State state = stateDto.MapToModel(document);
            Assert.Equal(5, state.Id);
            Assert.Equal("État X", state.Name);
            Assert.Equal(StateKind.Final, state.Kind);
        }
    }
}
