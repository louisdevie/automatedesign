using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    public class TransitionDtoTests
    {
        [Fact]
        public void ModelToDto()
        {
            Document document = new();
            State stateX = new(document, 11, "État X", default);
            State stateY = new(document, 22, "État Y", default);

            Transition transition = new(6, stateX, stateY, new DefaultEvent());

            TransitionDto transitionDto = TransitionDto.MapFromModel(transition);
            Assert.Equal(6, transitionDto.Id);
            Assert.Equal(11, transitionDto.Start);
            Assert.Equal(22, transitionDto.End);
            Assert.Equal(EventReferenceDto.EventType.DefaultEvent, transitionDto.TriggeredBy.Type);
            Assert.Null(transitionDto.TriggeredBy.Id);
        }

        [Fact]
        public void DtoToModel()
        {
            Document document = new();
            State stateX = document.CreateState("État X");
            State stateY = document.CreateState("État Y");
            EnumEvent eventA = document.CreateEnumEvent("A");

            TransitionDto transitionDto = new()
            {
                Id = 6,
                Start = stateX.Id,
                End = stateY.Id,
                TriggeredBy = new() { Type = EventReferenceDto.EventType.EnumEvent, Id = eventA.Id },
            };

            Transition transition = transitionDto.MapToModel(document);
            Assert.Equal(6, transition.Id);
            Assert.Same(stateX, transition.Start);
            Assert.Same(stateY, transition.End);
            Assert.Same(eventA, transition.TriggeredBy);
        }
    }
}
