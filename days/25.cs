using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MoreLinq.Extensions.SubsetsExtension;

namespace adv_of_code_2019
{
    public static class Day25
    {
        public static async Task Run ()
        {
            Console.WriteLine ("Part 1: " + await Part1Async (await File.ReadAllTextAsync ("inputs\\25.txt")));

            Console.WriteLine ("Part 2: " + Part2 ());
        }

        private static bool isInteractiveOutput = false;

        public static async Task<string> Part1Async (string input)
        {
            var intMachine = new painter.SynchronousIntMachine (input);
            var (rooms, collectedItems) = await DiscoverRoomsAndGatherItemsAsync (intMachine);
            var hullBreachRoom = rooms [HullBreach];
            var securityCheckpointRoom = rooms [SecurityCheckpoint];
            var pathToSecurityCheck = GetPath (rooms, hullBreachRoom, securityCheckpointRoom);
            await FollowPath (intMachine, pathToSecurityCheck);
            var nextDirection = securityCheckpointRoom.Doors.Keys.Single (d => d != InverseDirections [pathToSecurityCheck.Last ()]);

            var inventories = collectedItems
                .Subsets ()
                .Select (x => x.OrderBy (y => y).ToList ())
                .Append (new List<string> ())
                .OrderBy (x => Math.Abs (collectedItems.Count () / 2 - x.Count))
                .ToList ();
            var currentInventory = inventories.First ();

            string password = null;
            foreach (var inv in inventories)
            {
                if (!currentInventory.SequenceEqual (inv))
                {
                    var stuffToDrop = currentInventory.Except (inv).ToList ();
                    var stuffToTake = inv.Except (currentInventory).ToList ();
                    foreach (var stuff in stuffToDrop)
                    {
                        _ = await RunMachineAsync (intMachine, string.Concat ("drop ", stuff));
                    }
                    foreach (var stuff in stuffToTake)
                    {
                        _ = await RunMachineAsync (intMachine, string.Concat ("take ", stuff));
                    }
                }
                currentInventory = inv;

                var response = await RunMachineAsync (intMachine, nextDirection);
                var roomNames = RoomNameRegex.Matches (response).OfType<Match> ().Select (x => x.Groups [1].Value).ToList ();
                if (roomNames.Count == 1)
                {
                    password = PasswordRegex.Match (response).Value;
                    break;
                }
            }

            return password;
        }

        public static string Part2 () => "*";

        private static async Task FollowPath (painter.SynchronousIntMachine intMachine, IEnumerable<string> path)
        {
            foreach (var direction in path)
            {
                _ = await RunMachineAsync (intMachine, direction);
            }
        }

        private static List<string> GetPath (Dictionary<string, Room> rooms, Room startRoom, Room targetRoom)
        {
            var queue = new Queue < (Room Room, List<string> Path) > (new []
            {
                (startRoom, new List<string> ())
            });
            var visited = new HashSet<Room> ();
            while (queue.Count > 0)
            {
                var (room, path) = queue.Dequeue ();
                if (!visited.Add (room)) { continue; }
                if (room == targetRoom) { return path; }

                foreach (var (direction, nextRoom) in room.Doors)
                {
                    queue.Enqueue ((nextRoom, path.Append (direction).ToList ()));
                }
            }

            return null;
        }

        private static async Task < (Dictionary<string, Room> Rooms, List<string> Items) > DiscoverRoomsAndGatherItemsAsync (painter.SynchronousIntMachine intMachine,
            Dictionary<string, Room> rooms = null, List<string> collectedItems = null, Room previousRoom = null, string previousDirection = null)
        {
            rooms = rooms ?? new Dictionary<string, Room> ();
            collectedItems = collectedItems ?? new List<string> ();
            var response = await RunMachineAsync (intMachine, previousDirection);
            ParseResponse (response, out var roomName, out var doors, out var items);

            if (!rooms.TryGetValue (roomName, out var room))
            {
                room = new Room (roomName);
                rooms.Add (roomName, room);
                doors.ForEach (door => room.Doors [door] = null);

                if (previousRoom != null)
                {
                    room.Doors [InverseDirections [previousDirection]] = previousRoom;
                    previousRoom.Doors [previousDirection] = room;
                }

                if (roomName != SecurityCheckpoint)
                {
                    var pickableItems = items.Except (BadItems);
                    foreach (var item in pickableItems)
                    {
                        response = await RunMachineAsync (intMachine, string.Concat ("take ", item));
                        collectedItems.Add (item);
                    }

                    foreach (var direction in doors)
                    {
                        if (previousDirection != null && direction == InverseDirections [previousDirection]) { continue; }
                        await DiscoverRoomsAndGatherItemsAsync (intMachine, rooms, collectedItems, room, direction);
                    }
                }
            }

            if (previousDirection != null)
            {
                response = await RunMachineAsync (intMachine, InverseDirections [previousDirection]); // Step back
            }

            return (rooms, collectedItems);
        }

        private static void ParseResponse (string response, out string roomName, out List<string> doors, out List<string> items)
        {
            if (isInteractiveOutput)
            {
                Console.Write (response);
            }
            var roomDescription = RoomDescriptionRegex.Match (response).Groups [1].Value;
            var doorList = DoorListRegex.Match (response).Groups [1].Value;
            var itemList = ItemListRegex.Match (response).Groups [1].Value;

            roomName = RoomNameRegex.Match (response).Groups [1].Value;
            doors = DoorRegex.Matches (doorList).OfType<Match> ().Select (x => x.Groups [1].Value).ToList ();
            items = ItemRegex.Matches (itemList).OfType<Match> ().Select (x => x.Groups [1].Value).ToList ();
        }

        private static async Task<string> RunMachineAsync (painter.SynchronousIntMachine intMachine, string input)
        {
            var sb = new StringBuilder ();
            string message = null;
            painter.SynchronousIntMachine.ReturnCode returnCode;
            while (message == null && (returnCode = intMachine.RunUntilBlockOrComplete ()) != painter.SynchronousIntMachine.ReturnCode.Completed)
            {
                switch (returnCode)
                {
                case painter.SynchronousIntMachine.ReturnCode.WaitingForInput:
                    if (input == null)
                    {
                        message = sb.ToString ();
                        break;
                    }
                    input.Append ('\n').ToList ().ForEach (x => intMachine.InputQueue.Enqueue (x));
                    if (isInteractiveOutput) { Console.WriteLine (input); }
                    input = null;
                    break;
                case painter.SynchronousIntMachine.ReturnCode.WrittenOutput:
                    while (intMachine.OutputQueue.Count > 0)
                    {
                        var value = intMachine.OutputQueue.Dequeue ();
                        if (value < 256)
                        {
                            sb.Append ((char) value);
                        }
                        else
                        {
                            Console.WriteLine ("Non ascii value: " + value);
                        }
                    }
                    break;
                }
            }

            message = message ?? sb.ToString ();
            if (isInteractiveOutput) { Console.WriteLine (message); }

            return message;
        }

        private static Dictionary<string, string> InverseDirections { get; set; } = new Dictionary<string, string> ()
        { { "north", "south" }, { "east", "west" }, { "south", "north" }, { "west", "east" }
        };
        private static string [] Directions = new [] { "north", "east", "south", "west" };

        private static Regex RoomNameRegex = new Regex (@"== ([\S ]+) ==");
        private static Regex RoomDescriptionRegex = new Regex (@"==\n(.+)\n\n");
        private static Regex DoorListRegex = new Regex (@"Doors here lead:\n((?:- [a-z]+\n)+)");
        private static Regex DoorRegex = new Regex ("- ([a-z]+)");
        private static Regex ItemListRegex = new Regex (@"Items here:\n((?:- [A-Za-z ]+\n)+)");
        private static Regex ItemRegex = new Regex ("- ([A-Za-z ]+)");
        private static Regex PasswordRegex = new Regex (@"[0-9]{4,}");

        private const string HullBreach = "Hull Breach";
        private const string SecurityCheckpoint = "Security Checkpoint";

        private static string [] BadItems = new string []
        {
            "giant electromagnet",
            "infinite loop",
            "molten lava",
            "escape pod",
            "photons"
        };
    }

    public class Room
    {
        public string Name { get; }
        public Dictionary<string, Room> Doors { get; } = new Dictionary<string, Room> ();
        public Room (string name) => Name = name;
    }
}