## RPG bot for Telegram and Discord chats/channels.

This Project is made to practice .NET, NUnit with Moq and using it for fun with friends in our telegram group.

### Installation
- `$ touch RpgBot/Data/Database/bot.db`
- `$ cp RpgBot/appsettings.json.dist RpgBot/appsettings.json` and fill with necessary data

### Testing
- `$ dotnet test`

### Run
- `$ dotnet run`

### Todo list:
- [x] Telegram adapter
- [ ] Discord adapter
- [x] Drop groupId and services logic with it   
- [ ] Preload users in memory to decrease context calls
- [ ] Preload command aliases in memory to decrease context calls 
- [x] Command name alias
- [ ] Nicknames (alias for username)  
- [ ] Phase/combat system (duels, raids etc.)
- [ ] Shop 
- [ ] Inventory
- [ ] Items (potions, weapons and armors)
- [ ] Classes?
- [ ] Skills