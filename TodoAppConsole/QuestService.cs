namespace TodoAppConsole
{
    public class QuestService
    {
        private readonly List<Quest> _quests = new List<Quest>();

        private int GetLastId()
        {
            if (_quests.Count == 0)
            {
                return 0;
            }

            return _quests[_quests.Count - 1].Id;
        }

        public Quest AddQuest(Quest quest)
        {
            var id = GetLastId();
            quest.Id = id + 1;
            _quests.Add(quest);
            return quest;
        }

        public Quest UpdateQuest(Quest quest)
        {
            return quest;
        }

        public bool DeleteQuest(int id)
        {
            var quest = GetQuestById(id);

            if (quest is null)
            {
                return false;
            }

            _quests.Remove(quest);
            return true;
        }

        public Quest? GetQuestById(int id)
        {
            foreach (var quest in _quests)
            {
                if (quest.Id == id)
                {
                    return quest;
                }
            }

            return null;
        }

        public IReadOnlyList<Quest> GetAllQuests()
        {
            return _quests;
        }
    }
}
