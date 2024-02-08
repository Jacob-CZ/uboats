using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Type;
using System.Numerics;

namespace uboats
{
    internal class Program
    {
        static Db db { get; set; }
        static Tools tools { get; set; }
        static int sizex { get; set; }
        static int sizey { get; set; }
        static int move { get; set; } = 0;

        static int[,] oponentglobal { get; set; }
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            db = new Db();
            tools = new Tools();
            string[] option = { "create", "join" };
            int ind = tools.selector(option);
            if (ind == 0)
            {
                await db.CreateGame();
            }
            else
            {
                await db.JoinGame();
            }
            sizex = db.x;
            sizey = db.y;
            db.shipglobal = new int[sizey, sizex];
            oponentglobal = new int[sizey, sizex];
            shipBuilder(4);
            await game();

        }

        static async Task game()
        {
            bool finished = false;
            while (!finished)
            {

                if (db.move % 2 == db.modder)
                {
                    await GameSelector();

                }
                else
                {
                    ship_write(db.OpponentCursorX, db.OpponentCurosrY, db.shipglobal, false, true);
                    Console.WriteLine();
                    ship_write(-1, -1, oponentglobal, false, false);
                    Thread.Sleep(500);
                }

            }
        }


        static void WriteNewBoard(int[,] board)
        {
            char[] alphabet = new char[]
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G',
                'H', 'I', 'J', 'K', 'L', 'M', 'N',
                'O', 'P', 'Q', 'R', 'S', 'T',
                'U', 'V', 'W', 'X', 'Y', 'Z'
            };

            Console.Clear();
            for (int i = 0; i < sizex; i++)
            {
                Console.Write(alphabet[i]);
            }
            Console.WriteLine();

        }
        static async Task GameSelector()
        {
            db.CreateMove();
            int index = 0;
            int cursorx = 0;
            int cursory = 0;
            int[,] ship = oponentglobal;
            bool enered = false;
            do
            {
                ship_write(-1, -1, db.shipglobal, false, true);
                Console.WriteLine();
                ship_write(cursorx, cursory, ship, enered, false);
                enered = false;
                ConsoleKey sup = Console.ReadKey().Key;
                if (sup == ConsoleKey.DownArrow && cursory < sizey - 1)
                {
                    cursory++;

                }
                else if (sup == ConsoleKey.UpArrow && cursory > 0)
                {
                    cursory -= 1;

                }
                else if (sup == ConsoleKey.RightArrow && cursorx < sizex - 1)
                {
                    cursorx++;

                }
                else if (sup == ConsoleKey.LeftArrow && cursorx > 0)
                {
                    cursorx -= 1;

                }
                else if (sup == ConsoleKey.Enter)
                {
                    index++;
                    enered = true;
                }
                await db.UpdateCursorPositon(cursorx, cursory);
            } while (index != 1);
            ship_write(-1, -1, db.shipglobal, false, true);
            Console.WriteLine();
            ship_write(cursorx, cursory, ship, enered, false);
            //db1.writeArray(ship);

            await db.SetMoveDB(cursorx, cursory);
        }
        static void shipBuilder(int no_of_blocks)
        {
            int index = 0;
            int cursorx = 0;
            int cursory = 0;
            int[,] ship = new int[sizey, sizex];
            bool enered = false;
            do
            {
                ship_write(cursorx, cursory, ship, enered, true);
                enered = false;
                ConsoleKey sup = Console.ReadKey().Key;
                if (sup == ConsoleKey.DownArrow && cursory < sizey - 1)
                {
                    cursory++;

                }
                else if (sup == ConsoleKey.UpArrow && cursory > 0)
                {
                    cursory -= 1;

                }
                else if (sup == ConsoleKey.RightArrow && cursorx < sizex - 1)
                {
                    cursorx++;

                }
                else if (sup == ConsoleKey.LeftArrow && cursorx > 0)
                {
                    cursorx -= 1;

                }
                else if (sup == ConsoleKey.Enter)
                {
                    index++;
                    enered = true;
                }

            } while (index != no_of_blocks);
            ship_write(cursorx, cursory, ship, enered, true);
            //db1.writeArray(ship);

            db.shipglobal = ship;
        }
        static void ship_write(int cx, int cy, int[,] ship, bool enter, bool clear)
        {
            if (clear) Console.Clear();
            for (int i = 0; i < sizex; i++)
            {
                for (int j = 0; j < sizey; j++)
                {
                    if (cx == j && cy == i && !enter)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("█");
                        Console.Write("█");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (cx == j && cy == i && enter)
                    {
                        ship[cx, cy] = 1;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("█");
                        Console.Write("█");
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    else if (ship[j, i] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("█");
                        Console.Write("█");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (ship[j, i ] == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("█");
                        Console.Write("█");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("█");
                        Console.Write("█");
                    }

                }
                Console.WriteLine();
            }

        }
        static void shooter()
        {
            int index = 0;
            int cursorx = 0;
            int cursory = 0;
            int[,] ship = new int[sizey, sizex];
            bool enered = false;
            do
            {
                ship_write(cursorx, cursory, ship, enered, true);
                enered = false;
                ConsoleKey sup = Console.ReadKey().Key;
                if (sup == ConsoleKey.DownArrow && cursory < sizey - 1)
                {
                    cursory++;

                }
                else if (sup == ConsoleKey.UpArrow && cursory > 0)
                {
                    cursory -= 1;

                }
                else if (sup == ConsoleKey.RightArrow && cursorx < sizex - 1)
                {
                    cursorx++;

                }
                else if (sup == ConsoleKey.LeftArrow && cursorx > 0)
                {
                    cursorx -= 1;

                }
                else if (sup == ConsoleKey.Enter)
                {
                    index++;
                    enered = true;
                }

            } while (true);
            ship_write(cursorx, cursory, ship, enered, true);


        }



    }

    public class Db
    {
        public int[,] shipglobal { get; set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public string name { get; private set; }
        public int move { get; set; }
        public int modder { get; set; }
        public string username { get; private set; }
        private bool signedIn { get; set; } = false;
        private Tools Tools { get; set; }
        private CollectionReference gameref { get; set; }
        private CollectionReference userref { get; set; }
        private FirestoreDb myDB { get; set; }
        public string Id { get; set; }
        public int OpponentCursorX { get; private set; }
        public int OpponentCurosrY { get; private set; }
        public int Localhitx { get; private set; }
        public int Localhity { get; private set; }
        public Db()
        {
            Id = "u-boats";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "C:\\Users\\kuba\\Downloads\\u-boats-firebase-adminsdk-cjr1k-b6bbc94f00.json");
            myDB = FirestoreDb.Create(Id);
            gameref = myDB.Collection("games");
            userref = myDB.Collection("users");
            Tools = new Tools();
        }
        public async Task SignUserIn()
        {
            Console.WriteLine("username");
            username = Console.ReadLine();

            DocumentSnapshot usersnap = await userref.Document(username).GetSnapshotAsync();
            Dictionary<string, object> data = new Dictionary<string, object>();
            data = usersnap.ToDictionary();
            do
            {
                Console.WriteLine("password");
                string password = Console.ReadLine();
                if (data == null)
                {
                    Console.WriteLine("username doesnt exist adding account");
                    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                    keyValuePairs.Add("username", username);
                    keyValuePairs.Add("password", password);
                    await userref.Document(username).CreateAsync(keyValuePairs);
                    signedIn = true;
                }
                else if (data != null && data.TryGetValue("password", out object passObj))
                {
                    signedIn = Convert.ToString(passObj) == password;
                }
            } while (!signedIn);


            Console.WriteLine("signed in press any key to continue");
            Console.ReadKey();

        }
        public async Task CreateGame()
        {
            await SignUserIn();
            bool player2 = false;
            Console.WriteLine("name");
            name = Console.ReadLine();
            Console.WriteLine("x");
            x = Tools.parser();
            Console.WriteLine("y");
            y = Tools.parser();
            move = 0;
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "x", x },
                { "y", y },
                { "move" ,  move },
                { "player1" , true },
                { "player2" , false }
            };
            gameref.Document(name).SetAsync(updates).Wait();
            while (!player2)
            {
                DocumentSnapshot snap = await gameref.Document(name).GetSnapshotAsync();
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict = snap.ToDictionary();
                if (dict.TryGetValue("player2", out object test))
                {
                    player2 = Convert.ToBoolean(test);
                }
                Thread.Sleep(500);
            }
            SetupDocumentListener();
            Console.WriteLine("connected");
            modder = 0;
        }

        public async Task SetMoveDB(int hitx, int hity)
        {
            Dictionary<string, object> theMove = new Dictionary<string, object>
            {
                { "hitx", hitx },
                { "hity", hity },

            };
            await gameref.Document(name).Collection("moves").Document(move.ToString()).UpdateAsync(theMove);
            Thread.Sleep(500);
            move++;
            await CreateMove();
            SetupDocumentListener();
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "move" ,  move }

            };

            await gameref.Document(name).UpdateAsync(updates);
        }


        public void Write2dArrayToDB(int[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                int[] currentArray = new int[array.GetLength(1)];
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    currentArray[j] = array[i, j];
                }
                Dictionary<string, object> arrayDic = new Dictionary<string, object>
                {
                    { "array" + i.ToString() , currentArray}
                };

                gameref.Document(name).UpdateAsync(arrayDic).Wait();
            }
        }

        public async Task<String[]> GetCurrentGames()
        {
            QuerySnapshot snapshot = await gameref.GetSnapshotAsync();
            if (snapshot.Count == 0)
            {
                return new string[] { };
            }

            List<string> games = new List<string>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                string id = document.Id;
                games.Add(id);
            }
            return games.ToArray();
        }
        public async Task JoinGame()
        {
            await SignUserIn();
            string[] games = await GetCurrentGames();
            Dictionary<string, object> dict = new Dictionary<string, object>();

            if (games.Length > 0)
            {
                Console.WriteLine("Available games: " + string.Join(", ", games));
            }
            else
            {
                Console.WriteLine("No games available.");
                Environment.Exit(0);
            }
            string game = games[Tools.selector(games)];
            if (games.Contains(game) && game != "\n")
            {
                DocumentSnapshot snap;
                snap = await gameref.Document(game).GetSnapshotAsync();
                dict = snap.ToDictionary();
            }
            if (dict.TryGetValue("x", out object xValue) && dict.TryGetValue("y", out object yValue))
            {
                x = Convert.ToInt32(xValue);
                y = Convert.ToInt32(yValue);
                Dictionary<string, object> ready = new Dictionary<string, object>
                {
                    {"player2" , true}
                };

                await gameref.Document(game).UpdateAsync(ready);
                name = game;

            }
            else
            {
                Console.WriteLine("The document does not contain 'x' or 'y' fields.");
                Environment.Exit(0);
            }
            modder = 1;
            SetupDocumentListener();
            Thread.Sleep(500);

        }
        public async Task UpdateCursorPositon(int cursorx, int cursory)
        {
            Dictionary<string, object> moves = new Dictionary<string, object>
            {
                { "cursorx", cursorx },
                { "cursory", cursory }
            };
            await gameref.Document(name).Collection("moves").Document(move.ToString()).UpdateAsync(moves);
        }
        public void SetupDocumentListener()
        {
            DocumentReference docRef = gameref.Document(name);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            object turn;
            // Listen to the snapshot
            docRef.Listen(snapshot =>
            {
                if (snapshot.Exists)
                {
                    dict = snapshot.ToDictionary();
                }
                if (dict.TryGetValue("move", out turn))
                {
                    SetMoveNum(Convert.ToInt32(turn));
                }
            });
            DocumentReference moveref = gameref.Document(name).Collection("moves").Document(move.ToString());
            Dictionary<string, object> cursorDict = new Dictionary<string, object>();
            object cursorx;
            object cursory;
            object enterx;
            object entery;
            moveref.Listen(snapshot =>
            {
                if (snapshot.Exists)
                {
                    cursorDict = snapshot.ToDictionary();
                }
                if (cursorDict.TryGetValue("cursorx", out cursorx))
                {
                    OpponentCursorX = Convert.ToInt32(cursorx);

                }
                if (cursorDict.TryGetValue("cursory", out cursory))
                {
                    OpponentCurosrY = Convert.ToInt32(cursory);
                }
                if(cursorDict.TryGetValue("hitx", out enterx) && cursorDict.TryGetValue("hity", out entery))
                {
                    Localhity = Convert.ToInt32(enterx);
                    Console.WriteLine(Localhitx);
                    Localhitx = Convert.ToInt32(entery);
                    Console.WriteLine(Localhity);
                    shipglobal[Localhity, Localhitx] = 2;
                }

                
            }
            );
        }
        private async Task SetMoveNum(int moveLocal)
        {

            move = moveLocal;
        }
        public async Task CreateMove()
        {
            Dictionary<string, object> exists = new Dictionary<string, object>
            {
                {"exists", true }
            };
            await gameref.Document(name).Collection("moves").Document(move.ToString()).CreateAsync(exists);
        }



    }
    public class Tools
    {
        public int parser()
        {
            bool parsed = false;
            int num;
            do
            {
                if (int.TryParse(Console.ReadLine(), out num))
                {
                    parsed = true;
                }
                else
                {
                    Console.WriteLine("try again");
                }
            } while (!parsed);
            return num;
        }
        public int selector(string[] options)
        {

            int index = 0;
            bool selected = false;
            do
            {
                Console.Clear();
                for (int i = 0; i < options.Length; i++)
                {

                    if (i == index)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(" " + options[i]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(" " + options[i]);
                    }

                }
                ConsoleKey key = Console.ReadKey().Key;
                if ((key == ConsoleKey.LeftArrow) && index > 0)
                {
                    index--;
                }
                else if ((key == ConsoleKey.RightArrow) && index < options.Length - 1)
                {
                    index++;
                }
                else if (key == ConsoleKey.Enter)
                {
                    selected = true;
                }

            } while (!selected);
            for (int i = 0; i < options.Length; i++)
            {
                if (selected && i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" " + options[i]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(" " + options[i]);
                }
            }
            Thread.Sleep(500);
            Console.Clear();

            return index;
        }
    }
}
