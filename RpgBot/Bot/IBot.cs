using System.Threading.Tasks;
using RpgBot.DTO;
using RpgBot.Entity;
using RpgBot.Type;

namespace RpgBot.Bot
{
    public interface IBot<T, TK>
    {
        public void Listen();
        public Task<T> SendMessageAsync(TK chatId, string message, string messageId = null);
        public User Advance(User user, TK chat, MessageType type);
        public MessageDto<TK> ParseMessage(T message);
    }
}