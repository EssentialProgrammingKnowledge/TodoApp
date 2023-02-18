using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.DTO;
using TodoApp.Core.Services;

namespace TodoApp.Api.Controllers
{
    public class QuestsController : BaseController
    {
        private readonly IQuestService _questService;

        public QuestsController(IQuestService questService)
        {
            _questService = questService;
        }

        [HttpGet]
        public async Task<IEnumerable<QuestDto>> GetAll()
        {
            return await _questService.GetAllQuests();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<QuestDto>> Get(int id)
        {
            return OkOrNotFound(await _questService.GetQuestById(id));
        }

        [HttpPost]
        public async Task<ActionResult> Add(QuestDto questDto)
        {
            var dtoAdded = await _questService.AddQuest(questDto);
            return CreatedAtAction(nameof(Get), new { dtoAdded.Id }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, QuestDto questDto)
        {
            questDto.Id = id;
            await _questService.UpdateQuest(questDto);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> ChangeQuestStatus(int id, ChangeQuestStatus changeQuestStatus)
        {
            await _questService.ChangeQuestStatus(id, changeQuestStatus.Status);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _questService.DeleteQuest(id);
            return NoContent();
        }
    }
}
