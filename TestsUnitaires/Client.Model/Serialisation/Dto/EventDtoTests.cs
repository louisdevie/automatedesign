using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    public class EventDtoTests
    {
        [Fact]
        public void ModelToDto()
        {
            EnumEvent enumEvent = new(7, "Truc");
            
            EnumEventDto enumEventDto = EnumEventDto.MapFromModel(enumEvent);
            Assert.Equal(7, enumEventDto.Id);
            Assert.Equal("Truc", enumEventDto.Name);
        }

        [Fact]
        public void ModelToReferenceDto()
        {
            EnumEvent enumEvent = new(7, "Truc");

            EventReferenceDto enumEventDto = EventReferenceDto.MapFromModel(enumEvent);
            Assert.Equal(EventReferenceDto.EventType.EnumEvent, enumEventDto.Type);
            Assert.Equal(7, enumEventDto.Id);

            DefaultEvent defaultEvent = new();

            EventReferenceDto defaultEventDto = EventReferenceDto.MapFromModel(defaultEvent);
            Assert.Equal(EventReferenceDto.EventType.DefaultEvent, defaultEventDto.Type);
            Assert.Null(defaultEventDto.Id);
        }

        [Fact]
        public void DtoToModel()
        {
            EnumEventDto enumEventDto = new()
            {
                Id = 7,
                Name = "Truc",
            };

            EnumEvent enumEvent = enumEventDto.MapToModel();
            Assert.Equal(7, enumEvent.Id);
            Assert.Equal("Truc", enumEvent.Name);
        }

        [Fact]
        public void ReferenceDtoToModel()
        {
            Document document = new();
            document.AddEvents(new EnumEvent[1] { new(7, "Truc") });

            EventReferenceDto enumEventDto = new()
            {
                Type = EventReferenceDto.EventType.EnumEvent,
                Id = 7,
            };

            Event enumEvent = enumEventDto.MapToModel(document);
            Assert.IsType<EnumEvent>(enumEvent);
            Assert.Equal(7, (enumEvent as EnumEvent)?.Id);
            Assert.Equal("Truc", (enumEvent as EnumEvent)?.Name);

            EventReferenceDto defaultEventDto = new()
            {
                Type = EventReferenceDto.EventType.DefaultEvent,
            };

            Event defaultEvent = defaultEventDto.MapToModel(document);
            Assert.IsType<DefaultEvent>(defaultEvent);
        }
    }
}
