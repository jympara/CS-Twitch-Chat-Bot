using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;

namespace TwitchChatBot
{
	public class Forms : Form
	{
		static Queue<string> SendMessageQueue;
		public static TcpClient tcpClient;
		public static StreamReader reader;
		public static StreamWriter writer;
		public static DateTime LastSentMessage;
		public static string masterName;
		public static string loginUsername;
		private static string loginPassword;
		public System.Windows.Forms.Timer Ftimer1;
		private IContainer components;
		public Button Fbutton1;
		public Button Fbutton2;
		public Button Fbutton3;
		public Button Fbutton4;
		public Button Fbutton5;
		public Button Fbutton6;
		public Button Fbutton7;
		public Button Fbutton8;
		public Button Fbutton9;
		public Button Fbutton10;
		public Button Fbutton11;
		public Button Fbutton12;
		public ListBox FlistBox1;
		public RichTextBox FtextBox1;
		public TextBox FtextBox2;
		public TextBox FtextBox3;
		public static string BotCurrency = "N/A";
		public static string DefaultRank;
		public static List<DateTime> BotLastTimes = new List<DateTime>();


		/*
		public static ListBox FlistBox1;
		public static RichTextBox FtextBox1;
		public static TextBox FtextBox2;
		public static TextBox FtextBox3;
		*/

		//public BackgroundWorker RespondToMessages = new BackgroundWorker();
		private TextBox FInputBox1;
		private TextBox FInputBox2;
		public Type response;
		public static List<string> JoinedChats = new List<string>();
		private static bool J_L_eventsAll = false;
		public static bool debugMode = false;
		public static bool FtextBox1AutoScroll = true;
		public static int QueueTimeSeconds = 0;




		public class BotPlayerStorage
		{
			public string name;
			public double currency;
			public string title;
			public string rank;
			public int level;
			public double experiance;

			public BotPlayerStorage(string name, string title = "", string rank = "", int currency = 0,  
									int level = 0, double experiance = 0)
			{
				this.name = name;
				this.currency = currency;
				this.title = title;
				this.rank = rank;
				this.level = level;
				this.experiance = experiance;
			}
		}


		public class BotGames
		{
			public string description;
			public uint duration;
			public uint cooldown;
			public double cost;
			public double prizeCurrency;
			public string prizeExperiance;
			public bool groupGame;
			public float winChance;
			
			
			
			
			
			public BotGames(string Description, uint Duration,
							uint Cooldown, double Cost, double PrizeCurrency, string PrizeExperiance, 
							bool GroupGame, float WinChance)
			{
				this.description = Description;
				this.duration = Duration;
				this.cooldown = Cooldown;
				this.cost = Cost;
				this.prizeCurrency = PrizeCurrency;
				this.prizeExperiance = PrizeExperiance;
				this.groupGame = GroupGame;
				this.winChance = WinChance;
			}
		}

		//Name = 
		//Description = 
		//Duration = 
		//Cooldown = 
		//Cost = 
		//Prize Currency = 
		//Prize Experiance = 
		//Solo Game = 
		//End Of Game


		//				access list of players storage
		//static void Tests()
		//{
		//	List<BotPlayerStorage> af = new List<BotPlayerStorage>();
		//	af.Add(new BotPlayerStorage(5, "asd", "fgh", 6, 84));
		//	foreach (var item in af)
		//	{
		//		if (item.Currency == 5)
		//			item.Currency = 7;
		//			af.Insert(af.IndexOf(item), item);
		//	}
		//}

		//public static List<BotPlayerStorage> test = new List<BotPlayerStorage>();
		






		//static string chatUser, chatChannel, chatMessage;
		public static Dictionary<string, string> ResponsiveCommandsList = new Dictionary<string, string>()
		{
			// Populated from Responsive_Commands.txt
			// ^\ \^ statement
		};

		public static Dictionary<string, string> WhisperCommandsList = new Dictionary<string, string>()
		{
			// Populated from Whisper_Commands.txt
			// ^\ \^ statement
		};

		public static Dictionary<int, string> TimedCommandsList = new Dictionary<int, string>()
		{
			// Populated from Timed_Commands.txt
			// ^\ \^ statement
		};

		public static Dictionary<string, string> ModCommandsList = new Dictionary<string, string>()
		{
			// Populated from Mod_Commands.txt
			// ^\ \^ statement
		};


		public Timer CommandsTimer;
		public static List<string> botMods = new List<string>()
		{
			// Populated from Bot_Moderators.txt
		};

		// Collects all commands on cooldown
		public static Dictionary<string, DateTime> ActiveCooldownsList = new Dictionary<string, DateTime>();

		public static Dictionary<string, List<string>> ActiveGames = new Dictionary<string, List<string>>();

		public static Dictionary<string, BotGames> BotGamesList = new Dictionary<string, BotGames>();

		public static void StartGame(string GameName)
		{
			if(BotGamesList.ContainsKey(GameName))
			{
				ActiveCooldownsList.Add(GameName, DateTime.Now.AddSeconds(BotGamesList[GameName].cooldown));
			}
		}

		public Forms()
		{
			InitializeComponent();
			Ftimer1.Enabled = false;
			ManageFiles();
			LoadBotModerators();
			LoadAllCommands();
			Connect();
			Ftimer1.Enabled = true;
		}


		private static void ManageFiles()
		{
			if (!File.Exists("BotInfo.txt"))
			{
				var asd = new FormInfoFill();
				asd.ShowDialog();
			}
			LoadBotInfo();
			if (!File.Exists("Bot_Moderators.txt"))
				File.WriteAllText("Bot_Moderators.txt", masterName);
			if (!File.Exists("Default_Join_Channels.txt"))
				File.WriteAllText("Default_Join_Channels.txt", masterName);
			if (!File.Exists("Responsive_Commands.txt"))
				File.WriteAllText("Responsive_Commands.txt", "test <###> Yes?");
			if (!File.Exists("Whisper_Commands.txt"))
				File.WriteAllText("Whisper_Commands.txt", "say my name <###> ^\\User\\^");
			if (!File.Exists("Timed_Commands.txt"))
				File.WriteAllText("Timed_Commands.txt", "15 <###> Hello there I'm the bot, nice to meet you!");
			if (!File.Exists("Mod_Commands.txt"))
				File.WriteAllText("Mod_Commands.txt", "!rchelp <###> (chat)Command <###> (chat)Response\r\n" +
					"!whelp <###> (whisper/chat)Command <###> (whisper)Response\r\n" +
					"!mhelp <###> (mods only)Command <###> (chat)Response");
		}



		private static void LoadBotInfo()
		{
			string[] botInfo = File.ReadAllLines("BotInfo.txt");
			foreach (string currentLine in botInfo)
			{
				if (currentLine.Contains("Master Name"))
					masterName = currentLine.Substring(currentLine.IndexOf('=') + 2);
				else if (currentLine.Contains("Bot Name"))
					loginUsername = currentLine.Substring(currentLine.IndexOf('=') + 2);
				else if (currentLine.Contains("Bot OAuthPassword"))
					loginPassword = currentLine.Substring(currentLine.IndexOf('=') + 2);
				else if (currentLine.Contains("Currency Name"))
					BotCurrency = currentLine.Substring(currentLine.IndexOf('=') + 2);
			}
		}

		
		public static void LoadAllCommands()
		{
			LoadResponsiveCommands();
			LoadWhisperCommands();
			LoadTimedCommands();
			LoadModCommands();
		}

		public static void LoadResponsiveCommands()
		{
			string[] getLine = File.ReadAllLines("Responsive_Commands.txt");
			foreach (var line in getLine)
			{

				string command = line.Substring(0, line.IndexOf("<###>") - 1);
				string response = line.Substring(line.IndexOf("<###>") + 6);

				ResponsiveCommandsList.Add(command, response);

			}
		}

		public static void AddResponsiveCommand(string command, string response)
		{
			ResponsiveCommandsList.Add(command, response);
			File.AppendAllText("Responsive_Commands.txt", Environment.NewLine + command + " <###> " + response);
		}

		public static void LoadWhisperCommands()
		{
			string[] getLine = File.ReadAllLines("Whisper_Commands.txt");
			foreach (var line in getLine)
			{

				string command = line.Substring(0, line.IndexOf("<###>") - 1);
				string response = line.Substring(line.IndexOf("<###>") + 6);

				WhisperCommandsList.Add(command, response);

			}
		}

		public static void AddWhisperCommand(string command, string response)
		{
			ResponsiveCommandsList.Add(command, response);
			File.AppendAllText("Whisper_Commands.txt", Environment.NewLine + command + " <###> " + response);
		}


		public static void LoadTimedCommands()
		{
			string[] getLine = File.ReadAllLines("Timed_Commands.txt");
			foreach (var line in getLine)
			{

				string time = line.Substring(0, line.IndexOf("<###>") - 1);
				string message = line.Substring(line.IndexOf("<###>") + 6);
				if(!WhisperCommandsList.ContainsKey(time))
				WhisperCommandsList.Add(time, message);

			}
		}

		public static void AddTimedCommand(int time, string response)
		{
			TimedCommandsList.Add(time, response);
			File.AppendAllText("Timed_Commands.txt", Environment.NewLine + time + " <###> " + response);
		}

		public static void LoadModCommands()
		{
			string[] getLine = File.ReadAllLines("Mod_Commands.txt");
			foreach (var line in getLine)
			{

				string time = line.Substring(0, line.IndexOf("<###>") - 1);
				string message = line.Substring(line.IndexOf("<###>") + 6);
				if (!ModCommandsList.ContainsKey(time))
					ModCommandsList.Add(time, message);

			}
		}

		public static void AddModCommand(string command, string response)
		{
			ModCommandsList.Add(command, response);
			File.AppendAllText("Mod_Commands.txt", Environment.NewLine + command + " <###> " + response);
		}


		public static void LoadBotModerators()
		{
			string[] getLine = File.ReadAllLines("Bot_Moderators.txt");
			foreach (var line in getLine)
			{
				botMods.Add(line);
			}
		}


		public void CheckConnection()
		{
			if (!tcpClient.Connected)
			{
				Connect();
			}
		}

		public void Connect()
		{
			SendMessageQueue = new Queue<string>();
			tcpClient = new TcpClient("irc.chat.twitch.tv", 6667);
			reader = new StreamReader(tcpClient.GetStream());
			writer = new StreamWriter(tcpClient.GetStream())
			{
				AutoFlush = true
			};

			writer.WriteLine("PASS " + loginPassword + Environment.NewLine +
							"NICK " + loginUsername + Environment.NewLine +
							"USER " + loginUsername + " 8 * :" + loginUsername);
			writer.WriteLine("JOIN #jtv");
			FtextBox1.AppendText($"Connecting\r\n");
			JoinDefaultServers();
			LastSentMessage = DateTime.Now;
		}

		public static void JoinDefaultServers()
		{
			string[] filereader = File.ReadAllLines("Default_Join_Channels.txt");
			foreach (string line in filereader)
			{
				writer.WriteLine($"JOIN #{line}");
			}
		}

		private void Ftimer1_Tick(object sender, EventArgs e)
		{
			CheckConnection();
			TrySendMessage();
			TryReceiveMessage();
		}


		private void CommandsTimer_Tick(object sender, EventArgs e)
		{
			foreach (var item in ActiveCooldownsList)
			{
				// Removes all commands off cooldown
				if (item.Value >= DateTime.Now)
					ActiveCooldownsList.Remove(item.Key);
			}
		}


		public static void JoinOrLeaveChannel(string channelName)
		{
			if (JoinedChats.Contains(channelName))
			{
				writer.WriteLine($"PART #{channelName.ToLower()}\r\n");
			}
			else
			{
				writer.WriteLine($"JOIN #{channelName.ToLower()}\r\n");
			}


		}

		

		public static void TrySendMessage()
		{
			if (DateTime.Now - LastSentMessage > TimeSpan.FromSeconds(QueueTimeSeconds))
			{
				if (SendMessageQueue.Count > 0)
				{
					var message = SendMessageQueue.Dequeue();
					writer.WriteLine(message);
					LastSentMessage = DateTime.Now;
				}
			}
		}

		public void TryReceiveMessage()
		{
			if (tcpClient.Available > 0 || reader.Peek() >= 0)
			{
				var Line = reader.ReadLine();
				if (Line == ":tmi.twitch.tv 372 jymparabot :You are in a maze of twisty passages, all alike.")
					FtextBox1.AppendText($"Connected!\r\n");

				if (debugMode)
				{
					FtextBox1.AppendText($"\r\n{Line}\r\n");
				}


				if (Line == "PING :tmi.twitch.tv")
				{
					FtextBox2.AppendText("twitch.tv - PONG\r\n");
					writer.WriteLine("PONG :tmi.twitch.tv");
				}

				else if (Line.Contains(":tmi.twitch.tv 421"))
					FtextBox3.AppendText($"Wrong command!\r\n");

				else if (Line.Contains(":") &&Line.Contains("!") && Line.Contains("@"))
				{

					var chatUser = Line.Substring(Line.IndexOf(':', 0) + 1, Line.IndexOf('!', 0) - Line.IndexOf(':', 0) - 1);
					string conditions;

					if (Line.Substring(1).Contains(":"))
						conditions = Line.Substring(0, Line.IndexOf(':', 1));
					else
						conditions = Line.Substring(1);

					if (conditions.Contains("PRIVMSG"))
					{

						var chatMessage = Line.Substring(Line.IndexOf(':', 1) + 1);
						var chatChannel = Line.Substring(Line.IndexOf('#', 0) + 1, Line.IndexOf(' ', Line.IndexOf('#', 0)) - Line.IndexOf('#', 0) - 1);


						// Filtering buggy chars

						//							 v hidden character
						while (chatMessage.Contains(""))
							chatMessage = chatMessage.Remove(chatMessage.IndexOf(""), 1);


						//						      v hidden character before A
						//if (chatMessage.StartsWith("ACTION"))
						//{
						//	chatMessage = chatMessage.Substring(1, chatMessage.Length - 2);
						//}
						//while (chatMessage.Contains("ﾉ"))
						//	chatMessage = chatMessage.Remove(chatMessage.IndexOf('ﾉ'),1);

						//


						//chatMessage = chatMessage.ToLower();

						//if (ResponsiveCommandsList.ContainsKey(chatMessage))
						//{
						//	string commandResponse = ResponsiveCommandsList[chatMessage];

						//	string responseMessage = CreateIRCMessage(chatChannel, commandResponse);
						//	SendMessageQueue.Enqueue(responseMessage);
						//	//FtextBox2.AppendText(responseMessage);
						//	FtextBox2.AppendText($"[{chatChannel}] {chatUser} <- {commandResponse}\r\n");
						//}

						if (!debugMode)
						{
							OutputColorfulChatMessage(chatChannel, chatUser, chatMessage);
						}

						Dictionary<string, object> CommandReferences = new Dictionary<string, object>()
						{
							{"BotName" , loginUsername },
							{loginUsername , "BotName" },
							{"MasterName" , masterName },
							{masterName , "MasterName" },
							{"User", chatUser },
							//{chatUser , "User" },
							{"ShortTimeNow" , DateTime.Now.ToShortTimeString() },
							{"LongTimeNow" , DateTime.Now.ToLongTimeString() },
							{"ShortDateNow" , DateTime.Now.ToShortDateString() },
							{"LongDateNow" , DateTime.Now.ToLongDateString() },
						};

						// bot mods commands
						if (botMods.Contains(chatUser))
						{
							if (chatMessage.ToLower().StartsWith("!addresponsivecommand"))
							{
								if (!ResponsiveCommandsList.ContainsKey(chatMessage.Substring(12, chatMessage.IndexOf("<###>") - 13)))
								{
									string responsivecommand = chatMessage.Substring(12, chatMessage.IndexOf("<###>") - 13);
									string responsiveresponse = chatMessage.Substring(chatMessage.IndexOf("<###>") + 6);
									AddResponsiveCommand(responsivecommand, responsiveresponse);
								}
							}

							else if (chatMessage.ToLower().StartsWith("!addwhispercommand"))
							{
								if (!WhisperCommandsList.ContainsKey(chatMessage.Substring(12, chatMessage.IndexOf("<###>") - 13)))
								{
									string whispercommand = chatMessage.Substring(12, chatMessage.IndexOf("<###>") - 13);
									string whisperresponse = chatMessage.Substring(chatMessage.IndexOf("<###>") + 6);
									AddWhisperCommand(whispercommand, whisperresponse);
								}
							}

							else if (chatMessage.ToLower().StartsWith("!addtimedcommand"))
							{
								if (!TimedCommandsList.ContainsKey(Convert.ToInt32(chatMessage.Substring(12, chatMessage.IndexOf("<###>") - 13))))
								{
									int time = Convert.ToInt32(chatMessage.Substring(12, chatMessage.IndexOf("<###>") - 13));
									string responsiveresponse = chatMessage.Substring(chatMessage.IndexOf("<###>") + 6);
									AddTimedCommand(time, responsiveresponse);
								}
							}

							else if (chatMessage.ToLower().StartsWith("!addmodcommand"))
							{
								if (!ModCommandsList.ContainsKey(chatMessage.Substring(12, chatMessage.IndexOf("<###>") - 13)))
								{
									string modcommand = chatMessage.Substring(12, chatMessage.IndexOf("<###>") - 13);
									string modresponse = chatMessage.Substring(chatMessage.IndexOf("<###>") + 6);
									AddModCommand(modcommand, modresponse);
								}
							}

							else if (chatMessage.ToLower().StartsWith("!setqueuetime") && botMods.Contains(chatUser))
							{
								QueueTimeSeconds = Convert.ToInt32(chatMessage.Substring("!setqueuetime".Length + 1));
							}
						}





						//						TO DO						#1
						//			tmi.twitch.tv WHISPER jymparabot
						//			tmi.twitch.tv PRIVMSG #fairlight


						//List<object> commandRefs = new List<object>();
						//List<object> responseRefs = new List<object>();
						//while (chatMessage.Contains("^\\") && chatMessage.Contains("\\^"))
						//{

						//	string temp = chatMessage.Substring(chatMessage.IndexOf("^\\") + 2, chatMessage.IndexOf("\\^") - chatMessage.IndexOf("^\\") - 2);
						//	//commandRefs.Add(CommandReferences[temp]);
						//	chatMessage = chatMessage.Replace($"^\\{temp}\\^", $"{{{CommandReferences[temp]}}}");

						//}


						// Converts a chat message into a command line

						string commandCheck = chatMessage;

						foreach (var check in CommandReferences)
						{
							while (commandCheck.Contains(check.Key))
							{
								commandCheck = commandCheck.Replace(check.Key, $"^\\{check.Value.ToString()}\\^");
							}

						}




						//				Message Responses




						// Checks if the command line is on cooldown
						if (!ActiveCooldownsList.ContainsKey(commandCheck))
						{


							//		Responsive Commands
							if (ResponsiveCommandsList.ContainsKey(commandCheck))
							{
								var response = ResponsiveCommandsList[commandCheck];
								while (response.Contains("^\\") && response.Contains("\\^"))
								{

									string temp = response.Substring(response.IndexOf("^\\") + 2, response.IndexOf("\\^") - response.IndexOf("^\\") - 2);
									response = response.Replace($"^\\{temp}\\^", $"{CommandReferences[temp]}");
								}
								SendMessageQueue.Enqueue(CreateChatMessage(chatChannel, response));
								WriteToTextbox(2, $"[{chatChannel}] {chatUser} <- {response}\r\n");
							}


							//		Whisper commands
							else if (WhisperCommandsList.ContainsKey(commandCheck))
							{
								var response = WhisperCommandsList[commandCheck];
								while (response.Contains("^\\") && response.Contains("\\^"))
								{

									string temp = response.Substring(response.IndexOf("^\\") + 2, response.IndexOf("\\^") - response.IndexOf("^\\") - 2);
									response = response.Replace($"^\\{temp}\\^", $"{CommandReferences[temp]}");

								}
								SendMessageQueue.Enqueue(CreateWhisperMessage(chatUser, response));
								WriteToTextbox(2, $"[Whisper] {chatUser} <- {response}\r\n");
							}


							//		Bot Moderator commands
							else if (ModCommandsList.ContainsKey(commandCheck))
							{
								if (botMods.Contains(chatUser))
								{
									var response = ModCommandsList[commandCheck];
									while (response.Contains("^\\") && response.Contains("\\^"))
									{

										string temp = response.Substring(response.IndexOf("^\\") + 2, response.IndexOf("\\^") - response.IndexOf("^\\") - 2);
										response = response.Replace($"^\\{temp}\\^", $"{CommandReferences[temp]}");

									}
									SendMessageQueue.Enqueue(CreateChatMessage(chatChannel, response));
									WriteToTextbox(2, $"[{chatChannel}] {chatUser} <- {response}\r\n");
								}
							}




							// ############################################################################ */

						}

					}



					// Whispers to bot

					else if (conditions.Contains("WHISPER"))
					{

						var chatMessage = Line.Substring(Line.IndexOf(':', 1) + 1);
						

						Dictionary<string, object> CommandReferences = new Dictionary<string, object>()
						{
							{"BotName" , loginUsername },
							{loginUsername , "BotName" },
							{"MasterName" , masterName },
							{masterName , "MasterName" },
							{"User", chatUser },
							//{chatUser , "User" },
							{"ShortTimeNow" , DateTime.Now.ToShortTimeString() },
							{"LongTimeNow" , DateTime.Now.ToLongTimeString() },
							{"ShortDateNow" , DateTime.Now.ToShortDateString() },
							{"LongDateNow" , DateTime.Now.ToLongDateString() },
						};

						
						if (!debugMode)
						{
							OutputColorfulWhisperMessage(chatUser, chatMessage);
						}

						string commandCheck = chatMessage;

						foreach (var check in CommandReferences)
						{
							while (commandCheck.Contains(check.Key))
							{
								commandCheck = commandCheck.Replace(check.Key, $"^\\{check.Value.ToString()}\\^");
							}

						}
					

						if (WhisperCommandsList.ContainsKey(commandCheck))
						{
							var response = WhisperCommandsList[commandCheck];
							while (response.Contains("^\\") && response.Contains("\\^"))
							{

								string temp = response.Substring(response.IndexOf("^\\") + 2, response.IndexOf("\\^") - response.IndexOf("^\\") - 2);
								response = response.Replace($"^\\{temp}\\^", $"{CommandReferences[temp]}");

							}
							SendMessageQueue.Enqueue(CreateWhisperMessage(chatUser, response));
							WriteToTextbox(2, $"[Whisper] {chatUser} <- {response}\r\n");
						}
						
					}


					//					Leave/Join for the bot only
					if (conditions.Contains(loginUsername) && conditions.Contains("JOIN"))
					{
						var chatChannel = conditions.Substring(conditions.IndexOf('#', 0) + 1);
						if (chatChannel != "jtv")
						{
							WriteToTextbox(3, $"[{chatChannel}] {loginUsername} JOINED\r\n");
							JoinedChats.Add(chatChannel);
							FlistBox1.Items.Add(chatChannel);
						}
					}
					else if (conditions.Contains(loginUsername) && conditions.Contains("PART"))
					{
						var chatChannel = conditions.Substring(conditions.IndexOf('#', 0) + 1);
						WriteToTextbox(3, $"[{chatChannel}] {loginUsername} LEFT\r\n");
						JoinedChats.Remove(chatChannel);
						FlistBox1.Items.Remove(chatChannel);
					}

					//					Leave/Join event for all viewers
					if (J_L_eventsAll == true)
					{
						if (Line.Contains("JOIN"))
						{
							var chatChannel = Line.Substring(Line.IndexOf('#', 0) + 1);
							WriteToTextbox(3, $"[{chatChannel}] {chatUser} JOINED\r\n");
						}

						else if (Line.Contains("PART"))
						{
							var chatChannel = Line.Substring(Line.IndexOf('#', 0) + 1);
							WriteToTextbox(3, $"[{chatChannel}] {chatUser} LEFT\r\n");
						}
					}
					


				}
			}

		}







		public void WriteToTextbox(int TextboxNumber, string Text)
		{
			switch (TextboxNumber)
			{
				case 1:
					FtextBox1.AppendText(Text);
					break;
				case 2:
					FtextBox2.AppendText(Text);
					break;
				case 3:
					FtextBox3.AppendText(Text);
					break;
				default:
					break;
			}
		}

		public string TemplateChatMessage(string Channel, string ChatUser, string Message)
		{
			return $"[{Channel}] {ChatUser} <- {Message}\r\n";
		}

		public string TemplateWhisperMessage(string ChatUser, string Message)
		{
			return $"[Whisper] {ChatUser} <- {Message}\r\n";
		}


		public static string CreateChatMessage(string Channel, string Message)
		{
			return $":{loginUsername}!{loginUsername}@{loginUsername}.tmi.twitch.tv PRIVMSG #{Channel} :{Message}";
		}

		public static string CreateWhisperMessage(string ChatUser, string Message)
		{
			return $":{loginUsername}!{loginUsername}@{loginUsername}.tmi.twitch.tv PRIVMSG #jtv :/w {ChatUser} {Message}";
		}

		public void OutputColorfulChatMessage(string channelName, string userName, string message)
		{

			//FtextBox1.AppendText($"\r\n[{chatChannel}] {chatUser}: {chatMessage}\r\n");

			
			Color channelColor = Color.Blue;
			Color userColor = Color.Green;
			Color messageColor = Color.Black;
			bool me=false;
			if (message.StartsWith("ACTION"))
			{
				me = true;
				message = message.Remove(message.IndexOf("ACTION"), 6);
			}
			//FtextBox1.Font = new Font("Microsoft Sans Serif", 13F);
			FtextBox1.SelectionColor = FtextBox1.ForeColor;
			FtextBox1.AppendText($"[");
			FtextBox1.SelectionColor = channelColor;
			FtextBox1.AppendText(channelName);
			FtextBox1.SelectionColor = FtextBox1.ForeColor;
			FtextBox1.AppendText("] ");
			FtextBox1.SelectionColor = userColor;
			FtextBox1.AppendText(userName);
			if(!me)
			FtextBox1.SelectionColor = messageColor;
			FtextBox1.AppendText($": {message}\r\n");
		}

		public void OutputColorfulWhisperMessage(string userName, string message)
		{

			//FtextBox1.AppendText($"\r\n[{chatChannel}] {chatUser}: {chatMessage}\r\n");


			Color whisperColor = Color.Red;
			Color userColor = Color.Green;
			Color messageColor = Color.Black;
			//FtextBox1.Font = new Font("Microsoft Sans Serif", 13F);
			FtextBox1.SelectionColor = Color.Black;
			FtextBox1.AppendText($"[");
			FtextBox1.SelectionColor = whisperColor;
			FtextBox1.AppendText("Whisper");
			FtextBox1.SelectionColor = Color.Black;
			FtextBox1.AppendText("] ");
			FtextBox1.SelectionColor = userColor;
			FtextBox1.AppendText(userName);
			FtextBox1.SelectionColor = messageColor;
			FtextBox1.AppendText($": {message}\r\n");
		}
		



		private void FInputBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				if (FlistBox1.SelectedIndex < 0)
					FlistBox1.SelectedIndex = 0;
				string channel = FlistBox1.SelectedItem.ToString();
				string message = FInputBox1.Text;
				writer.WriteLine($":{loginUsername}!{loginUsername}@{loginUsername}.tmi.twitch.tv PRIVMSG #{channel} :{message}");
				OutputColorfulChatMessage(channel, loginUsername, message);
				//JoinOrLeaveChannel(FInputBox1.Text);
				FInputBox1.ResetText();
			}
		}

		private void FInputBox2_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				JoinOrLeaveChannel(FInputBox2.Text);
				FInputBox2.ResetText();
			}
		}


		private void FtextBox1_TextChanged(object sender, EventArgs e)
		{
			if (FtextBox1AutoScroll)
			{
				FtextBox1.ScrollToCaret();
			}
		}


		//					BUTTONS



		private void Fbutton1_Click(object sender, EventArgs e)
		{
			Form1 formNew = new Form1();
			formNew.Show();
		}

		private void Fbutton8_Click(object sender, EventArgs e)
		{
			if (FtextBox1AutoScroll)
			{
				Fbutton8.BackColor = Color.Red;
				FtextBox1AutoScroll = false;
			}
			else
			{
				Fbutton8.BackColor = Color.Green;
				FtextBox1AutoScroll = true;
			}
		}

		private void Fbutton9_Click(object sender, EventArgs e)
		{
			if (debugMode)
			{
				Fbutton9.BackColor = Color.Red;
				debugMode = false;
			}
			else
			{
				Fbutton9.BackColor = Color.Green;
				debugMode = true;
			}
		}

		private void Fbutton10_Click(object sender, EventArgs e)
		{
			Fbutton10.BackColor = Color.Green;
			writer.WriteLine("CAP REQ :twitch.tv/commands");
		}

		private void Fbutton11_Click(object sender, EventArgs e)
		{
			Fbutton11.BackColor = Color.Green;
			writer.WriteLine("CAP REQ :twitch.tv/membership");
		}

		
		private void Fbutton12_Click(object sender, EventArgs e)
		{
			if (J_L_eventsAll)
			{
				Fbutton12.BackColor = Color.Red;
				J_L_eventsAll = false;
			}
			else
			{
				Fbutton12.BackColor = Color.Green;
				J_L_eventsAll = true;
			}
		}


		//					BUTTONS

		private void FInputBox1_Enter(object sender, EventArgs e)
		{
			FInputBox1.ResetText();
		}


		private void FlistBox1_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				JoinOrLeaveChannel(FlistBox1.SelectedItem.ToString());
				FlistBox1.Items.Remove(FlistBox1.SelectedItem);
			}
		}


		private void Fbutton2_Click(object sender, EventArgs e)
		{
			string[] chatLog = FtextBox1.Text.Split('\n');
			if (!Directory.Exists("ChatLogs"))
				Directory.CreateDirectory("ChatLogs");
			File.WriteAllLines($"ChatLogs/ChatLog{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToShortTimeString().Replace(':', '.')}.txt", contents: chatLog);
		}


		private void Forms_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control == true)
			{
				if (e.Alt == true)
				{
					if (e.Shift == true)
					{
						if (e.KeyValue == 116) // F5
						{
							new SpamBot().Show();
						}
					}
				}
			}
		}
		

		private void Forms_FormClosing(object sender, FormClosingEventArgs e)
		{
			new FormConfirmAppClose().ShowDialog();
			e.Cancel = true;
		}


		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Forms));
			this.Fbutton2 = new System.Windows.Forms.Button();
			this.Fbutton1 = new System.Windows.Forms.Button();
			this.Ftimer1 = new System.Windows.Forms.Timer(this.components);
			this.Fbutton3 = new System.Windows.Forms.Button();
			this.Fbutton4 = new System.Windows.Forms.Button();
			this.Fbutton5 = new System.Windows.Forms.Button();
			this.Fbutton6 = new System.Windows.Forms.Button();
			this.Fbutton7 = new System.Windows.Forms.Button();
			this.Fbutton8 = new System.Windows.Forms.Button();
			this.Fbutton9 = new System.Windows.Forms.Button();
			this.Fbutton10 = new System.Windows.Forms.Button();
			this.Fbutton11 = new System.Windows.Forms.Button();
			this.FInputBox1 = new System.Windows.Forms.TextBox();
			this.Fbutton12 = new System.Windows.Forms.Button();
			this.FInputBox2 = new System.Windows.Forms.TextBox();
			this.FtextBox1 = new System.Windows.Forms.RichTextBox();
			this.FtextBox2 = new System.Windows.Forms.TextBox();
			this.FtextBox3 = new System.Windows.Forms.TextBox();
			this.FlistBox1 = new System.Windows.Forms.ListBox();
			this.CommandsTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// Fbutton2
			// 
			this.Fbutton2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton2.Location = new System.Drawing.Point(1035, 46);
			this.Fbutton2.Name = "Fbutton2";
			this.Fbutton2.Size = new System.Drawing.Size(145, 32);
			this.Fbutton2.TabIndex = 0;
			this.Fbutton2.Text = "Get Log";
			this.Fbutton2.UseVisualStyleBackColor = true;
			this.Fbutton2.Click += new System.EventHandler(this.Fbutton2_Click);
			// 
			// Fbutton1
			// 
			this.Fbutton1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton1.Location = new System.Drawing.Point(1035, 12);
			this.Fbutton1.Margin = new System.Windows.Forms.Padding(50, 3, 50, 3);
			this.Fbutton1.Name = "Fbutton1";
			this.Fbutton1.Size = new System.Drawing.Size(145, 32);
			this.Fbutton1.TabIndex = 1;
			this.Fbutton1.Text = "Command Line";
			this.Fbutton1.UseVisualStyleBackColor = true;
			this.Fbutton1.Click += new System.EventHandler(this.Fbutton1_Click);
			// 
			// Ftimer1
			// 
			this.Ftimer1.Enabled = true;
			this.Ftimer1.Interval = 250;
			this.Ftimer1.Tick += new System.EventHandler(this.Ftimer1_Tick);
			// 
			// Fbutton3
			// 
			this.Fbutton3.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton3.Location = new System.Drawing.Point(1035, 80);
			this.Fbutton3.Name = "Fbutton3";
			this.Fbutton3.Size = new System.Drawing.Size(145, 32);
			this.Fbutton3.TabIndex = 4;
			this.Fbutton3.UseVisualStyleBackColor = true;
			// 
			// Fbutton4
			// 
			this.Fbutton4.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton4.Location = new System.Drawing.Point(1035, 114);
			this.Fbutton4.Name = "Fbutton4";
			this.Fbutton4.Size = new System.Drawing.Size(145, 32);
			this.Fbutton4.TabIndex = 5;
			this.Fbutton4.UseVisualStyleBackColor = true;
			// 
			// Fbutton5
			// 
			this.Fbutton5.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton5.Location = new System.Drawing.Point(1035, 148);
			this.Fbutton5.Name = "Fbutton5";
			this.Fbutton5.Size = new System.Drawing.Size(145, 32);
			this.Fbutton5.TabIndex = 6;
			this.Fbutton5.UseVisualStyleBackColor = true;
			// 
			// Fbutton6
			// 
			this.Fbutton6.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton6.Location = new System.Drawing.Point(1035, 182);
			this.Fbutton6.Name = "Fbutton6";
			this.Fbutton6.Size = new System.Drawing.Size(145, 32);
			this.Fbutton6.TabIndex = 7;
			this.Fbutton6.UseVisualStyleBackColor = true;
			// 
			// Fbutton7
			// 
			this.Fbutton7.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton7.Location = new System.Drawing.Point(1035, 216);
			this.Fbutton7.Name = "Fbutton7";
			this.Fbutton7.Size = new System.Drawing.Size(145, 32);
			this.Fbutton7.TabIndex = 8;
			this.Fbutton7.UseVisualStyleBackColor = true;
			// 
			// Fbutton8
			// 
			this.Fbutton8.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton8.BackColor = System.Drawing.Color.Green;
			this.Fbutton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton8.Location = new System.Drawing.Point(1035, 250);
			this.Fbutton8.Name = "Fbutton8";
			this.Fbutton8.Size = new System.Drawing.Size(145, 32);
			this.Fbutton8.TabIndex = 9;
			this.Fbutton8.Text = "AutoScroll Box 1";
			this.Fbutton8.UseVisualStyleBackColor = false;
			this.Fbutton8.Click += new System.EventHandler(this.Fbutton8_Click);
			// 
			// Fbutton9
			// 
			this.Fbutton9.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton9.BackColor = System.Drawing.Color.Red;
			this.Fbutton9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton9.Location = new System.Drawing.Point(1035, 284);
			this.Fbutton9.Name = "Fbutton9";
			this.Fbutton9.Size = new System.Drawing.Size(145, 32);
			this.Fbutton9.TabIndex = 10;
			this.Fbutton9.Text = "Debug Mode";
			this.Fbutton9.UseVisualStyleBackColor = false;
			this.Fbutton9.Click += new System.EventHandler(this.Fbutton9_Click);
			// 
			// Fbutton10
			// 
			this.Fbutton10.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton10.BackColor = System.Drawing.Color.Red;
			this.Fbutton10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton10.Location = new System.Drawing.Point(1035, 318);
			this.Fbutton10.Name = "Fbutton10";
			this.Fbutton10.Size = new System.Drawing.Size(145, 32);
			this.Fbutton10.TabIndex = 11;
			this.Fbutton10.Text = "Whispers On/Off";
			this.Fbutton10.UseVisualStyleBackColor = false;
			this.Fbutton10.Click += new System.EventHandler(this.Fbutton10_Click);
			// 
			// Fbutton11
			// 
			this.Fbutton11.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton11.BackColor = System.Drawing.Color.Red;
			this.Fbutton11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton11.Location = new System.Drawing.Point(1035, 352);
			this.Fbutton11.Name = "Fbutton11";
			this.Fbutton11.Size = new System.Drawing.Size(145, 32);
			this.Fbutton11.TabIndex = 12;
			this.Fbutton11.Text = "Join/Leave On/Off";
			this.Fbutton11.UseVisualStyleBackColor = false;
			this.Fbutton11.Click += new System.EventHandler(this.Fbutton11_Click);
			// 
			// FInputBox1
			// 
			this.FInputBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.FInputBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.FInputBox1.Location = new System.Drawing.Point(630, 652);
			this.FInputBox1.Name = "FInputBox1";
			this.FInputBox1.Size = new System.Drawing.Size(400, 26);
			this.FInputBox1.TabIndex = 14;
			this.FInputBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FInputBox1_KeyPress);
			// 
			// Fbutton12
			// 
			this.Fbutton12.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.Fbutton12.BackColor = System.Drawing.Color.Red;
			this.Fbutton12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Fbutton12.Location = new System.Drawing.Point(1035, 386);
			this.Fbutton12.Name = "Fbutton12";
			this.Fbutton12.Size = new System.Drawing.Size(145, 32);
			this.Fbutton12.TabIndex = 15;
			this.Fbutton12.Text = "All Join/Leave";
			this.Fbutton12.UseVisualStyleBackColor = false;
			this.Fbutton12.Click += new System.EventHandler(this.Fbutton12_Click);
			// 
			// FInputBox2
			// 
			this.FInputBox2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.FInputBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.FInputBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.FInputBox2.Location = new System.Drawing.Point(1036, 550);
			this.FInputBox2.Name = "FInputBox2";
			this.FInputBox2.Size = new System.Drawing.Size(144, 26);
			this.FInputBox2.TabIndex = 19;
			this.FInputBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FInputBox2_KeyPress);
			// 
			// FtextBox1
			// 
			this.FtextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
			this.FtextBox1.Location = new System.Drawing.Point(0, 0);
			this.FtextBox1.Name = "FtextBox1";
			this.FtextBox1.ReadOnly = true;
			this.FtextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.FtextBox1.Size = new System.Drawing.Size(624, 678);
			this.FtextBox1.TabIndex = 17;
			this.FtextBox1.Text = "";
			this.FtextBox1.TextChanged += new System.EventHandler(this.FtextBox1_TextChanged);
			// 
			// FtextBox2
			// 
			this.FtextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
			this.FtextBox2.Location = new System.Drawing.Point(629, 0);
			this.FtextBox2.Multiline = true;
			this.FtextBox2.Name = "FtextBox2";
			this.FtextBox2.ReadOnly = true;
			this.FtextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.FtextBox2.Size = new System.Drawing.Size(400, 435);
			this.FtextBox2.TabIndex = 3;
			// 
			// FtextBox3
			// 
			this.FtextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
			this.FtextBox3.Location = new System.Drawing.Point(630, 441);
			this.FtextBox3.Multiline = true;
			this.FtextBox3.Name = "FtextBox3";
			this.FtextBox3.ReadOnly = true;
			this.FtextBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.FtextBox3.Size = new System.Drawing.Size(400, 205);
			this.FtextBox3.TabIndex = 13;
			// 
			// FlistBox1
			// 
			this.FlistBox1.FormattingEnabled = true;
			this.FlistBox1.ItemHeight = 18;
			this.FlistBox1.Location = new System.Drawing.Point(1036, 582);
			this.FlistBox1.Name = "FlistBox1";
			this.FlistBox1.Size = new System.Drawing.Size(144, 94);
			this.FlistBox1.TabIndex = 18;
			this.FlistBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FlistBox1_KeyUp);
			// 
			// CommandsTimer
			// 
			this.CommandsTimer.Enabled = true;
			this.CommandsTimer.Interval = 1000;
			this.CommandsTimer.Tick += new System.EventHandler(this.CommandsTimer_Tick);
			// 
			// Forms
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(1180, 678);
			this.Controls.Add(this.FInputBox2);
			this.Controls.Add(this.Fbutton12);
			this.Controls.Add(this.FInputBox1);
			this.Controls.Add(this.Fbutton11);
			this.Controls.Add(this.Fbutton10);
			this.Controls.Add(this.Fbutton9);
			this.Controls.Add(this.Fbutton8);
			this.Controls.Add(this.Fbutton7);
			this.Controls.Add(this.Fbutton6);
			this.Controls.Add(this.Fbutton5);
			this.Controls.Add(this.Fbutton4);
			this.Controls.Add(this.Fbutton3);
			this.Controls.Add(this.Fbutton1);
			this.Controls.Add(this.Fbutton2);
			this.Controls.Add(this.FlistBox1);
			this.Controls.Add(this.FtextBox2);
			this.Controls.Add(this.FtextBox1);
			this.Controls.Add(this.FtextBox3);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "Forms";
			this.Text = "Twitch Chat Bot - Main";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Forms_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Forms_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		





























		// Player stats template

		// bot file
		// <name> <title> <rank> <currency> <>







		// TO DO add <#1#> <#2#> <#3#> for exact command / starts with / any capitalization

		// TO DO 





		/*

						Multithreading fail TO DO							#9001

		static Thread openForm1 = new Thread(new ThreadStart(threadForm1));
		static Thread openForm2 = new Thread(new ThreadStart(threadForm2));

		
		private static void threadForm1()
		{
			var frm = new Form1();
			frm.ShowDialog();
		}
		private static void threadForm2()
		{
			var frm = new Form2();
			frm.ShowDialog();
		}


		private void buttonFtF1_Click(object sender, EventArgs e)
		{
			if (!openForm1.IsAlive)
			{
				openForm1 = new Thread(new ThreadStart(threadForm1));
				openForm1.Start();
			}
		}
		
		private void buttonFtF2_Click(object sender, EventArgs e)
		{
			if (!openForm2.IsAlive)
			{
				openForm2 = new Thread(new ThreadStart(threadForm2));
				openForm2.Start();
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			if (openForm1.IsAlive)
				openForm1.Abort();
			if (openForm2.IsAlive)
				openForm2.Abort();
			base.OnClosed(e);
		}

		//			Auto Open all windows
		protected override void OnLoad(EventArgs e)
		{
			if (!openForm1.IsAlive)
			{
				openForm1 = new Thread(new ThreadStart(threadForm1));
				openForm1.Start();
			}
			if (!openForm2.IsAlive)
			{
				openForm2 = new Thread(new ThreadStart(threadForm2));
				openForm2.Start();
			}
			base.OnLoad(e);
		}

			Form1.F1textBox1.Invoke((MethodInvoker)delegate
						{
							Form1.F1textBox1.AppendText($"\r\n {reader.ReadLine()}");
						});


		*/








		//Master Name = jympara
		//Bot Name = jymparabot
		//Bot OAuthPassword = oauth:akst7974f0otaehuxe01wo2nd5vxcj
	}
}
