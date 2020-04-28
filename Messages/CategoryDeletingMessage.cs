namespace Messages
{
    public class CategoryDeletingMessage
    {
        public int Id;
        public int NewId;
        
        public CategoryDeletingMessage(int id, int newId)
        {
            Id = id;
            NewId = newId;
        }
    }
}