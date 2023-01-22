using TodoApp.Core.DTO;
using TodoApp.Core.Entities;

namespace TodoApp.Core.Mappings
{
    internal static class Extensions
    {
        public static QuestDto AsDto(this Quest quest)
        {
            return new QuestDto
            {
                Id = quest.Id,
                Title = quest.Title,
                Description = quest.Description,
                Status = quest.Status.ToString(),
                Created = quest.Created,
                Modified = quest.Modified,
            };
        }

        public static Quest AsEntity(this QuestDto questDto)
        {
            return new Quest(questDto.Id, questDto.Title, questDto.Description, questDto.Status, questDto.Created, questDto.Modified);
        }

        public static MenuDto AsDto(this Menu menu)
        {
            return new MenuDto
            {
                Id = menu.Id,
                Name = menu.Name
            };
        }

        public static Menu AsEntity(this MenuDto menuDto)
        {
            return new Menu
            {
                Id = menuDto.Id,
                Name = menuDto.Name
            };
        }
    }
}
