using RpgBot.Type;

namespace RpgBot.DTO
{
    public class MessageDto<T>
    {
        public T Chat { get; set; }
        public MessageType MessageType { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public string MessageId { get; set; }
        public string Text { get; set; }
        public string GroupId { get; set; }
    }
}