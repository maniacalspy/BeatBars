import sys, os
import asyncio
sys.path.insert(0, 'C:/Users/Nolan/source/repos/PywinautoIntegration')
from TTYDGameData import KeyWordBindData, ButtonData
from twitchio.ext  import commands
#https://twitchio.readthedocs.io/en/rewrite/index.html

GameTitle = sys.argv[1]
print(GameTitle, flush = True)
import ctypes
import time

SendInput = ctypes.windll.user32.SendInput

PUL = ctypes.POINTER(ctypes.c_ulong)
class KeyBdInput(ctypes.Structure):
    _fields_ = [("wVk", ctypes.c_ushort),
                ("wScan", ctypes.c_ushort),
                ("dwFlags", ctypes.c_ulong),
                ("time", ctypes.c_ulong),
                ("dwExtraInfo", PUL)]

class HardwareInput(ctypes.Structure):
    _fields_ = [("uMsg", ctypes.c_ulong),
                ("wParamL", ctypes.c_short),
                ("wParamH", ctypes.c_ushort)]

class MouseInput(ctypes.Structure):
    _fields_ = [("dx", ctypes.c_long),
                ("dy", ctypes.c_long),
                ("mouseData", ctypes.c_ulong),
                ("dwFlags", ctypes.c_ulong),
                ("time",ctypes.c_ulong),
                ("dwExtraInfo", PUL)]

class Input_I(ctypes.Union):
    _fields_ = [("ki", KeyBdInput),
                 ("mi", MouseInput),
                 ("hi", HardwareInput)]

class Input(ctypes.Structure):
    _fields_ = [("type", ctypes.c_ulong),
                ("ii", Input_I)]


def PressKey(hexKeyCode):
    extra = ctypes.c_ulong(0)
    ii_ = Input_I()
    ii_.ki = KeyBdInput( 0, hexKeyCode, 0x0008, 0, ctypes.pointer(extra) )
    x = Input( ctypes.c_ulong(1), ii_ )
    ctypes.windll.user32.SendInput(1, ctypes.pointer(x), ctypes.sizeof(x))

def ReleaseKey(hexKeyCode):
    extra = ctypes.c_ulong(0)
    ii_ = Input_I()
    ii_.ki = KeyBdInput( 0, hexKeyCode, 0x0008 | 0x0002, 0, ctypes.pointer(extra) )
    x = Input( ctypes.c_ulong(1), ii_ )
    ctypes.windll.user32.SendInput(1, ctypes.pointer(x), ctypes.sizeof(x))

class TestBot(commands.Bot):

    bIntegrationMode = ctypes.c_bool(False)

    class chat_data:
        data = {}


    def __init__(self):
        super().__init__(irc_token=os.environ['TMI_TOKEN'], client_id=os.environ['CLIENT_ID'], nick=os.environ['BOT_NICK'],
                         prefix=os.environ['BOT_PREFIX'], initial_channels=[os.environ['CHANNEL']])
    

    async def event_ready(self):
        ws = self._ws
        await ws.send_privmsg(os.environ['CHANNEL'], f"/me CHAT INTEGRATION HAS STARTED")

    async def ToggleIntegration(self):
        self.bIntegrationMode = ctypes.c_bool(not bIntegrationMode)

    async def event_message(self, ctx):
        if ctx.author.name.lower() == os.environ['BOT_NICK'].lower():
            if ctx.content == "STOP":
                bot._ws.teardown()
                sys.exit()
                return
            
            chatmessage = ctx.content.strip(' ').lower()
            await self.handle_commands(ctx)

            if self.bIntegrationMode: HandleIntegration(self, chatmessagee)


    async def HandleIntegration(self, message):
        if(GameTitle == "BeatBars"):
            if message == 'up' or message == 'north':
                chat_data.data[ctx.author.name] = 'north'
            elif message == 'down' or message == 'south':
                chat_data.data[ctx.author.name] = 'south'
            elif message == 'right' or message == 'east':
                chat_data.data[ctx.author.name] = 'east'
            elif message == 'left' or message == 'west':
                chat_data.data[ctx.author.name] = 'west'

        elif(GameTitle == "TTYD"):
            if message in KeyWordBindData.keywords:
                InputData = KeyWordBindData.keywords[message]
                await SendKeyInputDuration(Key = InputData.button, duration = InputData.duration)

        if len(chat_data.data) > 0:
            await Print_Chat_Data()
            return

    @commands.command(name='test')
    async def TestCommand(self, ctx):
        await ctx.send(f'Hello {ctx.author.name}')
    
    @commands.command(name='dontdothis')
    async def TimeoutCommand(self, ctx):
        await ctx.timeout(user = ctx.author, duration = 6000, reason = 'Did the bad command')
        await ctx.send_me(f'{ctx.author.name} has tempted the fates, and now must learn their lesson')



    async def Print_Chat_Data():
        print(chat_data.data, flush=True)
        chat_data.data.clear()


async def SendKeyInput(Key):
    try:
        PressKey(Key)
        await asyncio.sleep(.01)
        ReleaseKey(Key)
    except e:
        print(e)

async def SendKeyInputDuration(Key, duration):
    try:
        PressKey(Key)
        await asyncio.sleep(duration)
        ReleaseKey(Key)
    except e:
        print(e)


if __name__ == "__main__":
    bot = TestBot()
    bot.run()